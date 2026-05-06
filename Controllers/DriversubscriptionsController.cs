using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GoChauffeurWebApi.Data;
using GoChauffeurWebApi.Models;
using GoChauffeurWebApi.ViewModels;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversubscriptionsController : ControllerBase
    {
        private readonly DataContext _context;

        public DriversubscriptionsController(DataContext context)
        {
            _context = context;
        }

   
        // GET: api/Driversubscriptions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DriversubscriptionTransctionVM>>> GetDriversubscriptions()
        {
            try
            {
                // Fetching all driver subscriptions from the database
                var DriversubscriptionsModelList = await _context.Driversubscriptions.ToListAsync();
                var DriversubscriptionsVmList = new List<DriversubscriptionTransctionVM>();

                if (DriversubscriptionsModelList.Any())
                {
                    // Iterating through each subscription model to create the view model
                    foreach (var driversubModel in DriversubscriptionsModelList)
                    {
                        var driversubvm = new DriversubscriptionTransctionVM();
                            


                          driversubvm.DriversubscriptionId = driversubModel.DriversubscriptionId;
                        driversubvm. DriverId = driversubModel.DriverId;
                        var driverdata = _context.Drivers.Find(driversubvm.DriverId);
                        if (driverdata != null)
                        {
                            driversubvm.DriverName = driverdata.DriverName;
                            driversubvm.PhoneNumber = driverdata.PhoneNumber;
                        }
                        driversubvm. Expirydate = driversubModel.Expirydate;
                        driversubvm.SubscriptionId = driversubModel.SubscriptionId;
                        

                        // Fetching the subscription details using the SubscriptionId
                        var subscription = await _context.Subscriptions.FindAsync(driversubvm.SubscriptionId);
                        if (subscription != null)
                        {
                         
                            driversubvm.SubName = subscription.SubName;
                            driversubvm.Duration = subscription.Duration;
                            driversubvm.SubPrice = subscription.SubPrice;
                            driversubvm.SubBenefits = subscription.SubBenefits;
                            driversubvm.SubDescription = subscription.SubDescription;
                            driversubvm.Status = subscription.Status;
                            driversubvm.IsDriverSub = subscription.IsDriverSub;
                        }

                        DriversubscriptionsVmList.Add(driversubvm);
                    }

                    return Ok(DriversubscriptionsVmList);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // GET: api/Driversubscriptions
        [HttpGet("Driversubscriptions/{driverid}")]
        public async Task<ActionResult<IEnumerable<DriversubscriptionTransctionVM>>> GetDriversubscriptionsdriver( int driverid)
        {
            try
            {
                var driversubModel = await _context.Driversubscriptions
                                      .Where(c => c.DriverId == driverid && c.Expirydate <= DateTime.Now)
                                      .OrderByDescending(c => c.DriversubscriptionId) // Sort by the latest date first
                                      .FirstOrDefaultAsync();
                var DriversubscriptionsVmList = new List<DriversubscriptionTransctionVM>();

                if (driversubModel != null)
                {
                   
                        var driversubvm = new DriversubscriptionTransctionVM();
                        driversubvm.DriversubscriptionId = driversubModel.DriversubscriptionId;
                        driversubvm.transactionId = driversubModel.transactionId;
                        driversubvm.Amount = driversubModel.Amount;
                        driversubvm.CreatedDate = driversubModel.CreatedDate;
                        driversubvm.DriverId = driversubModel.DriverId;

                        var driverdata = _context.Drivers.Find(driversubvm.DriverId);
                        if (driverdata != null)
                        {
                            driversubvm.DriverName = driverdata.DriverName;
                            driversubvm.PhoneNumber = driverdata.PhoneNumber;
                        }

                        driversubvm.SubscriptionId = driversubModel.SubscriptionId;
                        var subscription = _context.Subscriptions.Find(driversubvm.SubscriptionId);
                        if (subscription != null)
                        {
                            driversubvm.SubName = subscription.SubName;
                            driversubvm.Duration = subscription.Duration;
                            driversubvm.SubPrice = subscription.SubPrice;
                            driversubvm.SubBenefits = subscription.SubBenefits;
                            driversubvm.SubDescription = subscription.SubDescription;
                            driversubvm.Status = subscription.Status;
                            driversubvm.IsDriverSub = subscription.IsDriverSub;
                            driversubvm.AfterGsttotalsubamount = subscription.AfterGsttotalsubamount;
                        }
                     
                        if (driversubModel.CreatedDate.HasValue)
                        {
                            DateTime createdDate = driversubModel.CreatedDate.Value;
                            int durationInDays = Convert.ToInt32(driversubvm.Duration);

                            DateTime subscriptionExpiryDate = createdDate.AddDays(durationInDays);
                            driversubvm.Expirydate = subscriptionExpiryDate;
                        }
                        else
                        {
                            driversubvm.Expirydate = null;
                        }

                        // Add to the view model list
                        DriversubscriptionsVmList.Add(driversubvm);
                    // Save changes to the database
                    _context.SaveChanges();

                    return Ok(DriversubscriptionsVmList);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        // GET: api/Driversubscriptions/5   
        [HttpGet("{id}")]
        public async Task<ActionResult<Driversubscription>> GetDriversubscription(int id)
        {
          if (_context.Driversubscriptions == null)
          {
              return NotFound();
          }
            var driversubscription = await _context.Driversubscriptions.FindAsync(id);

            if (driversubscription == null)
            {
                return NotFound();
            }

            return driversubscription;
        }

        // PUT: api/Driversubscriptions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDriversubscription(int id, Driversubscription driversubscription)
        {
            if (id != driversubscription.DriversubscriptionId)
            {
                return BadRequest();
            }

            _context.Entry(driversubscription).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriversubscriptionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Driversubscriptions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Driversubscription>> PostDriversubscription(Driversubscription driversubscription)
        {
            if (_context.Driversubscriptions == null)
            {
                return Problem("Entity set 'DataContext.Driversubscriptions' is null.");
            }

            var subdata = _context.Subscriptions.Find(driversubscription.SubscriptionId);
            if (subdata != null)
            {
                driversubscription.Expirydate = DateTime.Now.AddDays(Convert.ToInt32(subdata.Duration));
            }
            _context.Driversubscriptions.Add(driversubscription);
            await _context.SaveChangesAsync();

            // 1)Update or create the corresponding entry in DriverTransactions
            var driverTransaction = _context.DriverTransactions
                .Where(dt => dt.DriverId == driversubscription.DriverId)
                .FirstOrDefault();
            var driversubscriptionData = _context.Subscriptions.Find(driversubscription.SubscriptionId);
            if (driversubscriptionData != null)
            {
                var subAmountwithoutgst = driversubscriptionData.SubPrice;
                var SubscripationGstId = driversubscriptionData.SubscripationGstId;

                var gstdata = _context.SubscripationGst.Find(SubscripationGstId);
                if(gstdata != null)
                {
                    var gstvalue  = gstdata.GstPercentage;
                    var gstAmount = subAmountwithoutgst * (Convert.ToDecimal(gstvalue) / 100);
                    var totalgstplussubAmountwithoutgst = subAmountwithoutgst + gstAmount;
                    var driverWallets = _context.Driverwallets
                   .Where(dt => dt.DriverId == dt.DriverId)
                   .FirstOrDefault();
                    if (driverWallets != null)
                    {
                        driverWallets.WalletBalance = (Convert.ToDecimal(driverWallets.WalletBalance) + Convert.ToDecimal(subAmountwithoutgst)).ToString();
                        _context.Entry(driverWallets).State = EntityState.Modified;
                    }
                    else
                    {
                        var driverWalletData = new Driverwallet()
                        {
                            DriverId = driversubscription.DriverId,
                            WalletBalance = subAmountwithoutgst.ToString(), // without gst amount
                        };
                        _context.Driverwallets.Add(driverWalletData);
                    }

                    //var subscriptionPriceWithGST =  gstAmount;
                    
                    }
            }
           
            //DriverWalletTransactionHistories == reciveed amt
            // 2)Update or create the corresponding entry in DriverWalletTransactionHistories
            var driverWalletHistoryTransaction = _context.DriverWalletTransactionHistories
                .Where(dt => dt.DriverWalletTransactionHistoryId == dt.DriverWalletTransactionHistoryId)
                .FirstOrDefault();
            var driversubscriptionHistoryData = _context.Subscriptions.Find(driversubscription.SubscriptionId);
            if (driversubscriptionHistoryData != null)
            {
                var subAmountwithoutgst = driversubscriptionHistoryData.SubPrice;
               
                var driverWalletTransaction = new DriverWalletTransactionHistory()
                {
                    DriverId = driversubscription.DriverId,
                    WalletAmount = subAmountwithoutgst, // without gst amount
                    CreatedDate = driversubscription.CreatedDate,
                    transactionId = driversubscription.transactionId,
                    description = "Subscriptions"
                };
                _context.DriverWalletTransactionHistories.Add(driverWalletTransaction);
            }
            await _context.SaveChangesAsync();


            // 3)Update or create the corresponding entry in Driverwallets
            var driverWallet = _context.Driverwallets
                .Where(dt => dt.DriverId == dt.DriverId)
                .FirstOrDefault();
            var driversubscriptionWalletData = _context.Subscriptions.Find(driversubscription.SubscriptionId);
            if (driversubscriptionWalletData != null)
            {
                var subAmountwithoutgst = Convert.ToString(driversubscriptionWalletData.SubPrice);
                var subGstAmountwithgst = driversubscriptionWalletData.AfterGsttotalsubamount;

                var driverWalletData = new Driverwallet()
                {
                    DriverId = driversubscription.DriverId,
                    WalletBalance = subAmountwithoutgst, // without gst amount
                };

                _context.Driverwallets.Add(driverWalletData);
            }
            else
            {
                var driverWallets = _context.Driverwallets
                   .Where(dt => dt.DriverId == driversubscription.DriverId)
                   .FirstOrDefault();
                if (driverWallets != null)
                {
                    driverWallets.WalletBalance = (Convert.ToDecimal(driverWallets.WalletBalance) + Convert.ToDecimal(driversubscription.Amount)).ToString();
                    _context.Entry(driverWallets).State = EntityState.Modified;
                }
                else
                {
                    var driverWalletData = new Driverwallet()
                    {
                        DriverId = driversubscription.DriverId,
                        WalletBalance = driversubscription.Amount.ToString(), // without gst amount
                    };
                    _context.Driverwallets.Add(driverWalletData);
                }
                driverTransaction = new DriverTransaction()
                {
                    DriverId = driversubscription.DriverId,
                    transactionId = driversubscription.transactionId,
                    Amount = driversubscription.Amount, // with gst amount
                    CreatedDate = driversubscription.CreatedDate,
                    IsJoiningFee = true,
                    description = "",
                };
                _context.DriverTransactions.Add(driverTransaction);
                await _context.SaveChangesAsync();
            }
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDriversubscription", new { id = driversubscription.DriversubscriptionId }, driversubscription);
        }

        // DELETE: api/Driversubscriptions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriversubscription(int id)
        {
            if (_context.Driversubscriptions == null)
            {
                return NotFound();
            }
            var driversubscription = await _context.Driversubscriptions.FindAsync(id);
            if (driversubscription == null)
            {
                return NotFound();
            }

            _context.Driversubscriptions.Remove(driversubscription);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DriversubscriptionExists(int id)
        {
            return (_context.Driversubscriptions?.Any(e => e.DriversubscriptionId == id)).GetValueOrDefault();
        }
    }
}
