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
    public class DriverPenaltiesController : ControllerBase
    {
        private readonly DataContext _context;

        public DriverPenaltiesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/DriverPenalties
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DriverPenalty>>> GetDriverPenalties()
        {
            if (_context.DriverPenalties == null)
            {
                return NotFound();
            }
            return await _context.DriverPenalties.ToListAsync();
        }
        [HttpGet("{driverid}/driver")]
        public async Task<ActionResult<IEnumerable<DriverPenalty>>> GetDriverPenalties(int driverid)
        {
            if (_context.DriverPenalties == null)
            {
                return NotFound();
            }
            return await _context.DriverPenalties.Where(c => c.DriverId == driverid).ToListAsync();
        }

        // GET: api/DriverPenalties/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DriverPenalty>> GetDriverPenalty(int id)
        {
            if (_context.DriverPenalties == null)
            {
                return NotFound();
            }
            var driverPenalty = await _context.DriverPenalties.FindAsync(id);

            if (driverPenalty == null)
            {
                return NotFound();
            }

            return driverPenalty;
        }

        // PUT: api/DriverPenalties/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDriverPenalty(int id, DriverPenalty driverPenalty)
        {
            if (id != driverPenalty.DriverPenaltyId)
            {
                return BadRequest();
            }

            _context.Entry(driverPenalty).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriverPenaltyExists(id))
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

        // POST: api/DriverPenalties
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DriverPenalty>> PostDriverPenalty(DriverPenalty driverPenalty)
        {
            if (_context.DriverPenalties == null)
            {
                return Problem("Entity set 'DataContext.DriverPenalties'  is null.");
            }
            _context.DriverPenalties.Add(driverPenalty);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDriverPenalty", new { id = driverPenalty.DriverPenaltyId }, driverPenalty);
        }

        // DELETE: api/DriverPenalties/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriverPenalty(int id)
        {
            if (_context.DriverPenalties == null)
            {
                return NotFound();
            }
            var driverPenalty = await _context.DriverPenalties.FindAsync(id);
            if (driverPenalty == null)
            {
                return NotFound();
            }

            _context.DriverPenalties.Remove(driverPenalty);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DriverPenaltyExists(int id)
        {
            return (_context.DriverPenalties?.Any(e => e.DriverPenaltyId == id)).GetValueOrDefault();
        }
    }
}
