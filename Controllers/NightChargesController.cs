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
    public class NightChargesController : ControllerBase
    {
        private readonly DataContext _context;

        public NightChargesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/NightCharges
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NightChargesVM>>> GetNightCharges()
        {
            try
            {
                var nightvallist = _context.NightCharges.ToList();
                var nightVMList = new List<NightChargesVM>();
                if (nightvallist.Count > 0)
                {
                    foreach(var n in nightvallist)
                    {
                        var nvm = new NightChargesVM();
                        nvm.NightChargesId = n.NightChargesId;

                        DateTime? startTime = n.StartTime;
                        var starttime = String.Format("{0:hh:mm tt}", startTime);
                        nvm.StartTime = starttime;

                        DateTime? endTime = n.EndTime;
                        var endtime = string.Format("{0:hh:mm tt}", endTime);
                        nvm.EndTime = endtime;
                        nvm.Charges = n.Charges;
                        nightVMList.Add(nvm);
                    }
                    return Ok(nightVMList);
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

        // GET: api/NightCharges/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NightCharges>> GetNightCharges(int id)
        {
          if (_context.NightCharges == null)
          {
              return NotFound();
          }
            var nightCharges = await _context.NightCharges.FindAsync(id);

            if (nightCharges == null)
            {
                return NotFound();
            }

            return nightCharges;
        }

        // PUT: api/NightCharges/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNightCharges(int id, NightCharges nightCharges)
        {
            if (id != nightCharges.NightChargesId)
            {
                return BadRequest();
            }

            _context.Entry(nightCharges).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NightChargesExists(id))
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

        // POST: api/NightCharges
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<NightCharges>> PostNightCharges(NightCharges nightCharges)
        {
          if (_context.NightCharges == null)
          {
              return Problem("Entity set 'DataContext.NightCharges'  is null.");
          }
            _context.NightCharges.Add(nightCharges);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNightCharges", new { id = nightCharges.NightChargesId }, nightCharges);
        }

        // DELETE: api/NightCharges/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNightCharges(int id)
        {
            if (_context.NightCharges == null)
            {
                return NotFound();
            }
            var nightCharges = await _context.NightCharges.FindAsync(id);
            if (nightCharges == null)
            {
                return NotFound();
            }

            _context.NightCharges.Remove(nightCharges);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NightChargesExists(int id)
        {
            return (_context.NightCharges?.Any(e => e.NightChargesId == id)).GetValueOrDefault();
        }
    }
}
