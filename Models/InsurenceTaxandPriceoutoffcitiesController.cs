using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GoChauffeurWebApi.Data;

namespace GoChauffeurWebApi.Models
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsurenceTaxandPriceoutoffcitiesController : ControllerBase
    {
        private readonly DataContext _context;

        public InsurenceTaxandPriceoutoffcitiesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/InsurenceTaxandPriceoutoffcities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InsurenceTaxandPriceoutoffcity>>> GetInsurenceTaxandPriceoutoffcity()
        {
          if (_context.InsurenceTaxandPriceoutoffcity == null)
          {
              return NotFound();
          }
            return await _context.InsurenceTaxandPriceoutoffcity.ToListAsync();
        }

        // GET: api/InsurenceTaxandPriceoutoffcities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InsurenceTaxandPriceoutoffcity>> GetInsurenceTaxandPriceoutoffcity(int id)
        {
          if (_context.InsurenceTaxandPriceoutoffcity == null)
          {
              return NotFound();
          }
            var insurenceTaxandPriceoutoffcity = await _context.InsurenceTaxandPriceoutoffcity.FindAsync(id);

            if (insurenceTaxandPriceoutoffcity == null)
            {
                return NotFound();
            }

            return insurenceTaxandPriceoutoffcity;
        }

        // PUT: api/InsurenceTaxandPriceoutoffcities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInsurenceTaxandPriceoutoffcity(int id, InsurenceTaxandPriceoutoffcity insurenceTaxandPriceoutoffcity)
        {
            if (id != insurenceTaxandPriceoutoffcity.InsurenceTaxandPriceoutoffcityId)
            {
                return BadRequest();
            }

            _context.Entry(insurenceTaxandPriceoutoffcity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InsurenceTaxandPriceoutoffcityExists(id))
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

        // POST: api/InsurenceTaxandPriceoutoffcities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<InsurenceTaxandPriceoutoffcity>> PostInsurenceTaxandPriceoutoffcity(InsurenceTaxandPriceoutoffcity insurenceTaxandPriceoutoffcity)
        {
          if (_context.InsurenceTaxandPriceoutoffcity == null)
          {
              return Problem("Entity set 'DataContext.InsurenceTaxandPriceoutoffcity'  is null.");
          }
            _context.InsurenceTaxandPriceoutoffcity.Add(insurenceTaxandPriceoutoffcity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInsurenceTaxandPriceoutoffcity", new { id = insurenceTaxandPriceoutoffcity.InsurenceTaxandPriceoutoffcityId }, insurenceTaxandPriceoutoffcity);
        }

        // DELETE: api/InsurenceTaxandPriceoutoffcities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInsurenceTaxandPriceoutoffcity(int id)
        {
            if (_context.InsurenceTaxandPriceoutoffcity == null)
            {
                return NotFound();
            }
            var insurenceTaxandPriceoutoffcity = await _context.InsurenceTaxandPriceoutoffcity.FindAsync(id);
            if (insurenceTaxandPriceoutoffcity == null)
            {
                return NotFound();
            }

            _context.InsurenceTaxandPriceoutoffcity.Remove(insurenceTaxandPriceoutoffcity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InsurenceTaxandPriceoutoffcityExists(int id)
        {
            return (_context.InsurenceTaxandPriceoutoffcity?.Any(e => e.InsurenceTaxandPriceoutoffcityId == id)).GetValueOrDefault();
        }
    }
}
