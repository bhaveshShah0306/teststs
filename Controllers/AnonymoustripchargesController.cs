
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
    public class AnonymoustripchargesController : ControllerBase
    {
        private readonly DataContext _context;

        public AnonymoustripchargesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Anonymoustripcharges
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnonymoustripchargesVM>>> GetAnonymoustripcharges()
        {
            try
            {
                var anonymuscharges = await _context.Anonymoustripcharges.ToListAsync();
                var anonymousVM = new List<AnonymoustripchargesVM>();

                if (anonymuscharges.Count > 0)
                {
                    foreach (var charge in anonymuscharges)
                    {
                        var tripName = await Gettriptypename(charge.TriptypeId);
                        var aVM = new AnonymoustripchargesVM
                        {
                            AnonymoustripchargesId = charge.AnonymoustripchargesId,
                            TriptypeId = charge.TriptypeId,
                            TriptypName = tripName,
                            Amount = charge.Amount
                        };
                        anonymousVM.Add(aVM);
                    }
                }

                return Ok(anonymousVM);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task<string> Gettriptypename(int? id)
        {
            try
            {
                var triptypedata = await _context.TripTypes.FindAsync(id);
                if (triptypedata != null)
                {
                    return triptypedata.TripName;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return ex.Message; // Handle the exception appropriately
            }
        }


        // GET: api/Anonymoustripcharges/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Anonymoustripcharges>> GetAnonymoustripcharges(int id)
        {
          if (_context.Anonymoustripcharges == null)
          {
              return NotFound();
          }
            var anonymoustripcharges = await _context.Anonymoustripcharges.FindAsync(id);

            if (anonymoustripcharges == null)
            {
                return NotFound();
            }

            return anonymoustripcharges;
        }

        // PUT: api/Anonymoustripcharges/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<IActionResult> PutAnonymoustripcharges(int id, Anonymoustripcharges anonymoustripcharges)
        {
            if (id != anonymoustripcharges.AnonymoustripchargesId)
            {
                return BadRequest();
            }

            _context.Entry(anonymoustripcharges).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnonymoustripchargesExists(id))
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

        // POST: api/Anonymoustripcharges
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Anonymoustripcharges>> PostAnonymoustripcharges(Anonymoustripcharges anonymoustripcharges)
        {
          if (_context.Anonymoustripcharges == null)
          {
              return Problem("Entity set 'DataContext.Anonymoustripcharges'  is null.");
          }
            _context.Anonymoustripcharges.Add(anonymoustripcharges);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAnonymoustripcharges", new { id = anonymoustripcharges.AnonymoustripchargesId }, anonymoustripcharges);
        }

        // DELETE: api/Anonymoustripcharges/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnonymoustripcharges(int id)
        {
            if (_context.Anonymoustripcharges == null)
            {
                return NotFound();
            }
            var anonymoustripcharges = await _context.Anonymoustripcharges.FindAsync(id);
            if (anonymoustripcharges == null)
            {
                return NotFound();
            }

            _context.Anonymoustripcharges.Remove(anonymoustripcharges);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AnonymoustripchargesExists(int id)
        {
            return (_context.Anonymoustripcharges?.Any(e => e.AnonymoustripchargesId == id)).GetValueOrDefault();
        }
    }
}
