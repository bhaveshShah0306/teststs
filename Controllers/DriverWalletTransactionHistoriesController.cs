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
using System.Security.Cryptography.Xml;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverWalletTransactionHistoriesController : ControllerBase
    {
        private readonly DataContext _context;

        public DriverWalletTransactionHistoriesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/DriverWalletTransactionHistories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DriverWalletTransactionHistory>>> GetDriverWalletTransactionHistories()
        {
          if (_context.DriverWalletTransactionHistories == null)
          {
              return NotFound();
          }
            return await _context.DriverWalletTransactionHistories.ToListAsync();
        }


        [HttpGet("{driverid}/drwtranhistories")]
        public async Task<ActionResult<IEnumerable<DriverWalletTransactionHistoriesVM>>> GetDriverWalletTransactionHistories(int driverid)
        {
            try
            {
                var driverTransactionHistoryModel = await _context.DriverWalletTransactionHistories
                                                                  .Where(c => c.DriverId == driverid)
                                                                  .ToListAsync();

                if (driverTransactionHistoryModel != null && driverTransactionHistoryModel.Any())
                {
                    var driverSubVm = driverTransactionHistoryModel.Select(driverTransaction => new DriverWalletTransactionHistoriesVM
                    {
                        DriverWalletTransactionHistoryId = driverTransaction.DriverWalletTransactionHistoryId,
                        transactionId = driverTransaction.transactionId,
                        DriverId = driverTransaction.DriverId,
                        CreatedDate = driverTransaction.CreatedDate,
                        WalletAmount = driverTransaction.WalletAmount,
                        description = driverTransaction.description
                    }).ToList();

                    return Ok(driverSubVm);
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

        // GET: api/DriverWalletTransactionHistories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DriverWalletTransactionHistory>> GetDriverWalletTransactionHistory(int id)
        {
          if (_context.DriverWalletTransactionHistories == null)
          {
              return NotFound();
          }
            var driverWalletTransactionHistory = await _context.DriverWalletTransactionHistories.FindAsync(id);

            if (driverWalletTransactionHistory == null)
            {
                return NotFound();
            }

            return driverWalletTransactionHistory;
        }

        // PUT: api/DriverWalletTransactionHistories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDriverWalletTransactionHistory(int id, DriverWalletTransactionHistory driverWalletTransactionHistory)
        {
            if (id != driverWalletTransactionHistory.DriverWalletTransactionHistoryId)
            {
                return BadRequest();
            }

            _context.Entry(driverWalletTransactionHistory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriverWalletTransactionHistoryExists(id))
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

        // POST: api/DriverWalletTransactionHistories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DriverWalletTransactionHistory>> PostDriverWalletTransactionHistory(DriverWalletTransactionHistory driverWalletTransactionHistory)
        {
          if (_context.DriverWalletTransactionHistories == null)
          {
              return Problem("Entity set 'DataContext.DriverWalletTransactionHistories'  is null.");
          }
            _context.DriverWalletTransactionHistories.Add(driverWalletTransactionHistory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDriverWalletTransactionHistory", new { id = driverWalletTransactionHistory.DriverWalletTransactionHistoryId }, driverWalletTransactionHistory);
        }

        // DELETE: api/DriverWalletTransactionHistories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriverWalletTransactionHistory(int id)
        {
            if (_context.DriverWalletTransactionHistories == null)
            {
                return NotFound();
            }
            var driverWalletTransactionHistory = await _context.DriverWalletTransactionHistories.FindAsync(id);
            if (driverWalletTransactionHistory == null)
            {
                return NotFound();
            }

            _context.DriverWalletTransactionHistories.Remove(driverWalletTransactionHistory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DriverWalletTransactionHistoryExists(int id)
        {
            return (_context.DriverWalletTransactionHistories?.Any(e => e.DriverWalletTransactionHistoryId == id)).GetValueOrDefault();
        }
    }
}
