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
    public class IgnoreValletTripsController : ControllerBase
    {
        private readonly DataContext _context;

        public IgnoreValletTripsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/IgnoreValletTrips
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IgnoreValletTrips>>> GetIgnoreValletTrips()
        {
          if (_context.IgnoreValletTrips == null)
          {
              return NotFound();
          }
            return await _context.IgnoreValletTrips.ToListAsync();
        }

        // GET: api/IgnoreValletTrips/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IgnoreValletTrips>> GetIgnoreValletTrips(int id)
        {
          if (_context.IgnoreValletTrips == null)
          {
              return NotFound();
          }
            var ignoreValletTrips = await _context.IgnoreValletTrips.FindAsync(id);

            if (ignoreValletTrips == null)
            {
                return NotFound();
            }

            return ignoreValletTrips;
        }

        // PUT: api/IgnoreValletTrips/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIgnoreValletTrips(int id, IgnoreValletTrips ignoreValletTrips)
        {
            if (id != ignoreValletTrips.IgnoreValletTripsId)
            {
                return BadRequest();
            }

            _context.Entry(ignoreValletTrips).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IgnoreValletTripsExists(id))
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

        // POST: api/IgnoreValletTrips
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<IgnoreValletTrips>> PostIgnoreValletTrips(IgnoreValletTrips ignoreValletTrips)
        {
          if (_context.IgnoreValletTrips == null)
          {
              return Problem("Entity set 'DataContext.IgnoreValletTrips'  is null.");
          }
            _context.IgnoreValletTrips.Add(ignoreValletTrips);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIgnoreValletTrips", new { id = ignoreValletTrips.IgnoreValletTripsId }, ignoreValletTrips);
        }

        // DELETE: api/IgnoreValletTrips/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIgnoreValletTrips(int id)
        {
            if (_context.IgnoreValletTrips == null)
            {
                return NotFound();
            }
            var ignoreValletTrips = await _context.IgnoreValletTrips.FindAsync(id);
            if (ignoreValletTrips == null)
            {
                return NotFound();
            }

            _context.IgnoreValletTrips.Remove(ignoreValletTrips);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IgnoreValletTripsExists(int id)
        {
            return (_context.IgnoreValletTrips?.Any(e => e.IgnoreValletTripsId == id)).GetValueOrDefault();
        }
    }
}
