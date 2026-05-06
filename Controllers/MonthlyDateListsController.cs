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
    public class MonthlyDateListsController : ControllerBase
    {
        private readonly DataContext _context;

        public MonthlyDateListsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/MonthlyDateLists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MonthlyDateList>>> GetMonthlyDateLists()
        {
          if (_context.MonthlyDateLists == null)
          {
              return NotFound();
          }
            return await _context.MonthlyDateLists.ToListAsync();
        }

        // GET: api/MonthlyDateLists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MonthlyDateList>> GetMonthlyDateList(int id)
        {
          if (_context.MonthlyDateLists == null)
          {
              return NotFound();
          }
            var monthlyDateList = await _context.MonthlyDateLists.FindAsync(id);

            if (monthlyDateList == null)
            {
                return NotFound();
            }

            return monthlyDateList;
        }
        // GET: api/MonthlyDateLists/MonthlyId/5
        [HttpGet("MonthlyId/{monthlyId}")]
        public async Task<ActionResult<MonthlyDateList>> GetMonthlyDateListByMonthlyId(int monthlyId)
        {
            if (_context.MonthlyDateLists == null)
            {
                return NotFound();
            }

            var monthlyDateList = await _context.MonthlyDateLists
                                                .Where(mdl => mdl.MonthlyId == monthlyId)
                                                .FirstOrDefaultAsync();

            if (monthlyDateList == null)
            {
                return NotFound();
            }

            return monthlyDateList;
        }



        // PUT: api/MonthlyDateLists/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMonthlyDateList(int id, MonthlyDateList monthlyDateList)
        {
            if (id != monthlyDateList.MonthlyDateListId)
            {
                return BadRequest();
            }

            _context.Entry(monthlyDateList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MonthlyDateListExists(id))
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

        // POST: api/MonthlyDateLists
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MonthlyDateList>> PostMonthlyDateList(MonthlyDateList monthlyDateList)
        {
          if (_context.MonthlyDateLists == null)
          {
              return Problem("Entity set 'DataContext.MonthlyDateLists'  is null.");
          }
            _context.MonthlyDateLists.Add(monthlyDateList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMonthlyDateList", new { id = monthlyDateList.MonthlyDateListId }, monthlyDateList);
        }

        // DELETE: api/MonthlyDateLists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMonthlyDateList(int id)
        {
            if (_context.MonthlyDateLists == null)
            {
                return NotFound();
            }
            var monthlyDateList = await _context.MonthlyDateLists.FindAsync(id);
            if (monthlyDateList == null)
            {
                return NotFound();
            }

            _context.MonthlyDateLists.Remove(monthlyDateList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MonthlyDateListExists(int id)
        {
            return (_context.MonthlyDateLists?.Any(e => e.MonthlyDateListId == id)).GetValueOrDefault();
        }
    }
}
