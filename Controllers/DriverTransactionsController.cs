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
    public class DriverTransactionsController : ControllerBase
    {
        private readonly DataContext _context;

        public DriverTransactionsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/DriverTransactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DriverTransaction>>> GetDriverTransactions()
        {
          if (_context.DriverTransactions == null)
          {
              return NotFound();
          }
            return await _context.DriverTransactions.ToListAsync();
        }

        [HttpGet("{driverid}/DriverTransactions")]
   
        public async Task<ActionResult<IEnumerable<DriverTransactionsVM>>> GetDriverDriverTransactions(int driverid)
        {
            try
            {
                var driverTransactionHistoryModel = await _context.DriverTransactions
                                                      .Where(c => c.DriverId == driverid)
                                                      .OrderByDescending(c => c.DriverTransactionId) 
                                                      .ToListAsync();


                if (driverTransactionHistoryModel.Count == 0)
                {
                    return NotFound($"No transactions found for driver ID {driverid}");
                }

                // Convert the transactions to ViewModel
                var driverTransactionVM = driverTransactionHistoryModel.Select(driverTransaction => new DriverTransactionsVM
                {
                    DriverTransactionId = driverTransaction.DriverTransactionId,
                    DriverId = driverTransaction.DriverId,
                    transactionId = driverTransaction.transactionId,
                    Amount = driverTransaction.Amount,
                    CreatedDate = driverTransaction.CreatedDate,
                    IsJoiningFee = driverTransaction.IsJoiningFee,
                    description = driverTransaction.description
                }).ToList();

                
                return Ok(driverTransactionVM);
            }
            catch (Exception ex)
            {
              
                return StatusCode(500, "Internal server error");
            }
        }


        // GET: api/DriverTransactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DriverTransaction>> GetDriverTransaction(int id)
        {
          if (_context.DriverTransactions == null)
          {
              return NotFound();
          }
            var driverTransaction = await _context.DriverTransactions.FindAsync(id);

            if (driverTransaction == null)
            {
                return NotFound();
            }

            return driverTransaction;
        }

        // PUT: api/DriverTransactions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDriverTransaction(int id, DriverTransaction driverTransaction)
        {
            if (id != driverTransaction.DriverTransactionId)
            {
                return BadRequest();
            }

            _context.Entry(driverTransaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriverTransactionExists(id))
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

        // POST: api/DriverTransactions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DriverTransaction>> PostDriverTransaction(DriverTransaction driverTransaction)
        {
          if (_context.DriverTransactions == null)
          {
              return Problem("Entity set 'DataContext.DriverTransactions'  is null.");
          }
            _context.DriverTransactions.Add(driverTransaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDriverTransaction", new { id = driverTransaction.DriverTransactionId }, driverTransaction);
        }

        // DELETE: api/DriverTransactions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriverTransaction(int id)
        {
            if (_context.DriverTransactions == null)
            {
                return NotFound();
            }
            var driverTransaction = await _context.DriverTransactions.FindAsync(id);
            if (driverTransaction == null)
            {
                return NotFound();
            }

            _context.DriverTransactions.Remove(driverTransaction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DriverTransactionExists(int id)
        {
            return (_context.DriverTransactions?.Any(e => e.DriverTransactionId == id)).GetValueOrDefault();
        }
    }
}
