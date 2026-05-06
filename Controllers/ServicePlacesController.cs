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
    public class ServicePlacesController : ControllerBase
    {
        private readonly DataContext _context;

        public ServicePlacesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/ServicePlaces
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServicePlaces>>> GetServicePlaces()
        {
          if (_context.ServicePlaces == null)
          {
              return NotFound();
          }
            var pinCodes = await _context.ServicePlaces.Select(sp => sp.PinCode).ToListAsync();
            return Ok(pinCodes);
        }
        
        [HttpGet("{flag}/1")]
        public async Task<ActionResult<IEnumerable<ServicePlaces>>> GetServicePlace(int flag)
        {
            var list = _context.ServicePlaces.ToList();
            return Ok(list);
        }
        // GET: api/ServicePlaces/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServicePlaces>> GetServicePlaces(int id)
        {
          if (_context.ServicePlaces == null)
          {
              return NotFound();
          }
            var servicePlaces = await _context.ServicePlaces.FindAsync(id);

            if (servicePlaces == null)
            {
                return NotFound();
            }

            return servicePlaces;
        }

        // PUT: api/ServicePlaces/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<IActionResult> PutServicePlaces(int id, ServicePlaces servicePlaces)
        {
            if (id != servicePlaces.ServicePlacesId)
            {
                return BadRequest();
            }

            _context.Entry(servicePlaces).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServicePlacesExists(id))
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

        // POST: api/ServicePlaces
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ServicePlaces>> PostServicePlaces(ServicePlaces servicePlaces)
        {
          if (_context.ServicePlaces == null)
          {
              return Problem("Entity set 'DataContext.ServicePlaces'  is null.");
          }
            _context.ServicePlaces.Add(servicePlaces);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetServicePlaces", new { id = servicePlaces.ServicePlacesId }, servicePlaces);
        }

        // DELETE: api/ServicePlaces/5
        [HttpPost("{id}/delete")]
        public async Task<IActionResult> DeleteServicePlaces(int id)
        {
            if (_context.ServicePlaces == null)
            {
                return NotFound();
            }
            var servicePlaces = await _context.ServicePlaces.FindAsync(id);
            if (servicePlaces == null)
            {
                return NotFound();
            }

            _context.ServicePlaces.Remove(servicePlaces);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ServicePlacesExists(int id)
        {
            return (_context.ServicePlaces?.Any(e => e.ServicePlacesId == id)).GetValueOrDefault();
        }
    }
}
