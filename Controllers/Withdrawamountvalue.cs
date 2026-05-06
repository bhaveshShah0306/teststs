using GoChauffeurWebApi.Data;
using GoChauffeurWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WithdrawamountvalueController : ControllerBase
    {
        private readonly DataContext _context;

        public WithdrawamountvalueController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Withdrawamountvalue
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Withdrawamountvalue>>> GetWithdrawamountvalues()
        {
            if (_context.Withdrawamountvalue == null)
            {
                return NotFound(new
                {
                    Status = "Error",
                    Message = "Withdrawamountvalue entity set is null."
                });
            }

            var withdrawamountvalues = await _context.Withdrawamountvalue.ToListAsync();

            return Ok(new
            {
                Status = "Success",
                Data = withdrawamountvalues
            });
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> PutWithdrawRequest(int id, Withdrawamountvalue withdrawRequest)
        {
            if (id != withdrawRequest.WithdrawamountvalueId)
            {
                return BadRequest();
            }

            _context.Entry(withdrawRequest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WithdrawRequestExists(id))
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
        private bool WithdrawRequestExists(int id)
        {
            return (_context.WithdrawRequest?.Any(e => e.WithdrawRequestId == id)).GetValueOrDefault();
        }
    }
}
