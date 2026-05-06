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
    public class TripTaxesController : ControllerBase
    {
        private readonly DataContext _context;

        public TripTaxesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/TripTaxes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TripTax>>> GetTripTaxes()
        {
          if (_context.TripTaxes == null)
          {
              return NotFound();
          }
            return await _context.TripTaxes.ToListAsync();
        }

        // GET: api/TripTaxes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TripTax>> GetTripTax(int id)
        {
          if (_context.TripTaxes == null)
          {
              return NotFound();
          }
            var tripTax = await _context.TripTaxes.FindAsync(id);

            if (tripTax == null)
            {
                return NotFound();
            }

            return tripTax;
        }

        // PUT: api/TripTaxes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTripTax(int id, TripTax tripTax)
        {
            if (id != tripTax.TripTaxId)
            {
                return BadRequest();
            }

            _context.Entry(tripTax).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TripTaxExists(id))
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

        // POST: api/TripTaxes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TripTax>> PostTripTax(TripTax tripTax)
        {
          if (_context.TripTaxes == null)
          {
              return Problem("Entity set 'DataContext.TripTaxes'  is null.");
          }
            _context.TripTaxes.Add(tripTax);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTripTax", new { id = tripTax.TripTaxId }, tripTax);
        }

        // DELETE: api/TripTaxes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTripTax(int id)
        {
            if (_context.TripTaxes == null)
            {
                return NotFound();
            }
            var tripTax = await _context.TripTaxes.FindAsync(id);
            if (tripTax == null)
            {
                return NotFound();
            }

            _context.TripTaxes.Remove(tripTax);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TripTaxExists(int id)
        {
            return (_context.TripTaxes?.Any(e => e.TripTaxId == id)).GetValueOrDefault();
        }
    }
}
