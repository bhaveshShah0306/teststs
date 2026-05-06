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
    public class AerialDistancePricesController : ControllerBase
    {
        private readonly DataContext _context;

        public AerialDistancePricesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/AerialDistancePrices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AerialDistancePrice>>> GetAerialDistancePrices()
        {
          if (_context.AerialDistancePrices == null)
          {
              return NotFound();
          }
            return await _context.AerialDistancePrices.ToListAsync();
        }

        // GET: api/AerialDistancePrices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AerialDistancePrice>> GetAerialDistancePrice(int id)
        {
          if (_context.AerialDistancePrices == null)
          {
              return NotFound();
          }
            var aerialDistancePrice = await _context.AerialDistancePrices.FindAsync(id);

            if (aerialDistancePrice == null)
            {
                return NotFound();
            }

            return aerialDistancePrice;
        }

        // PUT: api/AerialDistancePrices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<IActionResult> PutAerialDistancePrice(int id, AerialDistancePrice aerialDistancePrice)
        {
            if (id != aerialDistancePrice.AerialDistancePriceId)
            {
                return BadRequest();
            }

            _context.Entry(aerialDistancePrice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AerialDistancePriceExists(id))
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

        // POST: api/AerialDistancePrices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AerialDistancePrice>> PostAerialDistancePrice(AerialDistancePrice aerialDistancePrice)
        {
          if (_context.AerialDistancePrices == null)
          {
              return Problem("Entity set 'DataContext.AerialDistancePrices'  is null.");
          }
            _context.AerialDistancePrices.Add(aerialDistancePrice);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAerialDistancePrice", new { id = aerialDistancePrice.AerialDistancePriceId }, aerialDistancePrice);
        }

        // DELETE: api/AerialDistancePrices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAerialDistancePrice(int id)
        {
            if (_context.AerialDistancePrices == null)
            {
                return NotFound();
            }
            var aerialDistancePrice = await _context.AerialDistancePrices.FindAsync(id);
            if (aerialDistancePrice == null)
            {
                return NotFound();
            }

            _context.AerialDistancePrices.Remove(aerialDistancePrice);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AerialDistancePriceExists(int id)
        {
            return (_context.AerialDistancePrices?.Any(e => e.AerialDistancePriceId == id)).GetValueOrDefault();
        }
    }
}
