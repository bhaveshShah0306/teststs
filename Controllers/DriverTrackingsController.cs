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
    public class DriverTrackingsController : ControllerBase
    {
        private readonly DataContext _context;

        public DriverTrackingsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/DriverTrackings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DriverTracking>>> GetDriverTrackings()
        {
          if (_context.DriverTrackings == null)
          {
              return NotFound();
          }
            return await _context.DriverTrackings.ToListAsync();
        }

        // GET: api/DriverTrackings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DriverTracking>> GetDriverTracking(int id)
        {
          if (_context.DriverTrackings == null)
          {
              return NotFound();
          }
            var driverTracking = await _context.DriverTrackings.FindAsync(id);

            if (driverTracking == null)
            {
                return NotFound();
            }

            return driverTracking;
        }

        // PUT: api/DriverTrackings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<IActionResult> PutDriverTracking(int id, DriverTracking driverTracking)
        {
            if (id != driverTracking.DriverTrackingId)
            {
                return BadRequest();
            }

            _context.Entry(driverTracking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriverTrackingExists(id))
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

        // POST: api/DriverTrackings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DriverTracking>> PostDriverTracking(DriverTracking driverTracking)
        {
          if (_context.DriverTrackings == null)
          {
              return Problem("Entity set 'DataContext.DriverTrackings'  is null.");
          }
            _context.DriverTrackings.Add(driverTracking);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDriverTracking", new { id = driverTracking.DriverTrackingId }, driverTracking);
        }

        // DELETE: api/DriverTrackings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriverTracking(int id)
        {
            if (_context.DriverTrackings == null)
            {
                return NotFound();
            }
            var driverTracking = await _context.DriverTrackings.FindAsync(id);
            if (driverTracking == null)
            {
                return NotFound();
            }

            _context.DriverTrackings.Remove(driverTracking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DriverTrackingExists(int id)
        {
            return (_context.DriverTrackings?.Any(e => e.DriverTrackingId == id)).GetValueOrDefault();
        }
    }
}
