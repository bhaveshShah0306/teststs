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
    public class CustomerTicketsController : ControllerBase
    {
        private readonly DataContext _context;

        public CustomerTicketsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/CustomerTickets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerTicketsVM>>> GetCustomerTickets()
        {
            try
            {
                var ticketsdata = _context.CustomerTickets.ToList();
                var ticketsvmlist = new List<CustomerTicketsVM>();
                if (ticketsdata.Count > 0)
                {
                    foreach (var ticket in ticketsdata)
                    {
                        var ticketvm = new CustomerTicketsVM()
                        {
                            CustomerTicketsId = ticket.CustomerTicketsId,
                            UserId = ticket.UserId,
                            TicketId = ticket.TicketId,
                            TicketName = "",
                            Reasons = ticket.Reasons,
                            IsTicketClosed = ticket.IsTicketClosed,
                        };
                        if (ticket.TicketId != null)
                        {

                            var ticcketdata = await _context.DriverTicketsReasons.FindAsync(ticket.TicketId);
                            if (ticcketdata != null)
                            {
                                ticketvm.TicketName = ticcketdata.DriverTicketsReasonName;
                            }
                        }

                        if (ticket.UserId != null)
                        {
                            var driverdata = await _context.Users.FindAsync(ticket.UserId);
                            if (driverdata != null)
                            {
                                ticketvm.UserName = driverdata.Name;
                                ticketvm.Number = driverdata.PhoneNumber;

                            }
                        }

                        ticketsvmlist.Add(ticketvm);
                    }
                    return Ok(ticketsvmlist);
                }
                else
                {
                    return NoContent();
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/CustomerTickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerTickets>> GetCustomerTickets(int id)
        {
          if (_context.CustomerTickets == null)
          {
              return NotFound();
          }
            var customerTickets = await _context.CustomerTickets.FindAsync(id);

            if (customerTickets == null)
            {
                return NotFound();
            }

            return customerTickets;
        }

        // PUT: api/CustomerTickets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<IActionResult> PutCustomerTickets(int id, CustomerTickets customerTickets)
        {
            if (id != customerTickets.CustomerTicketsId)
            {
                return BadRequest();
            }

            _context.Entry(customerTickets).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerTicketsExists(id))
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

        // POST: api/CustomerTickets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CustomerTickets>> PostCustomerTickets(CustomerTickets customerTickets)
        {
          if (_context.CustomerTickets == null)
          {
              return Problem("Entity set 'DataContext.CustomerTickets'  is null.");
          }
            _context.CustomerTickets.Add(customerTickets);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomerTickets", new { id = customerTickets.CustomerTicketsId }, customerTickets);
        }

        // DELETE: api/CustomerTickets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerTickets(int id)
        {
            if (_context.CustomerTickets == null)
            {
                return NotFound();
            }
            var customerTickets = await _context.CustomerTickets.FindAsync(id);
            if (customerTickets == null)
            {
                return NotFound();
            }

            _context.CustomerTickets.Remove(customerTickets);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerTicketsExists(int id)
        {
            return (_context.CustomerTickets?.Any(e => e.CustomerTicketsId == id)).GetValueOrDefault();
        }
    }
}
