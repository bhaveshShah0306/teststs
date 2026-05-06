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
    public class IgnoredTripsController : ControllerBase
    {
        private readonly DataContext _context;

        public IgnoredTripsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/IgnoredTrips
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IgnoredTrips>>> GetIgnoredTrips()
        {
          if (_context.IgnoredTrips == null)
          {
              return NotFound();
          }
            return await _context.IgnoredTrips.ToListAsync();
        }

        // GET: api/IgnoredTrips/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IgnoredTrips>> GetIgnoredTrips(int id)
        {
          if (_context.IgnoredTrips == null)
          {
              return NotFound();
          }
            var ignoredTrips = await _context.IgnoredTrips.FindAsync(id);

            if (ignoredTrips == null)
            {
                return NotFound();
            }

            return ignoredTrips;
        }

        // PUT: api/IgnoredTrips/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIgnoredTrips(int id, IgnoredTrips ignoredTrips)
        {
            if (id != ignoredTrips.IgnoredTripsID)
            {
                return BadRequest();
            }

            _context.Entry(ignoredTrips).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IgnoredTripsExists(id))
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

        // POST: api/IgnoredTrips
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<IgnoredTrips>> PostIgnoredTrips(IgnoredTrips ignoredTrips)
        {
          if (_context.IgnoredTrips == null)
          {
              return Problem("Entity set 'DataContext.IgnoredTrips'  is null.");
          }
            _context.IgnoredTrips.Add(ignoredTrips);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIgnoredTrips", new { id = ignoredTrips.IgnoredTripsID }, ignoredTrips);
        }

        // DELETE: api/IgnoredTrips/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIgnoredTrips(int id)
        {
            if (_context.IgnoredTrips == null)
            {
                return NotFound();
            }
            var ignoredTrips = await _context.IgnoredTrips.FindAsync(id);
            if (ignoredTrips == null)
            {
                return NotFound();
            }

            _context.IgnoredTrips.Remove(ignoredTrips);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IgnoredTripsExists(int id)
        {
            return (_context.IgnoredTrips?.Any(e => e.IgnoredTripsID == id)).GetValueOrDefault();
        }
    }
}
