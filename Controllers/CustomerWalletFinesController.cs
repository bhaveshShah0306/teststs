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
    public class CustomerWalletFinesController : ControllerBase
    {
        private readonly DataContext _context;

        public CustomerWalletFinesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/CustomerWalletFines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerWalletFine>>> GetCustomerWalletFines()
        {
          if (_context.CustomerWalletFines == null)
          {
              return NotFound();
          }
            return await _context.CustomerWalletFines.ToListAsync();
        }

        // GET: api/CustomerWalletFines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerWalletFine>> GetCustomerWalletFine(int id)
        {
          if (_context.CustomerWalletFines == null)
          {
              return NotFound();
          }
            var customerWalletFine = await _context.CustomerWalletFines.FindAsync(id);

            if (customerWalletFine == null)
            {
                return NotFound();
            }

            return customerWalletFine;
        }

        // PUT: api/CustomerWalletFines/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<IActionResult> PutCustomerWalletFine(int id, CustomerWalletFine customerWalletFine)
        {
            if (id != customerWalletFine.CustomerWalletFineId)
            {
                return BadRequest();
            }

            _context.Entry(customerWalletFine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerWalletFineExists(id))
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

        // POST: api/CustomerWalletFines
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CustomerWalletFine>> PostCustomerWalletFine(CustomerWalletFine customerWalletFine)
        {
          if (_context.CustomerWalletFines == null)
          {
              return Problem("Entity set 'DataContext.CustomerWalletFines'  is null.");
          }
            _context.CustomerWalletFines.Add(customerWalletFine);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomerWalletFine", new { id = customerWalletFine.CustomerWalletFineId }, customerWalletFine);
        }

        // DELETE: api/CustomerWalletFines/5
        [HttpPost("{id}/delete")]
        public async Task<IActionResult> DeleteCustomerWalletFine(int id)
        {
            if (_context.CustomerWalletFines == null)
            {
                return NotFound();
            }
            var customerWalletFine = await _context.CustomerWalletFines.FindAsync(id);
            if (customerWalletFine == null)
            {
                return NotFound();
            }

            _context.CustomerWalletFines.Remove(customerWalletFine);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerWalletFineExists(int id)
        {
            return (_context.CustomerWalletFines?.Any(e => e.CustomerWalletFineId == id)).GetValueOrDefault();
        }
    }
}
