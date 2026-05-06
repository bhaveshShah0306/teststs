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
    public class FlexiDatesListsController : ControllerBase
    {
        private readonly DataContext _context;

        public FlexiDatesListsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/FlexiDatesLists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FlexiDatesList>>> GetFlexiDatesLists()
        {
          if (_context.FlexiDatesLists == null)
          {
              return NotFound();
          }
            return await _context.FlexiDatesLists.ToListAsync();
        }

        // GET: api/FlexiDatesLists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<FlexiDatesList>>> GetFlexiDatesList(int id)
        {
            if (_context.FlexiDatesLists == null)
            {
                return NotFound();
            }

            var flexiDatesLists = await _context.FlexiDatesLists.Where(f => f.FlexiId == id).OrderBy(c=>c.Date).ToListAsync();
           

            if (flexiDatesLists == null || !flexiDatesLists.Any())
            {
                return NotFound();
            }

            return Ok(flexiDatesLists);
        }
        [HttpGet("ByFlexiDatesListId/{flexiDatesListId}")]
        public async Task<ActionResult<FlexiDatesList>> GetFlexiDatesListById(int flexiDatesListId)
        {
            if (_context.FlexiDatesLists == null)
            {
                return NotFound();
            }

            var flexiDatesList = await _context.FlexiDatesLists
                                               .FirstOrDefaultAsync(f => f.FlexiDatesListId == flexiDatesListId);

            if (flexiDatesList == null)
            {
                return NotFound();
            }

            return Ok(flexiDatesList);
        }



        // PUT: api/FlexiDatesLists/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFlexiDatesList(int id, FlexiDatesList flexiDatesList)
        {
            if (id != flexiDatesList.FlexiDatesListId)
            {
                return BadRequest();
            }

            _context.Entry(flexiDatesList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlexiDatesListExists(id))
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

        // POST: api/FlexiDatesLists
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FlexiDatesList>> PostFlexiDatesList(FlexiDatesList flexiDatesList)
        {
          if (_context.FlexiDatesLists == null)
          {
              return Problem("Entity set 'DataContext.FlexiDatesLists'  is null.");
          }
            _context.FlexiDatesLists.Add(flexiDatesList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFlexiDatesList", new { id = flexiDatesList.FlexiDatesListId }, flexiDatesList);
        }

        // DELETE: api/FlexiDatesLists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlexiDatesList(int id)
        {
            if (_context.FlexiDatesLists == null)
            {
                return NotFound();
            }
            var flexiDatesList = await _context.FlexiDatesLists.FindAsync(id);
            if (flexiDatesList == null)
            {
                return NotFound();
            }

            _context.FlexiDatesLists.Remove(flexiDatesList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FlexiDatesListExists(int id)
        {
            return (_context.FlexiDatesLists?.Any(e => e.FlexiDatesListId == id)).GetValueOrDefault();
        }
    }
}
