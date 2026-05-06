using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GoChauffeurWebApi.Data;
using GoChauffeurWebApi.Models;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerwalletTransactionHistoriesController : ControllerBase
    {
        private readonly DataContext _context;

        public CustomerwalletTransactionHistoriesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/CustomerwalletTransactionHistories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerwalletTransactionHistory>>> GetcustomerwalletTransactionHistories()
        {
          if (_context.customerwalletTransactionHistories == null)
          {
              return NotFound();
          }
            return await _context.customerwalletTransactionHistories.ToListAsync();
        }
        
        [HttpGet("{userid}/flag")]
        public async Task<ActionResult<IEnumerable<CustomerwalletTransactionHistory>>> GetcustomerwalletTransactionHistories(int userid)
        {
          if (_context.customerwalletTransactionHistories == null)
          {
              return NotFound();
          }
            return await _context.customerwalletTransactionHistories.Where(c=>c.UserId == userid).ToListAsync();
        }

        // GET: api/CustomerwalletTransactionHistories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerwalletTransactionHistory>> GetCustomerwalletTransactionHistory(int id)
        {
          if (_context.customerwalletTransactionHistories == null)
          {
              return NotFound();
          }
            var customerwalletTransactionHistory = await _context.customerwalletTransactionHistories.FindAsync(id);

            if (customerwalletTransactionHistory == null)
            {
                return NotFound();
            }

            return customerwalletTransactionHistory;
        }

        // PUT: api/CustomerwalletTransactionHistories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerwalletTransactionHistory(int id, CustomerwalletTransactionHistory customerwalletTransactionHistory)
        {
            if (id != customerwalletTransactionHistory.CustomerwalletTransactionHistoryId)
            {
                return BadRequest();
            }

            _context.Entry(customerwalletTransactionHistory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerwalletTransactionHistoryExists(id))
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

        // POST: api/CustomerwalletTransactionHistories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CustomerwalletTransactionHistory>> PostCustomerwalletTransactionHistory(CustomerwalletTransactionHistory customerwalletTransactionHistory)
        {
          if (_context.customerwalletTransactionHistories == null)
          {
              return Problem("Entity set 'DataContext.customerwalletTransactionHistories'  is null.");
          }
            _context.customerwalletTransactionHistories.Add(customerwalletTransactionHistory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomerwalletTransactionHistory", new { id = customerwalletTransactionHistory.CustomerwalletTransactionHistoryId }, customerwalletTransactionHistory);
        }

        // DELETE: api/CustomerwalletTransactionHistories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerwalletTransactionHistory(int id)
        {
            if (_context.customerwalletTransactionHistories == null)
            {
                return NotFound();
            }
            var customerwalletTransactionHistory = await _context.customerwalletTransactionHistories.FindAsync(id);
            if (customerwalletTransactionHistory == null)
            {
                return NotFound();
            }

            _context.customerwalletTransactionHistories.Remove(customerwalletTransactionHistory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerwalletTransactionHistoryExists(int id)
        {
            return (_context.customerwalletTransactionHistories?.Any(e => e.CustomerwalletTransactionHistoryId == id)).GetValueOrDefault();
        }
    }
}
