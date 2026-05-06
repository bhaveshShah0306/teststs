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
    public class FavouriteTypesController : ControllerBase
    {
        private readonly DataContext _context;

        public FavouriteTypesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/FavouriteTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FavouriteTypes>>> GetFavouriteTypes()
        {
          if (_context.FavouriteTypes == null)
          {
              return NotFound();
          }
            return await _context.FavouriteTypes.ToListAsync();
        }

        // GET: api/FavouriteTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FavouriteTypes>> GetFavouriteTypes(int id)
        {
          if (_context.FavouriteTypes == null)
          {
              return NotFound();
          }
            var favouriteTypes = await _context.FavouriteTypes.FindAsync(id);

            if (favouriteTypes == null)
            {
                return NotFound();
            }

            return favouriteTypes;
        }

        // PUT: api/FavouriteTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFavouriteTypes(int id, FavouriteTypes favouriteTypes)
        {
            if (id != favouriteTypes.FavouriteTypesId)
            {
                return BadRequest();
            }

            _context.Entry(favouriteTypes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FavouriteTypesExists(id))
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

        // POST: api/FavouriteTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FavouriteTypes>> PostFavouriteTypes(FavouriteTypes favouriteTypes)
        {
          if (_context.FavouriteTypes == null)
          {
              return Problem("Entity set 'DataContext.FavouriteTypes'  is null.");
          }
            _context.FavouriteTypes.Add(favouriteTypes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFavouriteTypes", new { id = favouriteTypes.FavouriteTypesId }, favouriteTypes);
        }

        // DELETE: api/FavouriteTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFavouriteTypes(int id)
        {
            if (_context.FavouriteTypes == null)
            {
                return NotFound();
            }
            var favouriteTypes = await _context.FavouriteTypes.FindAsync(id);
            if (favouriteTypes == null)
            {
                return NotFound();
            }

            _context.FavouriteTypes.Remove(favouriteTypes);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FavouriteTypesExists(int id)
        {
            return (_context.FavouriteTypes?.Any(e => e.FavouriteTypesId == id)).GetValueOrDefault();
        }
    }
}
