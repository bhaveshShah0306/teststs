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
    public class CuponsController : ControllerBase
    {
        private readonly DataContext _context;

        public CuponsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Cupons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cupons>>> GetCupons()
        {
          if (_context.Cupons == null)
          {
              return NotFound();
          }
            return await _context.Cupons.ToListAsync();
        }
        
        // GET: api/Cupons
        [HttpGet("{userid}/user")]
        public async Task<ActionResult<IEnumerable<Cupons>>> GetCuponsbyuserid(int userid)
        {
            try
            {
                var tripsdata =_context.Trips.Where(c=>c.UserId == userid).ToList();
                var flexidatadata =_context.Flexis.Where(c=>c.UserId == userid).ToList();
                var monthlydata =_context.Monthlies.Where(c=>c.UserId == userid).ToList();
                if (tripsdata.Count > 0 && flexidatadata.Count > 0 && monthlydata.Count > 0)
                {
                    var tripCuponIds = tripsdata.Select(t => t.CuponId).ToList();
                    var flexiid = flexidatadata.Select(t => t.FlexiId).ToList();

                    var flexiCuponIds = _context.FlexiDatesLists
                        .Where(c => flexiid.Contains(Convert.ToInt32(c.FlexiId)))
                        .Select(c => c.CuponId)
                        .ToList();
                    var monthlyId = monthlydata.Select(m => m.MonthlyId).ToList();
                    var monthlyCuponIds = _context.MonthlyDateLists
                        .Where(c => monthlyId.Contains(Convert.ToInt32(c.MonthlyId))).Select(c => c.CuponId).ToList();
                    var cuponsdata = await _context.Cupons
       .Where(c => !tripCuponIds.Contains(c.CuponsId) &&
                   !flexiCuponIds.Contains(c.CuponsId) &&
                   !monthlyCuponIds.Contains(c.CuponsId))
       .ToListAsync();
                    return cuponsdata;
                }
                else if (tripsdata.Count > 0 && flexidatadata.Count > 0)
                {
                    var tripCuponIds = tripsdata.Select(t => t.CuponId).ToList();
                    var flexiid = flexidatadata.Select(t => t.FlexiId).ToList();

                    var flexiCuponIds = _context.FlexiDatesLists
                        .Where(c => flexiid.Contains(Convert.ToInt32(c.FlexiId)))
                        .Select(c => c.CuponId)
                        .ToList();
                   
                    var cuponsdata = await _context.Cupons
                    .Where(c => !tripCuponIds.Contains(c.CuponsId) &&
                   !flexiCuponIds.Contains(c.CuponsId) )
                    .ToListAsync();
                    return cuponsdata;
                }
                else if (tripsdata.Count > 0 && monthlydata.Count > 0) 
                {
                    var tripCuponIds = tripsdata.Select(t => t.CuponId).ToList();
                   
                    var monthlyId = monthlydata.Select(m => m.MonthlyId).ToList();
                    var monthlyCuponIds = _context.MonthlyDateLists
                        .Where(c => monthlyId.Contains(Convert.ToInt32(c.MonthlyId))).Select(c => c.CuponId).ToList();
                    var cuponsdata = await _context.Cupons
                   .Where(c => !tripCuponIds.Contains(c.CuponsId)  &&
                               !monthlyCuponIds.Contains(c.CuponsId))
                   .ToListAsync();
                    return cuponsdata;
                }
                else if (flexidatadata.Count > 0 && monthlydata.Count > 0) 
                {
                  
                    var flexiid = flexidatadata.Select(t => t.FlexiId).ToList();

                    var flexiCuponIds = _context.FlexiDatesLists
                        .Where(c => flexiid.Contains(Convert.ToInt32(c.FlexiId)))
                        .Select(c => c.CuponId)
                        .ToList();
                    var monthlyId = monthlydata.Select(m => m.MonthlyId).ToList();
                    var monthlyCuponIds = _context.MonthlyDateLists
                        .Where(c => monthlyId.Contains(Convert.ToInt32(c.MonthlyId))).Select(c => c.CuponId).ToList();
                    var cuponsdata = await _context.Cupons
                   .Where(c =>
                               !flexiCuponIds.Contains(c.CuponsId) &&
                               !monthlyCuponIds.Contains(c.CuponsId))
                   .ToListAsync();
                    return cuponsdata;
                }
                else { return await _context.Cupons.ToListAsync(); }

            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }

        }

        // GET: api/Cupons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cupons>> GetCupons(int id)
        {
          if (_context.Cupons == null)
          {
              return NotFound();
          }
            var cupons = await _context.Cupons.FindAsync(id);

            if (cupons == null)
            {
                return NotFound();
            }

            return cupons;
        }

        // PUT: api/Cupons/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<IActionResult> PutCupons(int id, Cupons cupons)
        {
            if (id != cupons.CuponsId)
            {
                return BadRequest();
            }

            _context.Entry(cupons).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CuponsExists(id))
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

        // POST: api/Cupons
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cupons>> PostCupons(Cupons cupons)
        {
          if (_context.Cupons == null)
          {
              return Problem("Entity set 'DataContext.Cupons'  is null.");
          }
            _context.Cupons.Add(cupons);
            await _context.SaveChangesAsync();
            cupons.CuponCode = cupons.CuponName?.Substring(0, 3) + cupons.Percentage + cupons.CuponsId;
            _context.Entry(cupons).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetCupons", new { id = cupons.CuponsId }, cupons);
        }

        // DELETE: api/Cupons/5
        [HttpPost("{id}/delete")]
        public async Task<IActionResult> DeleteCupons(int id)
        {
            if (_context.Cupons == null)
            {
                return NotFound();
            }
            var cupons = await _context.Cupons.FindAsync(id);
            if (cupons == null)
            {
                return NotFound();
            }

            _context.Cupons.Remove(cupons);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CuponsExists(int id)
        {
            return (_context.Cupons?.Any(e => e.CuponsId == id)).GetValueOrDefault();
        }
    }
}
