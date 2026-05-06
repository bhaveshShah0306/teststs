using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GoChauffeurWebApi;
using GoChauffeurWebApi.Data;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscripationGstsController : ControllerBase
    {
        private readonly DataContext _context;

        public SubscripationGstsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/SubscripationGsts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubscripationGst>>> GetSubscripationGst()
        {
          if (_context.SubscripationGst == null)
          {
              return NotFound();
          }
            return await _context.SubscripationGst.ToListAsync();
        }

        // GET: api/SubscripationGsts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SubscripationGst>> GetSubscripationGst(int id)
        {
          if (_context.SubscripationGst == null)
          {
              return NotFound();
          }
            var subscripationGst = await _context.SubscripationGst.FindAsync(id);

            if (subscripationGst == null)
            {
                return NotFound();
            }

            return subscripationGst;
        }

        // PUT: api/SubscripationGsts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<IActionResult> PutSubscripationGst(int id, SubscripationGst subscripationGst)
        {
            if (id != subscripationGst.SubscripationGstId)
            {
                return BadRequest();
            }

            _context.Entry(subscripationGst).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubscripationGstExists(id))
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

        // POST: api/SubscripationGsts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SubscripationGst>> PostSubscripationGst(SubscripationGst subscripationGst)
        {
          if (_context.SubscripationGst == null)
          {
              return Problem("Entity set 'DataContext.SubscripationGst'  is null.");
          }
            _context.SubscripationGst.Add(subscripationGst);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSubscripationGst", new { id = subscripationGst.SubscripationGstId }, subscripationGst);
        }

        // DELETE: api/SubscripationGsts/5
        [HttpPost("{id}/delete")]
        public async Task<IActionResult> DeleteSubscripationGst(int id)
        {
            if (_context.SubscripationGst == null)
            {
                return NotFound();
            }
            var subscripationGst = await _context.SubscripationGst.FindAsync(id);
            if (subscripationGst == null)
            {
                return NotFound();
            }

            _context.SubscripationGst.Remove(subscripationGst);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SubscripationGstExists(int id)
        {
            return (_context.SubscripationGst?.Any(e => e.SubscripationGstId == id)).GetValueOrDefault();
        }
    }
}
