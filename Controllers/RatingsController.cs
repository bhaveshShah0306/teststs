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
    public class RatingsController : ControllerBase
    {
        private readonly DataContext _context;

        public RatingsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Ratings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ratings>>> GetRatings()
        {
          if (_context.Ratings == null)
          {
              return NotFound();
          }
            return await _context.Ratings.ToListAsync();
        }

		[HttpGet("driver/{driverId}")]
		public async Task<ActionResult> GetRatingsByDriver(int driverId)
		{
			var ratings = await _context.Ratings
				.Where(r => r.DriverId == driverId && r.Rating != null && r.Rating > 0)
				.ToListAsync();

			if (!ratings.Any())
				return Ok(new { averageRating = 0, totalRatings = 0, ratings = new List<Ratings>() });

			var average = ratings.Average(r => r.Rating!.Value);

			return Ok(new
			{
				averageRating = Math.Round(average, 1),
				totalRatings = ratings.Count,
				ratings
			});
		}
		// GET: api/Ratings/5
		[HttpGet("{id}")]
        public async Task<ActionResult<Ratings>> GetRatings(int id)
        {
          if (_context.Ratings == null)
          {
              return NotFound();
          }
            var ratings = await _context.Ratings.FindAsync(id);

            if (ratings == null)
            {
                return NotFound();
            }

            return ratings;
        }

        // PUT: api/Ratings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRatings(int id, Ratings ratings)
        {
            if (id != ratings.RatingsId)
            {
                return BadRequest();
            }

            _context.Entry(ratings).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RatingsExists(id))
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

        // POST: api/Ratings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Ratings>> PostRatings(Ratings ratings)
        {
          if (_context.Ratings == null)
          {
              return Problem("Entity set 'DataContext.Ratings'  is null.");
          }
            _context.Ratings.Add(ratings);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRatings", new { id = ratings.RatingsId }, ratings);
        }

        // DELETE: api/Ratings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRatings(int id)
        {
            if (_context.Ratings == null)
            {
                return NotFound();
            }
            var ratings = await _context.Ratings.FindAsync(id);
            if (ratings == null)
            {
                return NotFound();
            }

            _context.Ratings.Remove(ratings);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RatingsExists(int id)
        {
            return (_context.Ratings?.Any(e => e.RatingsId == id)).GetValueOrDefault();
        }
    }
}
