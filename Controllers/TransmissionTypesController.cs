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
    public class TransmissionTypesController : ControllerBase
    {
        private readonly DataContext _context;

        public TransmissionTypesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/TransmissionTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransmissionType>>> GetTransmissionTypes()
        {
          if (_context.TransmissionTypes == null)
          {
              return NotFound();
          }
            return await _context.TransmissionTypes.ToListAsync();
        }
        
        // GET: api/TransmissionTypes
        [HttpGet("{id}/vehicletype")]
        public async Task<ActionResult<IEnumerable<TransmissionType>>> GetTransmissionTypes(int id)
        {
          if (_context.TransmissionTypes == null)
          {
              return NotFound();
          }
            return await _context.TransmissionTypes.Where(c=>c.VehicleTypeId== id).ToListAsync();
        }

        // GET: api/TransmissionTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TransmissionType>> GetTransmissionType(int id)
        {
          if (_context.TransmissionTypes == null)
          {
              return NotFound();
          }
            var transmissionType = await _context.TransmissionTypes.FindAsync(id);

            if (transmissionType == null)
            {
                return NotFound();
            }

            return transmissionType;
        }

        // PUT: api/TransmissionTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransmissionType(int id, TransmissionType transmissionType)
        {
            if (id != transmissionType.TransmissionTypeId)
            {
                return BadRequest();
            }

            _context.Entry(transmissionType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransmissionTypeExists(id))
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

        // POST: api/TransmissionTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TransmissionType>> PostTransmissionType(TransmissionType transmissionType)
        {
          if (_context.TransmissionTypes == null)
          {
              return Problem("Entity set 'DataContext.TransmissionTypes'  is null.");
          }
            _context.TransmissionTypes.Add(transmissionType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransmissionType", new { id = transmissionType.TransmissionTypeId }, transmissionType);
        }

        // DELETE: api/TransmissionTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransmissionType(int id)
        {
            if (_context.TransmissionTypes == null)
            {
                return NotFound();
            }
            var transmissionType = await _context.TransmissionTypes.FindAsync(id);
            if (transmissionType == null)
            {
                return NotFound();
            }

            _context.TransmissionTypes.Remove(transmissionType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TransmissionTypeExists(int id)
        {
            return (_context.TransmissionTypes?.Any(e => e.TransmissionTypeId == id)).GetValueOrDefault();
        }
    }
}
