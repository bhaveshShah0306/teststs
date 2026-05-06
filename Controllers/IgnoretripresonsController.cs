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
    public class IgnoretripresonsController : ControllerBase
    {
        private readonly DataContext _context;

        public IgnoretripresonsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Ignoretripresons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ignoretripresons>>> Getignoretripresons()
        {
          if (_context.ignoretripresons == null)
          {
              return NotFound();
          }
            return await _context.ignoretripresons.ToListAsync();
        }

        // GET: api/Ignoretripresons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ignoretripresons>> GetIgnoretripresons(int id)
        {
          if (_context.ignoretripresons == null)
          {
              return NotFound();
          }
            var ignoretripresons = await _context.ignoretripresons.FindAsync(id);

            if (ignoretripresons == null)
            {
                return NotFound();
            }

            return ignoretripresons;
        }

        // PUT: api/Ignoretripresons/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIgnoretripresons(int id, Ignoretripresons ignoretripresons)
        {
            if (id != ignoretripresons.IgnoretripresonsId)
            {
                return BadRequest();
            }

            _context.Entry(ignoretripresons).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IgnoretripresonsExists(id))
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

        // POST: api/Ignoretripresons
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Ignoretripresons>> PostIgnoretripresons(Ignoretripresons ignoretripresons)
        {
          if (_context.ignoretripresons == null)
          {
              return Problem("Entity set 'DataContext.ignoretripresons'  is null.");
          }
            _context.ignoretripresons.Add(ignoretripresons);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIgnoretripresons", new { id = ignoretripresons.IgnoretripresonsId }, ignoretripresons);
        }

        // DELETE: api/Ignoretripresons/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIgnoretripresons(int id)
        {
            if (_context.ignoretripresons == null)
            {
                return NotFound();
            }
            var ignoretripresons = await _context.ignoretripresons.FindAsync(id);
            if (ignoretripresons == null)
            {
                return NotFound();
            }

            _context.ignoretripresons.Remove(ignoretripresons);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IgnoretripresonsExists(int id)
        {
            return (_context.ignoretripresons?.Any(e => e.IgnoretripresonsId == id)).GetValueOrDefault();
        }
    }
}
