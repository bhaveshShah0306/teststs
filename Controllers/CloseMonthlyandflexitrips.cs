using GoChauffeurWebApi.Data;
using GoChauffeurWebApi.Models;
using GoChauffeurWebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CloseMonthlyandflexitrips : ControllerBase
    {
        private readonly DataContext _context;

        public CloseMonthlyandflexitrips(DataContext context)
        {
            _context = context;
        }



        [HttpPost]
        public async Task<ActionResult<dynamic>> Closetrips (int tripid, int driverid, Boolean iscashcollected, int flag,Boolean? ispaymentdone)
        {
            try
            {
                if (flag == 1)
                {
                    var flexidata = await _context.Flexis.FindAsync(tripid);
                    if (flexidata != null)
                    {
                        if(flexidata.DriverId == driverid)
                        {
                            Decimal? finalprice = 0;
                            int? estimatedhours = 0;
                            var flexidatelist = _context.FlexiDatesLists.Where(c => c.FlexiId == flexidata.FlexiId && c.IsTripCompByDriver == true).ToList();
                            if (flexidatelist.Count > 0)
                            {
                                foreach (var item in flexidatelist)
                                {
                                    finalprice += item.ActualPrice;
                                    estimatedhours += item.ActualHours;
                                }
                            }

                             finalprice = finalprice - flexidata.AdvancePaid;
                            var totalfinaleprice = finalprice;

                            var taxdata = await _context.Taxes.FirstOrDefaultAsync();
                            decimal? taxprice = 0;
                            if (taxdata != null)
                            {
                                var taxpercentage =taxdata.Percentage;
                                var price = totalfinaleprice * (taxpercentage / 100);
                                taxprice = price;
                            }
                            totalfinaleprice = totalfinaleprice ;
                           
                            var driverwalletdata = _context.Driverwallets.Where(c => c.DriverId == driverid).FirstOrDefault();
                            if (iscashcollected == true)
                            {

                                var anonymuscharges = await _context.Anonymoustripcharges.Where(c => c.TriptypeId == 4).FirstOrDefaultAsync();
                                var driversubdata = await _context.Driversubscriptions
                                             .Where(c => c.DriverId == driverid && c.Expirydate <= DateTime.Now)
                                             .OrderByDescending(c => c.DriversubscriptionId) // Sort by the latest date first
                                             .FirstOrDefaultAsync();
                                taxdata = _context.Taxes.FirstOrDefault();
                                var taxpercentage = 0;
                                if (taxdata != null)
                                {
                                    taxpercentage = Convert.ToInt32(taxdata.Percentage);
                                }
                                        int drivervalue = 0;
                                if (driversubdata != null)
                                {
                                    var subdata = _context.Subscriptions.Find(driversubdata.SubscriptionId);
                                    if (subdata != null && subdata.Percentage != null && subdata.Percentage != 0)
                                    {
                                        var percetage = subdata.Percentage;
                                        var taxvalue = totalfinaleprice * taxpercentage / 100;

                                        if (anonymuscharges != null && anonymuscharges.Amount.HasValue)
                                        {
                                            drivervalue = Convert.ToInt32(totalfinaleprice - taxvalue - Convert.ToInt32(anonymuscharges.Amount.Value) * percetage / 100);
                                        }
                                        else
                                        {
                                            // Handle the case when anonymuscharges or Amount is null
                                            drivervalue = Convert.ToInt32(totalfinaleprice - taxvalue * percetage / 100);
                                        }
                                    }
                                    else
                                    {
                                        var taxvalue = totalfinaleprice * taxpercentage / 100;


                                        if (anonymuscharges != null && anonymuscharges.Amount.HasValue)
                                        {
                                            drivervalue = Convert.ToInt32( totalfinaleprice - taxvalue - Convert.ToInt32(anonymuscharges.Amount.Value));
                                        }
                                        else
                                        {
                                            // Handle the case when anonymuscharges or Amount is null
                                            drivervalue = Convert.ToInt32(totalfinaleprice - taxvalue);
                                        }
                                    }



                                }
                                else
                                {
                                    var percetage = 80;
                                    var taxvalue = totalfinaleprice * taxpercentage / 100;


                                    drivervalue = Convert.ToInt32(totalfinaleprice - taxvalue - Convert.ToInt32(anonymuscharges.Amount.Value) * percetage / 100);
                                    // Handle the case when anonymuscharges or Amount is null
                                    drivervalue =Convert.ToInt32( totalfinaleprice - taxvalue * percetage / 100);

                                }
                                var driverTransaction = new DriverTransaction()
                                {
                                    DriverId = driverid,
                                    Amount = -totalfinaleprice, // with gst amount
                                    CreatedDate = DateTime.Now,
                                    description = "Trip Payment",
                                };

                                if (driverwalletdata != null)
                                {
                                    driverwalletdata.WalletBalance = (Convert.ToInt32(driverwalletdata.WalletBalance) - Math.Round(Convert.ToDecimal(totalfinaleprice), 0)).ToString();
                                    _context.Entry(driverwalletdata).State = EntityState.Modified;
                                    await _context.SaveChangesAsync();
                                }
                                _context.DriverTransactions.Add(driverTransaction);
                                await _context.SaveChangesAsync();

                                var driverTransaction1 = new DriverTransaction()
                                {
                                    DriverId = driverid,
                                    Amount = drivervalue, // with gst amount
                                    CreatedDate = DateTime.Now,
                                    description = "Your Earnings",
                                };
                                _context.DriverTransactions.Add(driverTransaction1);
                                await _context.SaveChangesAsync();
                                if (driverwalletdata != null)
                                {
                                    driverwalletdata.WalletBalance = (Convert.ToInt32(driverwalletdata.WalletBalance) + Math.Round(Convert.ToDecimal(drivervalue), 0)).ToString();
                                    _context.Entry(driverwalletdata).State = EntityState.Modified;
                                    await _context.SaveChangesAsync();
                                }
                            }
                            flexidata.FinalPrice = finalprice;
                            flexidata.CloseTrip = true;

                            if (ispaymentdone == true)
                            {
                                flexidata.IsPaymentdone = ispaymentdone;
                            }
                            flexidata.EstimatedHours = estimatedhours;
                            _context.Entry(flexidata).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                            return Ok(flexidata);
                        }
                        
                    }
                }
                else if(flag == 2)
                {
                    var monthlydata = await _context.Monthlies.FindAsync(tripid);
                    if (monthlydata != null)
                    {
                        if(monthlydata.DriverId == driverid)
                        {

                            int? finalprice = 0;
                            int? estimatedhours= 0;
                            var mopnthlydatelist = _context.MonthlyDateLists.Where(c => c.MonthlyId == monthlydata.MonthlyId && c.IsTripCompByDriver == true).ToList();
                            if (mopnthlydatelist.Count > 0)
                            {
                                foreach (var item in mopnthlydatelist)
                                {
                                    finalprice +=Convert.ToInt32( item.ActualPrice);
                                    estimatedhours += item.ActualHours;
                                }
                            }
                            if (monthlydata.AdvancePaid == null)
                            {
                                monthlydata.AdvancePaid = 0;
                            }
                            var totalfinaleprice = finalprice;
                            finalprice = Convert.ToInt32(finalprice - monthlydata.AdvancePaid);

                            var taxdata = await _context.Taxes.FirstOrDefaultAsync();
                            decimal? taxprice = 0;
                            if (taxdata != null)
                            {
                                var taxpercentage = taxdata.Percentage;
                                var price = totalfinaleprice * (taxpercentage / 100);
                                taxprice = price;
                            }
                            totalfinaleprice = totalfinaleprice ;
                            var driverwalletdata = _context.Driverwallets.Where(c => c.DriverId == driverid).FirstOrDefault();
                            if (iscashcollected == true)
                            {
                                var anonymuscharges = await _context.Anonymoustripcharges.Where(c => c.TriptypeId == 4).FirstOrDefaultAsync();
                                var driversubdata = await _context.Driversubscriptions
                                             .Where(c => c.DriverId == driverid && c.Expirydate <= DateTime.Now)
                                             .OrderByDescending(c => c.DriversubscriptionId) // Sort by the latest date first
                                             .FirstOrDefaultAsync();
                                taxdata = _context.Taxes.FirstOrDefault();
                                var taxpercentage = 0;
                                if (taxdata != null)
                                {
                                    taxpercentage = Convert.ToInt32(taxdata.Percentage);
                                }
                                int drivervalue = 0;
                                if (driversubdata != null)
                                {
                                    var subdata = _context.Subscriptions.Find(driversubdata.SubscriptionId);
                                    if (subdata != null && subdata.Percentage != null && subdata.Percentage != 0)
                                    {
                                        var percetage = subdata.Percentage;
                                        var taxvalue = totalfinaleprice * taxpercentage / 100;

                                        if (anonymuscharges != null && anonymuscharges.Amount.HasValue)
                                        {
                                            drivervalue = Convert.ToInt32(totalfinaleprice - taxvalue - Convert.ToInt32(anonymuscharges.Amount.Value) * percetage / 100);
                                        }
                                        else
                                        {
                                            // Handle the case when anonymuscharges or Amount is null
                                            drivervalue = Convert.ToInt32(totalfinaleprice - taxvalue * percetage / 100);
                                        }
                                    }
                                    else
                                    {
                                        var taxvalue = totalfinaleprice * taxpercentage / 100;


                                        if (anonymuscharges != null && anonymuscharges.Amount.HasValue)
                                        {
                                            drivervalue = Convert.ToInt32(totalfinaleprice - taxvalue - Convert.ToInt32(anonymuscharges.Amount.Value));
                                        }
                                        else
                                        {
                                            // Handle the case when anonymuscharges or Amount is null
                                            drivervalue = Convert.ToInt32(totalfinaleprice - taxvalue);
                                        }
                                    }



                                }
                                else
                                {
                                    var percetage = 80;
                                    var taxvalue = totalfinaleprice * taxpercentage / 100;


                                    drivervalue = Convert.ToInt32(totalfinaleprice - taxvalue - Convert.ToInt32(anonymuscharges.Amount.Value) * percetage / 100);
                                    // Handle the case when anonymuscharges or Amount is null
                                    drivervalue = Convert.ToInt32(totalfinaleprice - taxvalue * percetage / 100);

                                }
                                var driverTransaction = new DriverTransaction()
                                {
                                    DriverId = driverid,
                                    Amount = -Math.Round(Convert.ToDecimal(totalfinaleprice), 0), // with gst amount
                                    CreatedDate = DateTime.Now,
                                    description = "Trip Payment",
                                };

                                if (driverwalletdata != null)
                                {
                                    driverwalletdata.WalletBalance = (Convert.ToInt32(driverwalletdata.WalletBalance) - Math.Round(Convert.ToDecimal(totalfinaleprice), 0)).ToString();
                                    _context.Entry(driverwalletdata).State = EntityState.Modified;
                                    await _context.SaveChangesAsync();
                                }
                                _context.DriverTransactions.Add(driverTransaction);
                                await _context.SaveChangesAsync();

                                var driverTransaction1 = new DriverTransaction()
                                {
                                    DriverId = driverid,
                                    Amount = drivervalue, // with gst amount
                                    CreatedDate = DateTime.Now,
                                    description = "Trip Payment",
                                };
                                _context.DriverTransactions.Add(driverTransaction1);
                                await _context.SaveChangesAsync();
                                if (driverwalletdata != null)
                                {
                                    driverwalletdata.WalletBalance = (Convert.ToInt32(driverwalletdata.WalletBalance) + Math.Round(Convert.ToDecimal(drivervalue), 0) - Convert.ToInt32(taxprice)).ToString();
                                    _context.Entry(driverwalletdata).State = EntityState.Modified;
                                    await _context.SaveChangesAsync();
                                }
                            }
                            monthlydata.FinalPrice = finalprice;
                            monthlydata.EstimatedHours= Convert.ToInt32(estimatedhours);
                            monthlydata.CloseTrip = true;
                            if (ispaymentdone == true)
                            {
                                monthlydata.IsPaymentdone = ispaymentdone;
                            }
                            _context.Entry(monthlydata).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                            return Ok(monthlydata);
                        }
                    }
                }
              
                return BadRequest("Trip not found or driver ID mismatch.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
