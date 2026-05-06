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
    public class UsersTripsCancelResonsController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersTripsCancelResonsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/UsersTripsCancelResons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsersTripsCancelResons>>> GetUsersTripsCancelResons()
        {
          if (_context.UsersTripsCancelResons == null)
          {
              return NotFound();
          }
            return await _context.UsersTripsCancelResons.ToListAsync();
        }

        // GET: api/UsersTripsCancelResons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsersTripsCancelResons>> GetUsersTripsCancelResons(int id)
        {
          if (_context.UsersTripsCancelResons == null)
          {
              return NotFound();
          }
            var usersTripsCancelResons = await _context.UsersTripsCancelResons.FindAsync(id);

            if (usersTripsCancelResons == null)
            {
                return NotFound();
            }

            return usersTripsCancelResons;
        }

        // PUT: api/UsersTripsCancelResons/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<IActionResult> PutUsersTripsCancelResons(int id, UsersTripsCancelResons usersTripsCancelResons)
        {
            if (id != usersTripsCancelResons.UsersTripsCancelResonsId)
            {
                return BadRequest();
            }

            _context.Entry(usersTripsCancelResons).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersTripsCancelResonsExists(id))
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

        // POST: api/UsersTripsCancelResons
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UsersTripsCancelResons>> PostUsersTripsCancelResons(UsersTripsCancelResons usersTripsCancelResons)
        {
          if (_context.UsersTripsCancelResons == null)
          {
              return Problem("Entity set 'DataContext.UsersTripsCancelResons'  is null.");
          }
            _context.UsersTripsCancelResons.Add(usersTripsCancelResons);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsersTripsCancelResons", new { id = usersTripsCancelResons.UsersTripsCancelResonsId }, usersTripsCancelResons);
        }

        // DELETE: api/UsersTripsCancelResons/5
        [HttpPost("{id}/delete")]
        public async Task<IActionResult> DeleteUsersTripsCancelResons(int id)
        {
            if (_context.UsersTripsCancelResons == null)
            {
                return NotFound();
            }
            var usersTripsCancelResons = await _context.UsersTripsCancelResons.FindAsync(id);
            if (usersTripsCancelResons == null)
            {
                return NotFound();
            }

            _context.UsersTripsCancelResons.Remove(usersTripsCancelResons);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsersTripsCancelResonsExists(int id)
        {
            return (_context.UsersTripsCancelResons?.Any(e => e.UsersTripsCancelResonsId == id)).GetValueOrDefault();
        }
    }
}
