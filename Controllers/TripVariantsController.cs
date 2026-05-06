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
    public class TripVariantsController : ControllerBase
    {
        private readonly DataContext _context;

        public TripVariantsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/TripVariants
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TripVariant>>> GetTripVariants()
        {
          if (_context.TripVariants == null)
          {
              return NotFound();
          }
            return await _context.TripVariants.ToListAsync();
        }
        [HttpGet("{tripTypeid}/flag")]
        public async Task<ActionResult<IEnumerable<TripVariant>>> GetTripVariants(int tripTypeid)
        {
          if (_context.TripVariants == null)
          {
              return NotFound();
          }
            return await _context.TripVariants.Where(c=>c.TriptypeId == tripTypeid ).ToListAsync();
        }

        // GET: api/TripVariants/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TripVariant>> GetTripVariant(int id)
        {
          if (_context.TripVariants == null)
          {
              return NotFound();
          }
            var tripVariant = await _context.TripVariants.FindAsync(id);

            if (tripVariant == null)
            {
                return NotFound();
            }

            return tripVariant;
        }

        // PUT: api/TripVariants/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<IActionResult> PutTripVariant(int id, TripVariant tripVariant)
        {
            if (id != tripVariant.TripVariantId)
            {
                return BadRequest();
            }

            _context.Entry(tripVariant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TripVariantExists(id))
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

        // POST: api/TripVariants
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TripVariant>> PostTripVariant(TripVariant tripVariant)
        {
          if (_context.TripVariants == null)
          {
              return Problem("Entity set 'DataContext.TripVariants'  is null.");
          }
            _context.TripVariants.Add(tripVariant);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTripVariant", new { id = tripVariant.TripVariantId }, tripVariant);
        }

        // DELETE: api/TripVariants/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTripVariant(int id)
        {
            if (_context.TripVariants == null)
            {
                return NotFound();
            }
            var tripVariant = await _context.TripVariants.FindAsync(id);
            if (tripVariant == null)
            {
                return NotFound();
            }

            _context.TripVariants.Remove(tripVariant);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TripVariantExists(int id)
        {
            return (_context.TripVariants?.Any(e => e.TripVariantId == id)).GetValueOrDefault();
        }
    }
}
