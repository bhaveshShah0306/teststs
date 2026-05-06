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
    public class IgnoreMontlyTripsController : ControllerBase
    {
        private readonly DataContext _context;

        public IgnoreMontlyTripsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/IgnoreMontlyTrips
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IgnoreMontlyTrips>>> GetIgnoreMontlyTrips()
        {
          if (_context.IgnoreMontlyTrips == null)
          {
              return NotFound();
          }
            return await _context.IgnoreMontlyTrips.ToListAsync();
        }

        // GET: api/IgnoreMontlyTrips/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IgnoreMontlyTrips>> GetIgnoreMontlyTrips(int id)
        {
          if (_context.IgnoreMontlyTrips == null)
          {
              return NotFound();
          }
            var ignoreMontlyTrips = await _context.IgnoreMontlyTrips.FindAsync(id);

            if (ignoreMontlyTrips == null)
            {
                return NotFound();
            }

            return ignoreMontlyTrips;
        }

        // PUT: api/IgnoreMontlyTrips/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIgnoreMontlyTrips(int id, IgnoreMontlyTrips ignoreMontlyTrips)
        {
            if (id != ignoreMontlyTrips.IgnoreMontlyTripsId)
            {
                return BadRequest();
            }

            _context.Entry(ignoreMontlyTrips).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IgnoreMontlyTripsExists(id))
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

        // POST: api/IgnoreMontlyTrips
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<IgnoreMontlyTrips>> PostIgnoreMontlyTrips(IgnoreMontlyTrips ignoreMontlyTrips)
        {
          if (_context.IgnoreMontlyTrips == null)
          {
              return Problem("Entity set 'DataContext.IgnoreMontlyTrips'  is null.");
          }
            _context.IgnoreMontlyTrips.Add(ignoreMontlyTrips);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIgnoreMontlyTrips", new { id = ignoreMontlyTrips.IgnoreMontlyTripsId }, ignoreMontlyTrips);
        }

        // DELETE: api/IgnoreMontlyTrips/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIgnoreMontlyTrips(int id)
        {
            if (_context.IgnoreMontlyTrips == null)
            {
                return NotFound();
            }
            var ignoreMontlyTrips = await _context.IgnoreMontlyTrips.FindAsync(id);
            if (ignoreMontlyTrips == null)
            {
                return NotFound();
            }

            _context.IgnoreMontlyTrips.Remove(ignoreMontlyTrips);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IgnoreMontlyTripsExists(int id)
        {
            return (_context.IgnoreMontlyTrips?.Any(e => e.IgnoreMontlyTripsId == id)).GetValueOrDefault();
        }
    }
}
