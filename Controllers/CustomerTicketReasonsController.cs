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
    public class CustomerTicketReasonsController : ControllerBase
    {
        private readonly DataContext _context;

        public CustomerTicketReasonsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/CustomerTicketReasons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerTicketReasons>>> GetCustomerTicketReasons()
        {
          if (_context.CustomerTicketReasons == null)
          {
              return NotFound();
          }
            return await _context.CustomerTicketReasons.ToListAsync();
        }

        // GET: api/CustomerTicketReasons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerTicketReasons>> GetCustomerTicketReasons(int id)
        {
          if (_context.CustomerTicketReasons == null)
          {
              return NotFound();
          }
            var customerTicketReasons = await _context.CustomerTicketReasons.FindAsync(id);

            if (customerTicketReasons == null)
            {
                return NotFound();
            }

            return customerTicketReasons;
        }

        // PUT: api/CustomerTicketReasons/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<IActionResult> PutCustomerTicketReasons(int id, CustomerTicketReasons customerTicketReasons)
        {
            if (id != customerTicketReasons.CustomerTicketReasonsId)
            {
                return BadRequest();
            }

            _context.Entry(customerTicketReasons).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerTicketReasonsExists(id))
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

        // POST: api/CustomerTicketReasons
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CustomerTicketReasons>> PostCustomerTicketReasons(CustomerTicketReasons customerTicketReasons)
        {
          if (_context.CustomerTicketReasons == null)
          {
              return Problem("Entity set 'DataContext.CustomerTicketReasons'  is null.");
          }
            _context.CustomerTicketReasons.Add(customerTicketReasons);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomerTicketReasons", new { id = customerTicketReasons.CustomerTicketReasonsId }, customerTicketReasons);
        }

        // DELETE: api/CustomerTicketReasons/5
        [HttpPost("{id}/delete")]
        public async Task<IActionResult> DeleteCustomerTicketReasons(int id)
        {
            if (_context.CustomerTicketReasons == null)
            {
                return NotFound();
            }
            var customerTicketReasons = await _context.CustomerTicketReasons.FindAsync(id);
            if (customerTicketReasons == null)
            {
                return NotFound();
            }

            _context.CustomerTicketReasons.Remove(customerTicketReasons);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerTicketReasonsExists(int id)
        {
            return (_context.CustomerTicketReasons?.Any(e => e.CustomerTicketReasonsId == id)).GetValueOrDefault();
        }
    }
}
