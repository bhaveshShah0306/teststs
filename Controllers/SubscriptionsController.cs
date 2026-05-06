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
    public class SubscriptionsController : ControllerBase
    {
        private readonly DataContext _context;

        public SubscriptionsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Subscriptions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubscriptionVM>>> GetSubscriptions()
        {
            try
            {

                var subscriptionModelList = _context.Subscriptions.ToList();
                var DriversubscriptionsVmList = new List<SubscriptionVM>();

                if (subscriptionModelList.Any())
                {

                    foreach (var subscription in subscriptionModelList)
                    {
                        var driversubvm = new SubscriptionVM();
                        driversubvm.SubscriptionId = subscription.SubscriptionId;
                        driversubvm.SubName = subscription.SubName;
                        driversubvm.Duration = subscription.Duration;
                        driversubvm.SubPrice = subscription.SubPrice;
                        driversubvm.SubBenefits = subscription.SubBenefits;
                        driversubvm.SubDescription = subscription.SubDescription;
                        driversubvm.Status = subscription.Status;
                        driversubvm.Percentage = subscription.Percentage;
                        driversubvm.IsDriverSub = subscription.IsDriverSub;
                        driversubvm.SubscripationGstId = subscription.SubscripationGstId;
                        var subdata = _context.SubscripationGst.Find(driversubvm.SubscripationGstId);
                        if(subdata != null)
                        {
                            driversubvm.SubscripationGstId = subdata.SubscripationGstId;
                            driversubvm.SubscripationName = subdata.SubscripationName;
                            driversubvm.GstPercentage = subdata.GstPercentage;  
                        }

                        if (driversubvm.GstPercentage != null)
                        {

                            var gstAmount = driversubvm.SubPrice * (Convert.ToDecimal(driversubvm.GstPercentage) / 100);
                            var subscriptionPriceWithGST = driversubvm.SubPrice + gstAmount;
                            driversubvm.AfterGsttotalsubamount = subscriptionPriceWithGST;
                        }
                        subscription.AfterGsttotalsubamount = driversubvm.AfterGsttotalsubamount;

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

        // GET: api/Subscriptions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Subscription>> GetSubscription(int id)
        {
          if (_context.Subscriptions == null)
          {
              return NotFound();
          }
            var subscription = await _context.Subscriptions.FindAsync(id);

            if (subscription == null)
            {
                return NotFound();
            }

            return subscription;
        }

        // PUT: api/Subscriptions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<IActionResult> PutSubscription(int id, Subscription subscription)
        {
            if (id != subscription.SubscriptionId)
            {
                return BadRequest();
            }

            _context.Entry(subscription).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubscriptionExists(id))
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

        // POST: api/Subscriptions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Subscription>> PostSubscription(Subscription subscription)
        {
          if (_context.Subscriptions == null)
          {
              return Problem("Entity set 'DataContext.Subscriptions'  is null.");
          }
            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSubscription", new { id = subscription.SubscriptionId }, subscription);
        }

        // DELETE: api/Subscriptions/5
        [HttpPost("{id}/Delete")]
        public async Task<IActionResult> DeleteSubscription(int id)
        {
            if (_context.Subscriptions == null)
            {
                return NotFound();
            }
            var subscription = await _context.Subscriptions.FindAsync(id);
            if (subscription == null)
            {
                return NotFound();
            }

            _context.Subscriptions.Remove(subscription);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SubscriptionExists(int id)
        {
            return (_context.Subscriptions?.Any(e => e.SubscriptionId == id)).GetValueOrDefault();
        }
    }
}
