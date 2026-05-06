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
    public class ValetParkingPricingsController : ControllerBase
    {
        private readonly DataContext _context;

        public ValetParkingPricingsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/ValetParkingPricings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ValetParkingPricing>>> GetValetParkingPricing()
        {
          if (_context.ValetParkingPricing == null)
          {
              return NotFound();
          }
            return await _context.ValetParkingPricing.ToListAsync();
        }

        // GET: api/ValetParkingPricings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ValetParkingPricing>> GetValetParkingPricing(int id)
        {
          if (_context.ValetParkingPricing == null)
          {
              return NotFound();
          }
            var valetParkingPricing = await _context.ValetParkingPricing.FindAsync(id);

            if (valetParkingPricing == null)
            {
                return NotFound();
            }

            return valetParkingPricing;
        }

        // PUT: api/ValetParkingPricings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<IActionResult> PutValetParkingPricing(int id, ValetParkingPricing valetParkingPricing)
        {
            if (id != valetParkingPricing.ValetParkingPricingId)
            {
                return BadRequest();
            }

            _context.Entry(valetParkingPricing).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ValetParkingPricingExists(id))
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

        // POST: api/ValetParkingPricings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ValetParkingPricing>> PostValetParkingPricing(ValetParkingPricing valetParkingPricing)
        {
          if (_context.ValetParkingPricing == null)
          {
              return Problem("Entity set 'DataContext.ValetParkingPricing'  is null.");
          }
            _context.ValetParkingPricing.Add(valetParkingPricing);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetValetParkingPricing", new { id = valetParkingPricing.ValetParkingPricingId }, valetParkingPricing);
        }

        // DELETE: api/ValetParkingPricings/5
        [HttpPost("{id}/delete")]
        public async Task<IActionResult> DeleteValetParkingPricing(int id)
        {
            if (_context.ValetParkingPricing == null)
            {
                return NotFound();
            }
            var valetParkingPricing = await _context.ValetParkingPricing.FindAsync(id);
            if (valetParkingPricing == null)
            {
                return NotFound();
            }

            _context.ValetParkingPricing.Remove(valetParkingPricing);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ValetParkingPricingExists(int id)
        {
            return (_context.ValetParkingPricing?.Any(e => e.ValetParkingPricingId == id)).GetValueOrDefault();
        }
    }
}
