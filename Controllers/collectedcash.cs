using GoChauffeurWebApi.Data;
using GoChauffeurWebApi.Models;
using GoChauffeurWebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class collectedcash : ControllerBase
    {
        private readonly DataContext _context;

        public collectedcash(DataContext context)
        {
            _context = context;
        }

        [HttpPost]

        public async Task<ActionResult<dynamic>> collect(int id, int DriverId, int triptypeId,int tripprice, int flag,Boolean iscashcollected)
        {
            try
            {
                var tripdata = _context.Trips.Find(id);
                if (tripdata != null)
                {
                    tripdata.DriverId = DriverId;

                    if (tripdata.IsdriverArrived == true && tripdata.IsTripStarted == true)
                    {
                        tripdata.ActualEndTime = DateTime.Now;
                        tripdata.StartDateTime = tripdata.StartDateTime;
                        TimeSpan? timeDifference = DateTime.Now - tripdata.StartDateTime;
                        Decimal? taxpercentage = 0;
                        var taxdata = await _context.Taxes.FirstOrDefaultAsync();
                        if (taxdata != null)
                        {
                            taxpercentage = taxdata.Percentage;
                            
                        }
                        decimal? careprice = 0;
                        if (tripdata.TripTypeId != 3)
                        {

                            var insurencedata = await _context.InsurenceTaxandPrice.FirstOrDefaultAsync();
                            if (insurencedata != null)
                            {
                                var percentage = insurencedata.TaxPercentage;
                                var price = insurencedata.Price;
                                var value = price * (percentage / 100);

                                careprice = price+value;
                            }
                        }
                        else
                        {
                            var insurencedata = await _context.InsurenceTaxandPriceoutoffcity.FirstOrDefaultAsync();
                            if (insurencedata != null)
                            {
                                var percentage = insurencedata.TaxPercentage;
                                var price = insurencedata.Price;
                                var value = price * (percentage / 100);
                                careprice = price + value;
                            }
                        }

                        if (timeDifference.HasValue)
                        {
                            double hoursDifference = timeDifference.Value.TotalHours;
                            // Assign the calculated hours to ActualEndTime or any other property as needed
                            tripdata.ActualEndTime = DateTime.Now;
                            tripdata.NoOfHoursActual = Convert.ToInt32(hoursDifference);

                            var difference = (Convert.ToInt32(tripdata.NoOfHoursActual) - Convert.ToInt32(tripdata.NoOfHoursSelected));
                            var hoursdata = _context.Hours
                              .Where(c => c.TripTypeId == tripdata.TripTypeId && c.HoursName <= difference)
                              .OrderByDescending(c => c.HoursName) // Order by HoursName in descending order
                              .FirstOrDefault();


                            if (hoursdata != null)
                            {
                                if (difference <= 0)
                                {
                                    tripdata.TotalTripValue = tripdata.EstimatedPrice;
                                    var driversubdata = await _context.Driversubscriptions
                                      .Where(c => c.DriverId == tripdata.DriverId && c.Expirydate <= DateTime.Now)
                                      .OrderByDescending(c => c.DriversubscriptionId) // Sort by the latest date first
                                      .FirstOrDefaultAsync();
                                    if (driversubdata != null)
                                    {
                                        var subscriptiondata = _context.Subscriptions.Find(driversubdata.SubscriptionId);
                                        if (subscriptiondata != null)
                                        {
                                            var percentage = subscriptiondata.Percentage;
                                            if (percentage > 0)
                                            {
                                                int? anonymuscharges = 0;
                                                var anonymusdata = _context.Anonymoustripcharges.Where(c => c.TriptypeId == triptypeId).FirstOrDefault();
                                                if (anonymusdata != null)
                                                {
                                                    anonymuscharges=anonymusdata.Amount;
                                                }
                                                careprice = anonymuscharges;
                                                var taxvalue = Convert.ToInt32(tripprice) * (taxpercentage / 100);
                                                decimal? cuponnprice = 0;
                                                if(tripdata.CuponId!=null && tripdata.CuponId != 0)
                                                {

                                                    var cupondata = _context.Cupons.Find(tripdata.CuponId);
                                                    if (cupondata != null)
                                                    {
                                                        var price = Convert.ToInt32(tripprice) - taxvalue;
                                                        if (cupondata.Percentage != 0 && cupondata != null)
                                                        {
                                                            cuponnprice = price * (cupondata.Percentage / 100);
                                                        }
                                                        else 
                                                        {
                                                            cuponnprice = Convert.ToDecimal(cupondata.Price);
                                                        }
                                                    }
                                                }
                                                var drivervalue = (Convert.ToInt32(tripprice+ cuponnprice) - taxvalue - careprice) * (percentage / 100);
                                                tripdata.DriversPrice = Math.Round(drivervalue ?? 0, 0);

                                                var driverTransaction = new DriverTransaction()
                                                {
                                                    DriverId = DriverId,
                                                    Amount = -tripprice, // with gst amount
                                                    CreatedDate = DateTime.Now,
                                                    description = "Trip Payment",
                                                };
                                                    var driverwalletdata = _context.Driverwallets.Where(c => c.DriverId == DriverId).FirstOrDefault();
                                                if (iscashcollected == true)
                                                {
                                                    
                                                    if (driverwalletdata != null)
                                                    {
                                                        driverwalletdata.WalletBalance = (Convert.ToInt32(driverwalletdata.WalletBalance) - Convert.ToInt32(tripprice)).ToString();
                                                        _context.Entry(driverwalletdata).State = EntityState.Modified;
                                                        await _context.SaveChangesAsync();
                                                    }
                                                    _context.DriverTransactions.Add(driverTransaction);
                                                    await _context.SaveChangesAsync();
                                                }

                                                var driverTransaction1 = new DriverTransaction()
                                                {
                                                    DriverId = DriverId,
                                                    Amount = Convert.ToDecimal(drivervalue), // with gst amount
                                                    CreatedDate = DateTime.Now,
                                                    description = "Your Earning",
                                                };

                                                if (driverwalletdata != null)
                                                {
                                                    driverwalletdata.WalletBalance = (Convert.ToInt32(driverwalletdata.WalletBalance) + Convert.ToInt32(drivervalue)).ToString();
                                                    _context.Entry(driverwalletdata).State = EntityState.Modified;
                                                    await _context.SaveChangesAsync();
                                                }
                                                _context.DriverTransactions.Add(driverTransaction1);
                                                await _context.SaveChangesAsync();

                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    int minutes = difference * 60;
                                    decimal totalprice = 0;
                                    var tripvarientsdata = _context.TripVariants.Where(c => c.TriptypeId == tripdata.TripTypeId).FirstOrDefault();
                                    if (tripvarientsdata != null)
                                    {
                                        totalprice = minutes * Convert.ToInt32(tripvarientsdata.ChargesperMinute);
                                    }
                                    var driversubdata = _context.Driversubscriptions.Where(c => c.DriverId == tripdata.DriverId && c.Expirydate <= DateTime.Now).FirstOrDefault();
                                    if (driversubdata != null)
                                    {
                                        var subscriptiondata = _context.Subscriptions.Find(driversubdata.SubscriptionId);
                                        if (subscriptiondata != null)
                                        {
                                            var percentage = Convert.ToInt32(subscriptiondata.Percentage);
                                            if (percentage > 0)
                                            {
                                                tripdata.TotalTripValue = Math.Abs(Convert.ToDecimal(tripdata.EstimatedPrice) + totalprice) ;
                                                decimal totalValue = Convert.ToDecimal(tripdata.EstimatedPrice) + Convert.ToDecimal(totalprice);
                                                var taxvalue = Convert.ToInt32(totalValue) * (taxpercentage / 100);
                                                int? anonymuscharges = 0;
                                                var anonymusdata = _context.Anonymoustripcharges.Where(c => c.TriptypeId == triptypeId).FirstOrDefault();
                                                if (anonymusdata != null)
                                                {
                                                    anonymuscharges = anonymusdata.Amount;
                                                }
                                                decimal? cuponnprice = 0;
                                                if (tripdata.CuponId != null && tripdata.CuponId != 0)
                                                {

                                                    var cupondata = _context.Cupons.Find(tripdata.CuponId);
                                                    if (cupondata != null)
                                                    {
                                                        var price = Convert.ToInt32(tripprice) - taxvalue;
                                                        if (cupondata.Percentage != 0 && cupondata != null)
                                                        {
                                                            cuponnprice = price * (cupondata.Percentage / 100);
                                                        }
                                                        else
                                                        {
                                                            cuponnprice = Convert.ToDecimal(cupondata.Price);
                                                        }
                                                    }
                                                }
                                                // Calculate drivervalue using decimal arithmetic to preserve precision
                                                decimal drivervalue = ((totalValue+Convert.ToDecimal(cuponnprice)) - Convert.ToDecimal(taxvalue) - Convert.ToDecimal(careprice)-Convert.ToInt32(anonymuscharges)) * percentage / 100;
                                                tripdata.DriversPrice = Math.Round(drivervalue , 0);
                                                // Convert drivervalue to string for assignment
                                                tripdata.DriversPrice = drivervalue;
                                                var driverTransaction = new DriverTransaction()
                                                {
                                                    DriverId = DriverId,
                                                    Amount = -Convert.ToDecimal(tripprice), // with gst amount
                                                    CreatedDate = DateTime.Now,
                                                    description = "Trip Payment",
                                                };
                                                var driverwalletdata = _context.Driverwallets.Where(c => c.DriverId == DriverId).FirstOrDefault();
                                                if (iscashcollected == true)
                                                {

                                                    if (driverwalletdata != null)
                                                    {
                                                        driverwalletdata.WalletBalance = (Convert.ToInt32(driverwalletdata.WalletBalance) - Convert.ToInt32(tripprice)).ToString();
                                                        _context.Entry(driverwalletdata).State = EntityState.Modified;
                                                        await _context.SaveChangesAsync();
                                                    }
                                                }
                                                _context.DriverTransactions.Add(driverTransaction);
                                                await _context.SaveChangesAsync();

                                                var driverTransaction1 = new DriverTransaction()
                                                {
                                                    DriverId = DriverId,
                                                    Amount = Convert.ToDecimal(drivervalue), // with gst amount
                                                    CreatedDate = DateTime.Now,
                                                    description = "Your Earnings",
                                                };
                                                if (driverwalletdata != null)
                                                {
                                                    driverwalletdata.WalletBalance = (Convert.ToInt32(driverwalletdata.WalletBalance) + Convert.ToInt32(drivervalue)).ToString();
                                                    _context.Entry(driverwalletdata).State = EntityState.Modified;
                                                    await _context.SaveChangesAsync();
                                                }
                                                _context.DriverTransactions.Add(driverTransaction1);
                                                await _context.SaveChangesAsync();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var percentage = 80;
                                        if (percentage > 0)
                                        {
                                            tripdata.TotalTripValue = Math.Abs(Math.Round(Convert.ToDecimal(tripdata.EstimatedPrice), 1) + totalprice);
                                            // Convert EstimatedPrice and totalprice to decimal and add them
                                            decimal totalValue = Convert.ToDecimal(tripdata.EstimatedPrice) + Convert.ToDecimal(totalprice);

                                            // Calculate drivervalue using decimal arithmetic to preserve precision
                                            var taxvalue = Convert.ToInt32(totalValue) * (taxpercentage / 100);
                                            decimal? cuponnprice = 0;
                                            if (tripdata.CuponId != null && tripdata.CuponId != 0)
                                            {

                                                var cupondata = _context.Cupons.Find(tripdata.CuponId);
                                                if (cupondata != null)
                                                {
                                                    var price = Convert.ToInt32(tripprice) - taxvalue;
                                                    if (cupondata.Percentage != 0 && cupondata != null)
                                                    {
                                                        cuponnprice = price * (cupondata.Percentage / 100);
                                                    }
                                                    else
                                                    {
                                                        cuponnprice = Convert.ToDecimal(cupondata.Price);
                                                    }
                                                }
                                            }
                                            // Calculate drivervalue using decimal arithmetic to preserve precision
                                            decimal drivervalue = (totalValue+ Convert.ToDecimal(cuponnprice )- Convert.ToDecimal(taxvalue) - Convert.ToDecimal(careprice)) * percentage / 100;

                                            tripdata.DriversPrice = Math.Round(drivervalue, 0);
                                            // Convert drivervalue to string for assignment
                                            tripdata.DriversPrice = drivervalue;
                                            var driverTransaction = new DriverTransaction()
                                            {
                                                DriverId = DriverId,
                                                Amount = -Math.Round(Convert.ToDecimal(tripprice)), // with gst amount
                                                CreatedDate = DateTime.Now,
                                                description = "Trip Payment",
                                            };
                                            var driverwalletdata = _context.Driverwallets.Where(c => c.DriverId == DriverId).FirstOrDefault();

                                            if (iscashcollected == true) 
                                            { 
                                                if (driverwalletdata != null)
                                                {
                                                    driverwalletdata.WalletBalance = (Convert.ToInt32(driverwalletdata.WalletBalance) - Math.Round(Convert.ToDecimal(tripprice),0)).ToString();
                                                    _context.Entry(driverwalletdata).State = EntityState.Modified;
                                                    await _context.SaveChangesAsync();
                                                }
                                            }


                                            _context.DriverTransactions.Add(driverTransaction);
                                            await _context.SaveChangesAsync();

                                            var driverTransaction1 = new DriverTransaction()
                                            {
                                                DriverId = DriverId,
                                                Amount = Convert.ToDecimal(drivervalue), // with gst amount
                                                CreatedDate = DateTime.Now,
                                                description = "Your Earning",
                                            };
                                            if (driverwalletdata != null)
                                            {
                                                driverwalletdata.WalletBalance = (Convert.ToInt32(driverwalletdata.WalletBalance) + Convert.ToInt32(drivervalue)).ToString();
                                                _context.Entry(driverwalletdata).State = EntityState.Modified;
                                                await _context.SaveChangesAsync();
                                            }
                                            _context.DriverTransactions.Add(driverTransaction1);
                                            await _context.SaveChangesAsync();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                tripdata.TotalTripValue = tripdata.EstimatedPrice;

                                var driversubdata = _context.Driversubscriptions.Where(c => c.DriverId == tripdata.DriverId && c.Expirydate <= DateTime.Now).FirstOrDefault();
                                if (driversubdata != null)
                                {
                                    var subscriptiondata = _context.Subscriptions.Find(driversubdata.SubscriptionId);
                                    if (subscriptiondata != null)
                                    {
                                        var percentage = subscriptiondata.Percentage;
                                        if (percentage > 0)
                                        {
                                            // Calculate drivervalue using decimal arithmetic to preserve precision
                                            var taxvalue = Convert.ToInt32(tripprice) * (taxpercentage / 100);
                                            int? anonymuscharges = 0;
                                            var anonymusdata = _context.Anonymoustripcharges.Where(c => c.TriptypeId == triptypeId).FirstOrDefault();
                                            if (anonymusdata != null)
                                            {
                                                anonymuscharges = anonymusdata.Amount;
                                            }
                                            decimal? cuponnprice = 0;
                                            if (tripdata.CuponId != null && tripdata.CuponId != 0)
                                            {

                                                var cupondata = _context.Cupons.Find(tripdata.CuponId);
                                                if (cupondata != null)
                                                {
                                                    var price = Convert.ToInt32(tripprice) - taxvalue;
                                                    if (cupondata.Percentage != 0 && cupondata != null)
                                                    {
                                                        cuponnprice = price * (cupondata.Percentage / 100);
                                                    }
                                                    else
                                                    {
                                                        cuponnprice = Convert.ToDecimal(cupondata.Price);
                                                    }
                                                }
                                            }
                                            // Calculate drivervalue using decimal arithmetic to preserve precision
                                            var drivervalue = (Convert.ToInt32(tripdata.TotalTripValue)+Convert.ToDecimal(cuponnprice )- Convert.ToDecimal(taxvalue) - Convert.ToDecimal(careprice)- anonymuscharges) * percentage / 100;

                                            tripdata.DriversPrice = drivervalue;
                                            var driverTransaction = new DriverTransaction()
                                            {
                                                DriverId = DriverId,
                                                Amount = -Math.Round(Convert.ToDecimal(tripprice)), // with gst amount
                                                CreatedDate = DateTime.Now,
                                                description = "Trip Payment",
                                            };
                                            var driverwalletdata = _context.Driverwallets.Where(c => c.DriverId == DriverId).FirstOrDefault();
                                            if (iscashcollected == true)
                                            {
                                                if (driverwalletdata != null)
                                                {
                                                    driverwalletdata.WalletBalance = (Convert.ToInt32(driverwalletdata.WalletBalance) - Math.Round(Convert.ToDecimal(tripprice), 0)).ToString();
                                                    _context.Entry(driverwalletdata).State = EntityState.Modified;
                                                    await _context.SaveChangesAsync();
                                                }
                                            }

                                            _context.DriverTransactions.Add(driverTransaction);
                                            await _context.SaveChangesAsync();

                                            var driverTransaction1 = new DriverTransaction()
                                            {
                                                DriverId = DriverId,
                                                Amount = Convert.ToDecimal(drivervalue), // with gst amount
                                                CreatedDate = DateTime.Now,
                                                description = "Your Earnings",
                                            };
                                            if (driverwalletdata != null)
                                            {
                                                driverwalletdata.WalletBalance = (Convert.ToInt32(driverwalletdata.WalletBalance) + Convert.ToInt32(drivervalue)).ToString();
                                                _context.Entry(driverwalletdata).State = EntityState.Modified;
                                                await _context.SaveChangesAsync();
                                            }
                                            _context.DriverTransactions.Add(driverTransaction1);
                                            await _context.SaveChangesAsync();
                                        }
                                    }
                                }
                                else
                                {
                                    var percentage = 80;
                                    if (percentage > 0)
                                    {
                                        tripdata.TotalTripValue = Math.Abs(Convert.ToDecimal(tripdata.EstimatedPrice));
                                        // Convert EstimatedPrice and totalprice to decimal and add them
                                        decimal totalValue = Convert.ToDecimal(tripdata.EstimatedPrice);

                                        var taxvalue = Convert.ToInt32(totalValue) * (taxpercentage / 100);
                                        int? anonymuscharges = 0;
                                        var anonymusdata = _context.Anonymoustripcharges.Where(c => c.TriptypeId == triptypeId).FirstOrDefault();
                                        if (anonymusdata != null)
                                        {
                                            anonymuscharges = anonymusdata.Amount;
                                        }
                                        decimal? cuponnprice = 0;
                                        if (tripdata.CuponId != null && tripdata.CuponId != 0)
                                        {

                                            var cupondata = _context.Cupons.Find(tripdata.CuponId);
                                            if (cupondata != null)
                                            {
                                                var price = Convert.ToInt32(tripprice) - taxvalue;
                                                if (cupondata.Percentage != 0 && cupondata != null)
                                                {
                                                    cuponnprice = price * (cupondata.Percentage / 100);
                                                }
                                                else
                                                {
                                                    cuponnprice = Convert.ToDecimal(cupondata.Price);
                                                }
                                            }
                                        }
                                        // Calculate drivervalue using decimal arithmetic to preserve precision
                                        decimal drivervalue = (totalValue+Convert.ToDecimal(cuponnprice) - Convert.ToDecimal(taxvalue) - Convert.ToDecimal(careprice)) * percentage / 100;

                                        // Convert drivervalue to string for assignment
                                        tripdata.DriversPrice = drivervalue;
                                        var driverTransaction = new DriverTransaction()
                                        {
                                            DriverId = DriverId,
                                            Amount = -Math.Round(Convert.ToDecimal(tripdata.TotalTripValue)), // with gst amount
                                            CreatedDate = DateTime.Now,
                                            description = "Trip Payment",
                                        };
                                        var driverwalletdata = _context.Driverwallets.Where(c => c.DriverId == DriverId).FirstOrDefault();
                                        if (iscashcollected == true)
                                        {
                                            if (driverwalletdata != null)
                                            {
                                                driverwalletdata.WalletBalance = (Convert.ToInt32(driverwalletdata.WalletBalance) - Math.Round(Convert.ToDecimal(tripdata.TotalTripValue), 0)).ToString();
                                                _context.Entry(driverwalletdata).State = EntityState.Modified;
                                                await _context.SaveChangesAsync();
                                            }
                                        }

                                        _context.DriverTransactions.Add(driverTransaction);
                                        await _context.SaveChangesAsync();

                                        var driverTransaction1 = new DriverTransaction()
                                        {
                                            DriverId = DriverId,
                                            Amount = Convert.ToDecimal(drivervalue), // with gst amount
                                            CreatedDate = DateTime.Now,
                                            description = "Your Earnings",
                                        };
                                        if (driverwalletdata != null)
                                        {
                                            driverwalletdata.WalletBalance = (Convert.ToInt32(driverwalletdata.WalletBalance) + Convert.ToInt32(drivervalue)).ToString();
                                            _context.Entry(driverwalletdata).State = EntityState.Modified;
                                            await _context.SaveChangesAsync();
                                        }
                                        _context.DriverTransactions.Add(driverTransaction1);
                                        await _context.SaveChangesAsync();
                                    }
                                }
                            }


                            // Optionally, update StartDateTime if necessary
                            // tripdata.StartDateTime = tripdata.StartDateTime; // This line seems redundant unless you have a specific reason to reassign StartDateTime to itself


                        }
                        else
                        {
                            tripdata.TotalTripValue = tripdata.EstimatedPrice;

                            var driversubdata = _context.Driversubscriptions.Where(c => c.DriverId == tripdata.DriverId && c.Expirydate <= DateTime.Now).FirstOrDefault();
                            if (driversubdata != null)
                            {
                                var subscriptiondata = _context.Subscriptions.Find(driversubdata.SubscriptionId);
                                if (subscriptiondata != null)
                                {
                                    var percentage = subscriptiondata.Percentage;
                                    if (percentage > 0)
                                    {
                                        int? anonymuscharges = 0;
                                        var anonymusdata = _context.Anonymoustripcharges.Where(c => c.TriptypeId == triptypeId).FirstOrDefault();
                                        if (anonymusdata != null)
                                        {
                                            anonymuscharges = anonymusdata.Amount;
                                        }
                                        var taxvalue = Convert.ToInt32(tripprice) * (taxpercentage / 100);
                                        decimal? cuponnprice = 0;
                                        if (tripdata.CuponId != null && tripdata.CuponId != 0)
                                        {

                                            var cupondata = _context.Cupons.Find(tripdata.CuponId);
                                            if (cupondata != null)
                                            {
                                                var price = Convert.ToInt32(tripprice) - taxvalue;
                                                if (cupondata.Percentage != 0 && cupondata != null)
                                                {
                                                    cuponnprice = price * (cupondata.Percentage / 100);
                                                }
                                                else
                                                {
                                                    cuponnprice = Convert.ToDecimal(cupondata.Price);
                                                }
                                            }
                                        }
                                        // Calculate drivervalue using decimal arithmetic to preserve precision
                                        var drivervalue = (Convert.ToInt32(tripprice)+Convert.ToDecimal(cuponnprice )- taxvalue - Convert.ToDecimal(careprice)- anonymuscharges) * (percentage / 100);
                                        tripdata.DriversPrice = drivervalue;
                                        var driverTransaction = new DriverTransaction()
                                        {
                                            DriverId = DriverId,
                                            Amount = -Math.Round(Convert.ToDecimal(tripdata.TotalTripValue)), // with gst amount
                                            CreatedDate = DateTime.Now,
                                            description = "Trip Payment",
                                        };
                                        var driverwalletdata = _context.Driverwallets.Where(c => c.DriverId == DriverId).FirstOrDefault();
                                        if (iscashcollected == true)
                                        {
                                            if (driverwalletdata != null)
                                            {
                                                driverwalletdata.WalletBalance = (Convert.ToInt32(driverwalletdata.WalletBalance) - Math.Round(Convert.ToDecimal(tripdata.TotalTripValue), 0)).ToString();
                                                _context.Entry(driverwalletdata).State = EntityState.Modified;
                                                await _context.SaveChangesAsync();
                                            }
                                        }
                                        _context.DriverTransactions.Add(driverTransaction);
                                        await _context.SaveChangesAsync();

                                        var driverTransaction1 = new DriverTransaction()
                                        {
                                            DriverId = DriverId,
                                            Amount = Convert.ToDecimal(drivervalue), // with gst amount
                                            CreatedDate = DateTime.Now,
                                            description = "Trip Payment",
                                        };
                                        if (driverwalletdata != null)
                                        {
                                            driverwalletdata.WalletBalance = (Convert.ToInt32(driverwalletdata.WalletBalance) + Convert.ToInt32(drivervalue)).ToString();
                                            _context.Entry(driverwalletdata).State = EntityState.Modified;
                                            await _context.SaveChangesAsync();
                                        }
                                        _context.DriverTransactions.Add(driverTransaction1);
                                        await _context.SaveChangesAsync();
                                    }
                                }
                            }
                            else
                            {
                                var percentage = 80;
                                if (percentage > 0)
                                {
                                    tripdata.TotalTripValue = Math.Abs(Convert.ToDecimal(tripdata.EstimatedPrice));
                                    // Convert EstimatedPrice and totalprice to decimal and add them
                                    decimal totalValue = Convert.ToDecimal(tripdata.EstimatedPrice);
                                    var taxvalue = Convert.ToInt32(totalValue) * (taxpercentage / 100);
                                    // Calculate drivervalue using decimal arithmetic to preserve precision
                                    decimal? cuponnprice = 0;
                                    if (tripdata.CuponId != null && tripdata.CuponId != 0)
                                    {

                                        var cupondata = _context.Cupons.Find(tripdata.CuponId);
                                        if (cupondata != null)
                                        {
                                            var price = Convert.ToInt32(tripprice) - taxvalue;
                                            if (cupondata.Percentage != 0 && cupondata != null)
                                            {
                                                cuponnprice = price * (cupondata.Percentage / 100);
                                            }
                                            else
                                            {
                                                cuponnprice = Convert.ToDecimal(cupondata.Price);
                                            }
                                        }
                                    }
                                    decimal drivervalue = (totalValue+Convert.ToDecimal(cuponnprice) - Convert.ToDecimal(taxvalue) - Convert.ToDecimal(careprice)) * percentage / 100;

                                    // Convert drivervalue to string for assignment
                                    tripdata.DriversPrice = drivervalue;
                                    var driverTransaction = new DriverTransaction()
                                    {
                                        DriverId = DriverId,
                                        Amount = -Math.Round(Convert.ToDecimal(tripprice)), // with gst amount
                                        CreatedDate = DateTime.Now,
                                        description = "Trip Payment",
                                    };
                                    var driverwalletdata = _context.Driverwallets.Where(c => c.DriverId == DriverId).FirstOrDefault();
                                    if (iscashcollected == true)
                                    {
                                        if (driverwalletdata != null)
                                        {
                                            driverwalletdata.WalletBalance = (Convert.ToInt32(driverwalletdata.WalletBalance) - Math.Round(Convert.ToDecimal(tripprice), 0)).ToString();
                                            _context.Entry(driverwalletdata).State = EntityState.Modified;
                                            await _context.SaveChangesAsync();
                                        }
                                    }
                                    _context.DriverTransactions.Add(driverTransaction);
                                    await _context.SaveChangesAsync();

                                    var driverTransaction1 = new DriverTransaction()
                                    {
                                        DriverId = DriverId,
                                        Amount = Convert.ToDecimal(drivervalue), // with gst amount
                                        CreatedDate = DateTime.Now,
                                        description = "Your Earnings",
                                    };
                                    if (driverwalletdata != null)
                                    {
                                        driverwalletdata.WalletBalance = (Convert.ToInt32(driverwalletdata.WalletBalance) + Convert.ToInt32(drivervalue)).ToString();
                                        _context.Entry(driverwalletdata).State = EntityState.Modified;
                                        await _context.SaveChangesAsync();
                                    }
                                    _context.DriverTransactions.Add(driverTransaction1);
                                    await _context.SaveChangesAsync();
                                }
                            }
                        }

                        tripdata.ActualEndTime = DateTime.Now;
                        tripdata.IsTripCompByDriver = true;
                       
                        tripdata.IsPaymentdone = true;
                    }
                    else
                    {
                        return BadRequest("Driver arival is not clearly done");
                    }


                    _context.Entry(tripdata).State = EntityState.Modified;
                    await _context.SaveChangesAsync();



                    return Ok(tripdata);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
