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
    public class ReferEarningsController : ControllerBase
    {
        private readonly DataContext _context;

        public ReferEarningsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/ReferEarnings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReferEarning>>> GetReferEarnings()
        {
            if (_context.ReferEarnings == null)
            {
                return NotFound();
            }
            return await _context.ReferEarnings.ToListAsync();
        }

        // GET: api/ReferEarnings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReferEarning>> GetReferEarning(int id)
        {
            if (_context.ReferEarnings == null)
            {
                return NotFound();
            }
            var referEarning = await _context.ReferEarnings.FindAsync(id);

            if (referEarning == null)
            {
                return NotFound();
            }

            return referEarning;
        }

        // PUT: api/ReferEarnings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<IActionResult> PutReferEarning(int id, ReferEarning referEarning)
        {
            if (id != referEarning.ReferEarningId)
            {
                return BadRequest();
            }

            _context.Entry(referEarning).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReferEarningExists(id))
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

        // POST: api/ReferEarnings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ReferEarning>> PostReferEarning(ReferEarning referEarning)
        {
            if (_context.ReferEarnings == null)
            {
                return Problem("Entity set 'DataContext.ReferEarnings'  is null.");
            }
            _context.ReferEarnings.Add(referEarning);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReferEarning", new { id = referEarning.ReferEarningId }, referEarning);
        }

        // DELETE: api/ReferEarnings/5
        [HttpPost("{id}/delete")]
        public async Task<IActionResult> DeleteReferEarning(int id)
        {
            if (_context.ReferEarnings == null)
            {
                return NotFound();
            }
            var referEarning = await _context.ReferEarnings.FindAsync(id);
            if (referEarning == null)
            {
                return NotFound();
            }

            _context.ReferEarnings.Remove(referEarning);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReferEarningExists(int id)
        {
            return (_context.ReferEarnings?.Any(e => e.ReferEarningId == id)).GetValueOrDefault();
        }
    }
}
