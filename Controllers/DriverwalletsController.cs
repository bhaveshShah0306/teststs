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
    public class DriverwalletsController : ControllerBase
    {
        private readonly DataContext _context;

        public DriverwalletsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Driverwallets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Driverwallet>>> GetDriverwallets()
        {
          if (_context.Driverwallets == null)
          {
              return NotFound();
          }
            return await _context.Driverwallets.ToListAsync();
        }

        // GET: api/Driverwallets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Driverwallet>> GetDriverwallet(int id)
        {
          if (_context.Driverwallets == null)
          {
              return NotFound();
          }
            var driverwallet = await _context.Driverwallets.FindAsync(id);

            if (driverwallet == null)
            {
                return NotFound();
            }

            return driverwallet;
        }


        [HttpGet("{driverid}/driverwallet")]
        public async Task<ActionResult<DriverwalletsVM>> GetDriverWallet(int driverid)
        {
            try
            {
                var driverTransactionHistoryModel = _context.Driverwallets.Where(c => c.DriverId == driverid).FirstOrDefault();

                if (driverTransactionHistoryModel != null)
                {
                    var driverWalletVM = new DriverwalletsVM
                    {
                        DriverwalletId = driverTransactionHistoryModel.DriverwalletId,
                        WalletBalance = driverTransactionHistoryModel.WalletBalance,
                        DriverId = driverTransactionHistoryModel.DriverId,
                        DriverWalletDeductionResonsId = driverTransactionHistoryModel.DriverWalletDeductionResonsId
                    };

                    var driverWalletDeductionReasonsData = _context.DriverWalletDeductionResons.Find(driverWalletVM.DriverWalletDeductionResonsId);
                    if (driverWalletDeductionReasonsData != null)
                    {
                        driverWalletVM.DriverWalletDeductionResonsName = driverWalletDeductionReasonsData.DriverWalletDeductionResonsName;
                    }
                    return Ok(driverWalletVM);
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

        // PUT: api/Driverwallets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDriverwallet(int id, Driverwallet driverwallet)
        {
            if (id != driverwallet.DriverwalletId)
            {
                return BadRequest();
            }

            _context.Entry(driverwallet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriverwalletExists(id))
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

        // POST: api/Driverwallets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Driverwallet>> PostDriverwallet(Driverwallet driverwallet)
        {
          if (_context.Driverwallets == null)
          {
              return Problem("Entity set 'DataContext.Driverwallets'  is null.");
          }
            _context.Driverwallets.Add(driverwallet);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDriverwallet", new { id = driverwallet.DriverwalletId }, driverwallet);
        }

        // DELETE: api/Driverwallets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriverwallet(int id)
        {
            if (_context.Driverwallets == null)
            {
                return NotFound();
            }
            var driverwallet = await _context.Driverwallets.FindAsync(id);
            if (driverwallet == null)
            {
                return NotFound();
            }

            _context.Driverwallets.Remove(driverwallet);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DriverwalletExists(int id)
        {
            return (_context.Driverwallets?.Any(e => e.DriverwalletId == id)).GetValueOrDefault();
        }
    }
}
