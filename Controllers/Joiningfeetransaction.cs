using GoChauffeurWebApi.Data;
using GoChauffeurWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Joiningfeetransaction : ControllerBase
    {
        private readonly DataContext _context;

        public Joiningfeetransaction(DataContext context)
        {
            _context = context;
        }


        [HttpPost]
        public async Task<ActionResult<DriverTransaction>> updatetransaction(int driverid,Decimal? Amount,string? transactionId)
        {
            try
            {

                var driverdata = _context.Drivers.Find(driverid);
                if (driverdata == null)
                {
                    return NoContent();
                }
                var transaction = new DriverTransaction();

                transaction.transactionId = transactionId;
                transaction.DriverId = driverid;
                transaction.Amount = Amount;
                _context.DriverTransactions.Add(transaction);
                await _context.SaveChangesAsync();

                driverdata.IsPaymentDone = true;

                _context.Entry(driverdata).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(transaction);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
