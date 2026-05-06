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
    public class RequiredValetstaffsController : ControllerBase
    {
        private readonly DataContext _context;

        public RequiredValetstaffsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/RequiredValetstaffs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequiredValetstaffs>>> GetRequiredValetstaffs()
        {
          if (_context.RequiredValetstaffs == null)
          {
              return NotFound();
          }
            return await _context.RequiredValetstaffs.ToListAsync();
        }

        // GET: api/RequiredValetstaffs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RequiredValetstaffs>> GetRequiredValetstaffs(int id)
        {
          if (_context.RequiredValetstaffs == null)
          {
              return NotFound();
          }
            var requiredValetstaffs = await _context.RequiredValetstaffs.FindAsync(id);

            if (requiredValetstaffs == null)
            {
                return NotFound();
            }

            return requiredValetstaffs;
        }

        // PUT: api/RequiredValetstaffs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequiredValetstaffs(int id, RequiredValetstaffs requiredValetstaffs)
        {
            if (id != requiredValetstaffs.RequiredValetstaffsId)
            {
                return BadRequest();
            }

            _context.Entry(requiredValetstaffs).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequiredValetstaffsExists(id))
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

        // POST: api/RequiredValetstaffs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RequiredValetstaffs>> PostRequiredValetstaffs(RequiredValetstaffs requiredValetstaffs)
        {
          if (_context.RequiredValetstaffs == null)
          {
              return Problem("Entity set 'DataContext.RequiredValetstaffs'  is null.");
          }
            _context.RequiredValetstaffs.Add(requiredValetstaffs);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequiredValetstaffs", new { id = requiredValetstaffs.RequiredValetstaffsId }, requiredValetstaffs);
        }

        // DELETE: api/RequiredValetstaffs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequiredValetstaffs(int id)
        {
            if (_context.RequiredValetstaffs == null)
            {
                return NotFound();
            }
            var requiredValetstaffs = await _context.RequiredValetstaffs.FindAsync(id);
            if (requiredValetstaffs == null)
            {
                return NotFound();
            }

            _context.RequiredValetstaffs.Remove(requiredValetstaffs);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RequiredValetstaffsExists(int id)
        {
            return (_context.RequiredValetstaffs?.Any(e => e.RequiredValetstaffsId == id)).GetValueOrDefault();
        }
    }
}
