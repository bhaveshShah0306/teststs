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
    public class Driver_Joining_FeeController : ControllerBase
    {
        private readonly DataContext _context;

        public Driver_Joining_FeeController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Driver_Joining_Fee
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Driver_Joining_Fee>>> GetDriver_Joining_Fees()
        {
          if (_context.Driver_Joining_Fees == null)
          {
              return NotFound();
          }
            return await _context.Driver_Joining_Fees.ToListAsync();
        }

        // GET: api/Driver_Joining_Fee/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Driver_Joining_Fee>> GetDriver_Joining_Fee(int id)
        {
          if (_context.Driver_Joining_Fees == null)
          {
              return NotFound();
          }
            var driver_Joining_Fee = await _context.Driver_Joining_Fees.FindAsync(id);

            if (driver_Joining_Fee == null)
            {
                return NotFound();
            }

            return driver_Joining_Fee;
        }

        // PUT: api/Driver_Joining_Fee/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDriver_Joining_Fee(int id, Driver_Joining_Fee driver_Joining_Fee)
        {
            if (id != driver_Joining_Fee.Driver_Joining_FeeId)
            {
                return BadRequest();
            }

            _context.Entry(driver_Joining_Fee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Driver_Joining_FeeExists(id))
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

        // POST: api/Driver_Joining_Fee
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Driver_Joining_Fee>> PostDriver_Joining_Fee(Driver_Joining_Fee driver_Joining_Fee)
        {
          if (_context.Driver_Joining_Fees == null)
          {
              return Problem("Entity set 'DataContext.Driver_Joining_Fees'  is null.");
          }
            _context.Driver_Joining_Fees.Add(driver_Joining_Fee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDriver_Joining_Fee", new { id = driver_Joining_Fee.Driver_Joining_FeeId }, driver_Joining_Fee);
        }

        // DELETE: api/Driver_Joining_Fee/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriver_Joining_Fee(int id)
        {
            if (_context.Driver_Joining_Fees == null)
            {
                return NotFound();
            }
            var driver_Joining_Fee = await _context.Driver_Joining_Fees.FindAsync(id);
            if (driver_Joining_Fee == null)
            {
                return NotFound();
            }

            _context.Driver_Joining_Fees.Remove(driver_Joining_Fee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Driver_Joining_FeeExists(int id)
        {
            return (_context.Driver_Joining_Fees?.Any(e => e.Driver_Joining_FeeId == id)).GetValueOrDefault();
        }
    }
}
