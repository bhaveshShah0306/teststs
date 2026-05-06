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
    public class DriverTicketsController : ControllerBase
    {
        private readonly DataContext _context;

        public DriverTicketsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/DriverTickets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DriverTicketsVM>>> GetDriverTickets()
        {
            try
            {

                var ticketsdata = _context.DriverTickets.ToList();
                var ticketsvmlist = new List<DriverTicketsVM>();
                if(ticketsdata.Count > 0)
                {
                    foreach (var ticket in ticketsdata) 
                    {
                        var ticketvm = new DriverTicketsVM()
                        {
                            DriverTicketsId = ticket.DriverTicketsId,
                            DriverId = ticket.DriverId,
                            TicketId = ticket.TicketId,
                            TicketName = "",
                            Reasons= ticket.Reasons,
                            IsTicketClosed= ticket.IsTicketClosed,
                        };
                        if (ticket.TicketId!=null) 
                        {

                            var ticcketdata = await _context.DriverTicketsReasons.FindAsync(ticket.TicketId);
                            if(ticcketdata != null)
                            {
                                ticketvm.TicketName = ticcketdata.DriverTicketsReasonName;
                            }
                        }

                        if(ticket.DriverId!=null)
                        {
                            var driverdata = await _context.Drivers.FindAsync(ticket.DriverId);
                            if (driverdata != null)
                            {
                                ticketvm.DriverName = driverdata.DriverName;
                                ticketvm.Number= driverdata.PhoneNumber;
                                
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        // GET: api/DriverTickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DriverTickets>> GetDriverTickets(int id)
        {
          if (_context.DriverTickets == null)
          {
              return NotFound();
          }
            var driverTickets = await _context.DriverTickets.FindAsync(id);

            if (driverTickets == null)
            {
                return NotFound();
            }

            return driverTickets;
        }

        // PUT: api/DriverTickets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<IActionResult> PutDriverTickets(int id, DriverTickets driverTickets)
        {
            if (id != driverTickets.DriverTicketsId)
            {
                return BadRequest();
            }

            _context.Entry(driverTickets).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriverTicketsExists(id))
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

        // POST: api/DriverTickets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DriverTickets>> PostDriverTickets(DriverTickets driverTickets)
        {
          if (_context.DriverTickets == null)
          {
              return Problem("Entity set 'DataContext.DriverTickets'  is null.");
          }
            _context.DriverTickets.Add(driverTickets);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDriverTickets", new { id = driverTickets.DriverTicketsId }, driverTickets);
        }

        // DELETE: api/DriverTickets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriverTickets(int id)
        {
            if (_context.DriverTickets == null)
            {
                return NotFound();
            }
            var driverTickets = await _context.DriverTickets.FindAsync(id);
            if (driverTickets == null)
            {
                return NotFound();
            }

            _context.DriverTickets.Remove(driverTickets);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DriverTicketsExists(int id)
        {
            return (_context.DriverTickets?.Any(e => e.DriverTicketsId == id)).GetValueOrDefault();
        }
    }
}
