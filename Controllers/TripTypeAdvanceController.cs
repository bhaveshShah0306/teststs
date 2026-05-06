using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GoChauffeurWebApi.Data;
using GoChauffeurWebApi.Models;
using GoChauffeurWebApi.ViewModels;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripTypeAdvanceController : ControllerBase
    {
        private readonly DataContext _context;

        public TripTypeAdvanceController(DataContext context)
        {
            _context = context;
        }

        // GET: api/TripTypeAdvance
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdvanceAmountVM>>> GetAdvanceAmounts()
        {
            try
            {
                var advanceamounts = _context.AdvanceAmounts.ToList() ;
                var advvmlist = new List<AdvanceAmountVM>() ;
                if (advanceamounts.Count > 0)
                {
                    foreach(var advanceamount in advanceamounts)
                    {
                        var advvm = new AdvanceAmountVM() ;
                        advvm.AdvanceAmountId = advanceamount.AdvanceAmountId ;
                        advvm.Advance = advanceamount.Advance;
                        advvm.TripTypeId = advanceamount.TripTypeId;

                        var triptyname = _context.TripTypes.Find(advvm.TripTypeId);
                        if(triptyname != null)
                        {
                            advvm.TripTypeName = triptyname.TripName;
                        }

                        advvmlist.Add(advvm) ;
                    }
                    return Ok(advvmlist);
                }
                else
                {
                    return NoContent();
                }
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/TripTypeAdvance/5
        [HttpGet("{truptypeId}")]
        public async Task<ActionResult<AdvanceAmount>> GetAdvanceAmount(int truptypeId)
        {
          if (_context.AdvanceAmounts == null)
          {
              return NotFound();
          }
            var advanceAmount = await _context.AdvanceAmounts.Where(c=>c.TripTypeId == truptypeId).FirstOrDefaultAsync();

            if (advanceAmount == null)
            {
                return NotFound();
            }

            return advanceAmount;
        }

        // PUT: api/TripTypeAdvance/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<IActionResult> PutAdvanceAmount(int id, AdvanceAmount advanceAmount)
        {
            if (id != advanceAmount.AdvanceAmountId)
            {
                return BadRequest();
            }

            _context.Entry(advanceAmount).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdvanceAmountExists(id))
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

        // POST: api/TripTypeAdvance
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AdvanceAmount>> PostAdvanceAmount(AdvanceAmount advanceAmount)
        {
          if (_context.AdvanceAmounts == null)
          {
              return Problem("Entity set 'DataContext.AdvanceAmounts'  is null.");
          }
            _context.AdvanceAmounts.Add(advanceAmount);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAdvanceAmount", new { id = advanceAmount.AdvanceAmountId }, advanceAmount);
        }

        // DELETE: api/TripTypeAdvance/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdvanceAmount(int id)
        {
            if (_context.AdvanceAmounts == null)
            {
                return NotFound();
            }
            var advanceAmount = await _context.AdvanceAmounts.FindAsync(id);
            if (advanceAmount == null)
            {
                return NotFound();
            }

            _context.AdvanceAmounts.Remove(advanceAmount);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AdvanceAmountExists(int id)
        {
            return (_context.AdvanceAmounts?.Any(e => e.AdvanceAmountId == id)).GetValueOrDefault();
        }
    }
}
