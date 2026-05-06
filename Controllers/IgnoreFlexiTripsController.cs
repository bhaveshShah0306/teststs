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
    public class IgnoreFlexiTripsController : ControllerBase
    {
        private readonly DataContext _context;

        public IgnoreFlexiTripsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/IgnoreFlexiTrips
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IgnoreFlexiTrips>>> GetIgnoreFlexiTrips()
        {
          if (_context.IgnoreFlexiTrips == null)
          {
              return NotFound();
          }
            return await _context.IgnoreFlexiTrips.ToListAsync();
        }

        // GET: api/IgnoreFlexiTrips/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IgnoreFlexiTrips>> GetIgnoreFlexiTrips(int id)
        {
          if (_context.IgnoreFlexiTrips == null)
          {
              return NotFound();
          }
            var ignoreFlexiTrips = await _context.IgnoreFlexiTrips.FindAsync(id);

            if (ignoreFlexiTrips == null)
            {
                return NotFound();
            }

            return ignoreFlexiTrips;
        }

        // PUT: api/IgnoreFlexiTrips/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIgnoreFlexiTrips(int id, IgnoreFlexiTrips ignoreFlexiTrips)
        {
            if (id != ignoreFlexiTrips.IgnoreFlexiTripsId)
            {
                return BadRequest();
            }

            _context.Entry(ignoreFlexiTrips).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IgnoreFlexiTripsExists(id))
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

        // POST: api/IgnoreFlexiTrips
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<IgnoreFlexiTrips>> PostIgnoreFlexiTrips(IgnoreFlexiTrips ignoreFlexiTrips)
        {
          if (_context.IgnoreFlexiTrips == null)
          {
              return Problem("Entity set 'DataContext.IgnoreFlexiTrips'  is null.");
          }
            _context.IgnoreFlexiTrips.Add(ignoreFlexiTrips);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIgnoreFlexiTrips", new { id = ignoreFlexiTrips.IgnoreFlexiTripsId }, ignoreFlexiTrips);
        }

        // DELETE: api/IgnoreFlexiTrips/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIgnoreFlexiTrips(int id)
        {
            if (_context.IgnoreFlexiTrips == null)
            {
                return NotFound();
            }
            var ignoreFlexiTrips = await _context.IgnoreFlexiTrips.FindAsync(id);
            if (ignoreFlexiTrips == null)
            {
                return NotFound();
            }

            _context.IgnoreFlexiTrips.Remove(ignoreFlexiTrips);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IgnoreFlexiTripsExists(int id)
        {
            return (_context.IgnoreFlexiTrips?.Any(e => e.IgnoreFlexiTripsId == id)).GetValueOrDefault();
        }
    }
}
