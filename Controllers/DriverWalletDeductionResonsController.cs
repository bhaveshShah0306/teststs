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
    public class DriverWalletDeductionResonsController : ControllerBase
    {
        private readonly DataContext _context;

        public DriverWalletDeductionResonsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/DriverWalletDeductionResons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DriverWalletDeductionResons>>> GetDriverWalletDeductionResons()
        {
          if (_context.DriverWalletDeductionResons == null)
          {
              return NotFound();
          }
            return await _context.DriverWalletDeductionResons.ToListAsync();
        }

        // GET: api/DriverWalletDeductionResons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DriverWalletDeductionResons>> GetDriverWalletDeductionResons(int id)
        {
          if (_context.DriverWalletDeductionResons == null)
          {
              return NotFound();
          }
            var driverWalletDeductionResons = await _context.DriverWalletDeductionResons.FindAsync(id);

            if (driverWalletDeductionResons == null)
            {
                return NotFound();
            }

            return driverWalletDeductionResons;
        }

        // PUT: api/DriverWalletDeductionResons/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDriverWalletDeductionResons(int id, DriverWalletDeductionResons driverWalletDeductionResons)
        {
            if (id != driverWalletDeductionResons.DriverWalletDeductionResonsId)
            {
                return BadRequest();
            }

            _context.Entry(driverWalletDeductionResons).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriverWalletDeductionResonsExists(id))
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

        // POST: api/DriverWalletDeductionResons
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DriverWalletDeductionResons>> PostDriverWalletDeductionResons(DriverWalletDeductionResons driverWalletDeductionResons)
        {
          if (_context.DriverWalletDeductionResons == null)
          {
              return Problem("Entity set 'DataContext.DriverWalletDeductionResons'  is null.");
          }
            _context.DriverWalletDeductionResons.Add(driverWalletDeductionResons);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDriverWalletDeductionResons", new { id = driverWalletDeductionResons.DriverWalletDeductionResonsId }, driverWalletDeductionResons);
        }

        // DELETE: api/DriverWalletDeductionResons/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriverWalletDeductionResons(int id)
        {
            if (_context.DriverWalletDeductionResons == null)
            {
                return NotFound();
            }
            var driverWalletDeductionResons = await _context.DriverWalletDeductionResons.FindAsync(id);
            if (driverWalletDeductionResons == null)
            {
                return NotFound();
            }

            _context.DriverWalletDeductionResons.Remove(driverWalletDeductionResons);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DriverWalletDeductionResonsExists(int id)
        {
            return (_context.DriverWalletDeductionResons?.Any(e => e.DriverWalletDeductionResonsId == id)).GetValueOrDefault();
        }
    }
}
