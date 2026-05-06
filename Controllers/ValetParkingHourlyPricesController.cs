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
    public class ValetParkingHourlyPricesController : ControllerBase
    {
        private readonly DataContext _context;

        public ValetParkingHourlyPricesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/ValetParkingHourlyPrices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ValetParkingHourlyPrices>>> GetValetParkingHourlyPrices()
        {
          if (_context.ValetParkingHourlyPrices == null)
          {
              return NotFound();
          }
            return await _context.ValetParkingHourlyPrices.ToListAsync();
        }

        // GET: api/ValetParkingHourlyPrices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ValetParkingHourlyPrices>> GetValetParkingHourlyPrices(int id)
        {
          if (_context.ValetParkingHourlyPrices == null)
          {
              return NotFound();
          }
            var valetParkingHourlyPrices = await _context.ValetParkingHourlyPrices.FindAsync(id);

            if (valetParkingHourlyPrices == null)
            {
                return NotFound();
            }

            return valetParkingHourlyPrices;
        }

        // PUT: api/ValetParkingHourlyPrices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutValetParkingHourlyPrices(int id, ValetParkingHourlyPrices valetParkingHourlyPrices)
        {
            if (id != valetParkingHourlyPrices.ValetParkingHourlyPricesId)
            {
                return BadRequest();
            }

            _context.Entry(valetParkingHourlyPrices).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ValetParkingHourlyPricesExists(id))
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

        // POST: api/ValetParkingHourlyPrices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ValetParkingHourlyPrices>> PostValetParkingHourlyPrices(ValetParkingHourlyPrices valetParkingHourlyPrices)
        {
          if (_context.ValetParkingHourlyPrices == null)
          {
              return Problem("Entity set 'DataContext.ValetParkingHourlyPrices'  is null.");
          }
            _context.ValetParkingHourlyPrices.Add(valetParkingHourlyPrices);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetValetParkingHourlyPrices", new { id = valetParkingHourlyPrices.ValetParkingHourlyPricesId }, valetParkingHourlyPrices);
        }

        // DELETE: api/ValetParkingHourlyPrices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteValetParkingHourlyPrices(int id)
        {
            if (_context.ValetParkingHourlyPrices == null)
            {
                return NotFound();
            }
            var valetParkingHourlyPrices = await _context.ValetParkingHourlyPrices.FindAsync(id);
            if (valetParkingHourlyPrices == null)
            {
                return NotFound();
            }

            _context.ValetParkingHourlyPrices.Remove(valetParkingHourlyPrices);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ValetParkingHourlyPricesExists(int id)
        {
            return (_context.ValetParkingHourlyPrices?.Any(e => e.ValetParkingHourlyPricesId == id)).GetValueOrDefault();
        }
    }
}
