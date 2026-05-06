using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GoChauffeurWebApi.Data;
using GoChauffeurWebApi.Models;
using GoChauffeurWebApi.ViewModels;

namespace GunturPickles_Grocery_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleTypesController : ControllerBase
    {
        private readonly DataContext _context;

        public VehicleTypesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/VehicleTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleTypeVM>>> GetVehicleTypes()
        {
            try
            {
               var vehicleTypes = await _context.VehicleTypes.ToListAsync();
               return  Ok(vehicleTypes.Adapt<List<VehicleTypeVM>>());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/VehicleTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleTypeVM>> GetVehicleType(int id)
        {
            try
            {
                var vType = await _context.VehicleTypes.FindAsync(id);
                return Ok(vType.Adapt<VehicleTypeVM>());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/VehicleTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<IActionResult> PutVehicleType(int id, VehicleType vehicleType)
        {
            if (id != vehicleType.VehicleTypeId)
            {
                return BadRequest();
            }

            _context.Entry(vehicleType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleTypesExists(id))
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

        // POST: api/VehicleTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<VehicleType>> PostVehicleType(VehicleType vType)
        {
            if (_context.VehicleTypes == null)
            {
                return Problem("Entity set 'DataContext.VehicleTypes'  is null.");
            }
            _context.VehicleTypes.Add(vType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVehicleType", new { id = vType.VehicleTypeId }, vType);
        }

        // DELETE: api/VehicleTypes/5
        [HttpPost("{id}/Delete")]
        public async Task<IActionResult> DeleteVehicleType(int id)
        {
            if (_context.VehicleTypes == null)
            {
                return NotFound();
            }
            var vehicleType = await _context.VehicleTypes.FindAsync(id);
            if (vehicleType == null)
            {
                return NotFound();
            }

            _context.VehicleTypes.Remove(vehicleType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VehicleTypesExists(int id)
        {
            return (_context.VehicleTypes?.Any(e => e.VehicleTypeId == id)).GetValueOrDefault();
        }
    }
}
