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
    public class SubscriptionHistoriesController : ControllerBase
    {
        private readonly DataContext _context;

        public SubscriptionHistoriesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/SubscriptionHistories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubscriptionHistoryVM>>> GetSubHistories()
        {
            try
            {
               var subHist = await _context.SubscriptionHistories.ToListAsync();
               return  Ok(subHist.Adapt<List<SubscriptionHistoryVM>>());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/SubscriptionHistories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SubscriptionHistoryVM>> GetSubHistory(int id)
        {
            try
            {
                var subHist = await _context.SubscriptionHistories.FindAsync(id);
                return Ok(subHist.Adapt<SubscriptionHistoryVM>());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/SubscriptionHistories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<IActionResult> PutSubHistory(int id, SubscriptionHistory subHist)
        {
            if (id != subHist.Id)
            {
                return BadRequest();
            }

            _context.Entry(subHist).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubHistsExists(id))
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

        // POST: api/SubscriptionHistories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SubscriptionHistory>> PostSubscriptionhistory(SubscriptionHistory subHist)
        {
            if (_context.SubscriptionHistories == null)
            {
                return Problem("Entity set 'DataContext.SubscriptionHistories'  is null.");
            }
            _context.SubscriptionHistories.Add(subHist);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSubHistory", new { id = subHist.Id }, subHist);
        }

        // DELETE: api/SubscriptionHistories/5
        [HttpPost("{id}/Delete")]
        public async Task<IActionResult> DeleteSubHistory(int id)
        {
            if (_context.SubscriptionHistories == null)
            {
                return NotFound();
            }
            var users = await _context.SubscriptionHistories.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }

            _context.SubscriptionHistories.Remove(users);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SubHistsExists(int id)
        {
            return (_context.SubscriptionHistories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
