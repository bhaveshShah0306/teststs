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
    public class DriverTicketsReasonsController : ControllerBase
    {
        private readonly DataContext _context;

        public DriverTicketsReasonsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/DriverTicketsReasons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DriverTicketsReason>>> GetDriverTicketsReasons()
        {
          if (_context.DriverTicketsReasons == null)
          {
              return NotFound();
          }
            return await _context.DriverTicketsReasons.ToListAsync();
        }
        
        [HttpGet("{type}/flag")]
        public async Task<ActionResult<IEnumerable<DriverTicketsReason>>> GetDriverTicketsReasonsbytype(int type)
        {
          if (_context.DriverTicketsReasons == null)
          {
              return NotFound();
          }
            return await _context.DriverTicketsReasons.Where(c=>c.Type==type).ToListAsync();
        }

        // GET: api/DriverTicketsReasons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DriverTicketsReason>> GetDriverTicketsReason(int id)
        {
          if (_context.DriverTicketsReasons == null)
          {
              return NotFound();
          }
            var driverTicketsReason = await _context.DriverTicketsReasons.FindAsync(id);

            if (driverTicketsReason == null)
            {
                return NotFound();
            }

            return driverTicketsReason;
        }

        // PUT: api/DriverTicketsReasons/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<IActionResult> PutDriverTicketsReason(int id, DriverTicketsReason driverTicketsReason)
        {
            if (id != driverTicketsReason.DriverTicketsReasonId)
            {
                return BadRequest();
            }

            _context.Entry(driverTicketsReason).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriverTicketsReasonExists(id))
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

        // POST: api/DriverTicketsReasons
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DriverTicketsReason>> PostDriverTicketsReason(DriverTicketsReason driverTicketsReason)
        {
          if (_context.DriverTicketsReasons == null)
          {
              return Problem("Entity set 'DataContext.DriverTicketsReasons'  is null.");
          }
            _context.DriverTicketsReasons.Add(driverTicketsReason);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDriverTicketsReason", new { id = driverTicketsReason.DriverTicketsReasonId }, driverTicketsReason);
        }

        // DELETE: api/DriverTicketsReasons/5
        [HttpPost("{id}/delete")]
        public async Task<IActionResult> DeleteDriverTicketsReason(int id)
        {
            if (_context.DriverTicketsReasons == null)
            {
                return NotFound();
            }
            var driverTicketsReason = await _context.DriverTicketsReasons.FindAsync(id);
            if (driverTicketsReason == null)
            {
                return NotFound();
            }

            _context.DriverTicketsReasons.Remove(driverTicketsReason);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DriverTicketsReasonExists(int id)
        {
            return (_context.DriverTicketsReasons?.Any(e => e.DriverTicketsReasonId == id)).GetValueOrDefault();
        }
    }
}
