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
    public class DriversurroundingtripsController : ControllerBase
    {
        private readonly DataContext _context;

        public DriversurroundingtripsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Driversurroundingtrips
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Driversurroundingtrips>>> GetDriversurroundingtrips()
        {
          if (_context.Driversurroundingtrips == null)
          {
              return NotFound();
          }
            return await _context.Driversurroundingtrips.ToListAsync();
        }

        // GET: api/Driversurroundingtrips/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Driversurroundingtrips>> GetDriversurroundingtrips(int id)
        {
          if (_context.Driversurroundingtrips == null)
          {
              return NotFound();
          }
            var driversurroundingtrips = await _context.Driversurroundingtrips.FindAsync(id);

            if (driversurroundingtrips == null)
            {
                return NotFound();
            }

            return driversurroundingtrips;
        }

        // PUT: api/Driversurroundingtrips/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDriversurroundingtrips(int id, Driversurroundingtrips driversurroundingtrips)
        {
            if (id != driversurroundingtrips.DriversurroundingtripsId)
            {
                return BadRequest();
            }

            _context.Entry(driversurroundingtrips).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriversurroundingtripsExists(id))
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

        // POST: api/Driversurroundingtrips
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Driversurroundingtrips>> PostDriversurroundingtrips(Driversurroundingtrips driversurroundingtrips)
        {
          if (_context.Driversurroundingtrips == null)
          {
              return Problem("Entity set 'DataContext.Driversurroundingtrips'  is null.");
          }
            _context.Driversurroundingtrips.Add(driversurroundingtrips);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDriversurroundingtrips", new { id = driversurroundingtrips.DriversurroundingtripsId }, driversurroundingtrips);
        }

        // DELETE: api/Driversurroundingtrips/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriversurroundingtrips(int id)
        {
            if (_context.Driversurroundingtrips == null)
            {
                return NotFound();
            }
            var driversurroundingtrips = await _context.Driversurroundingtrips.FindAsync(id);
            if (driversurroundingtrips == null)
            {
                return NotFound();
            }

            _context.Driversurroundingtrips.Remove(driversurroundingtrips);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DriversurroundingtripsExists(int id)
        {
            return (_context.Driversurroundingtrips?.Any(e => e.DriversurroundingtripsId == id)).GetValueOrDefault();
        }
    }
}
