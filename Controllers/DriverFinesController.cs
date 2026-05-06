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
    public class DriverFinesController : ControllerBase
    {
        private readonly DataContext _context;

        public DriverFinesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/DriverFines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DriverFines>>> GetDriverFines()
        {
          if (_context.DriverFines == null)
          {
              return NotFound();
          }
            return await _context.DriverFines.ToListAsync();
        }

        // GET: api/DriverFines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DriverFines>> GetDriverFines(int id)
        {
          if (_context.DriverFines == null)
          {
              return NotFound();
          }
            var driverFines = await _context.DriverFines.FindAsync(id);

            if (driverFines == null)
            {
                return NotFound();
            }

            return driverFines;
        }

        // PUT: api/DriverFines/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<IActionResult> PutDriverFines(int id, DriverFines driverFines)
        {
            if (id != driverFines.DriverFinesId)
            {
                return BadRequest();
            }

            _context.Entry(driverFines).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriverFinesExists(id))
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

        // POST: api/DriverFines
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DriverFines>> PostDriverFines(DriverFines driverFines)
        {
          if (_context.DriverFines == null)
          {
              return Problem("Entity set 'DataContext.DriverFines'  is null.");
          }
            _context.DriverFines.Add(driverFines);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDriverFines", new { id = driverFines.DriverFinesId }, driverFines);
        }

        // DELETE: api/DriverFines/5
        [HttpPost("{id}/delete")]
        public async Task<IActionResult> DeleteDriverFines(int id)
        {
            if (_context.DriverFines == null)
            {
                return NotFound();
            }
            var driverFines = await _context.DriverFines.FindAsync(id);
            if (driverFines == null)
            {
                return NotFound();
            }

            _context.DriverFines.Remove(driverFines);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DriverFinesExists(int id)
        {
            return (_context.DriverFines?.Any(e => e.DriverFinesId == id)).GetValueOrDefault();
        }
    }
}
