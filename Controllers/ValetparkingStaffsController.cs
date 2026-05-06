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
    public class ValetparkingStaffsController : ControllerBase
    {
        private readonly DataContext _context;

        public ValetparkingStaffsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/ValetparkingStaffs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ValetparkingStaff>>> GetValetparkingStaff()
        {
          if (_context.ValetparkingStaff == null)
          {
              return NotFound();
          }
            return await _context.ValetparkingStaff.ToListAsync();
        }

        // GET: api/ValetparkingStaffs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ValetparkingStaff>> GetValetparkingStaff(int id)
        {
          if (_context.ValetparkingStaff == null)
          {
              return NotFound();
          }
            var valetparkingStaff = await _context.ValetparkingStaff.FindAsync(id);

            if (valetparkingStaff == null)
            {
                return NotFound();
            }

            return valetparkingStaff;
        }

        // PUT: api/ValetparkingStaffs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutValetparkingStaff(int id, ValetparkingStaff valetparkingStaff)
        {
            if (id != valetparkingStaff.ValetparkingStaffId)
            {
                return BadRequest();
            }

            _context.Entry(valetparkingStaff).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ValetparkingStaffExists(id))
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

        // POST: api/ValetparkingStaffs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ValetparkingStaff>> PostValetparkingStaff(ValetparkingStaff valetparkingStaff)
        {
          if (_context.ValetparkingStaff == null)
          {
              return Problem("Entity set 'DataContext.ValetparkingStaff'  is null.");
          }
            _context.ValetparkingStaff.Add(valetparkingStaff);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetValetparkingStaff", new { id = valetparkingStaff.ValetparkingStaffId }, valetparkingStaff);
        }

        // DELETE: api/ValetparkingStaffs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteValetparkingStaff(int id)
        {
            if (_context.ValetparkingStaff == null)
            {
                return NotFound();
            }
            var valetparkingStaff = await _context.ValetparkingStaff.FindAsync(id);
            if (valetparkingStaff == null)
            {
                return NotFound();
            }

            _context.ValetparkingStaff.Remove(valetparkingStaff);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ValetparkingStaffExists(int id)
        {
            return (_context.ValetparkingStaff?.Any(e => e.ValetparkingStaffId == id)).GetValueOrDefault();
        }
    }
}
