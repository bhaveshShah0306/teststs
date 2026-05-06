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
    public class TripStatusController : ControllerBase
    {
        private readonly DataContext _context;

        public TripStatusController(DataContext context)
        {
            _context = context;
        }

        // GET: api/TripStatus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TripStatus>>> GetTripStatuses()
        {
          if (_context.TripStatuses == null)
          {
              return NotFound();
          }
            return await _context.TripStatuses.ToListAsync();
        }

        // GET: api/TripStatus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TripStatus>> GetTripStatus(int id)
        {
          if (_context.TripStatuses == null)
          {
              return NotFound();
          }
            var tripStatus = await _context.TripStatuses.FindAsync(id);

            if (tripStatus == null)
            {
                return NotFound();
            }

            return tripStatus;
        }

        // PUT: api/TripStatus/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<IActionResult> PutTripStatus(int id, TripStatus tripStatus)
        {
            if (id != tripStatus.TripStatusId)
            {
                return BadRequest();
            }

            _context.Entry(tripStatus).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TripStatusExists(id))
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

        // POST: api/TripStatus
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TripStatus>> PostTripStatus(TripStatus tripStatus)
        {
          if (_context.TripStatuses == null)
          {
              return Problem("Entity set 'DataContext.TripStatuses'  is null.");
          }
            _context.TripStatuses.Add(tripStatus);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTripStatus", new { id = tripStatus.TripStatusId }, tripStatus);
        }

        // DELETE: api/TripStatus/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTripStatus(int id)
        {
            if (_context.TripStatuses == null)
            {
                return NotFound();
            }
            var tripStatus = await _context.TripStatuses.FindAsync(id);
            if (tripStatus == null)
            {
                return NotFound();
            }

            _context.TripStatuses.Remove(tripStatus);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TripStatusExists(int id)
        {
            return (_context.TripStatuses?.Any(e => e.TripStatusId == id)).GetValueOrDefault();
        }
    }
}
