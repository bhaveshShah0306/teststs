using GoChauffeurWebApi.Data;
using GoChauffeurWebApi.Models;
using GoChauffeurWebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OutstationinsurenceController : ControllerBase
    {
        private readonly DataContext _context;

        public OutstationinsurenceController(DataContext context)
        {
            _context = context;
        }


        // GET: api/Outstationinsurence
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Outstationinsurence>>> GetOutstationinsurence()
        {
            if (_context.Outstationinsurence == null)
            {
                return NotFound();
            }
            return await _context.Outstationinsurence.ToListAsync();
        }

        // GET: api/Outstationinsurence/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OutstationinsurenceVM>> GetOutstationinsurence(int id)
        {
            try
            {
                var outstationinsurencedata = await _context.Outstationinsurence.FindAsync(id);
                if (outstationinsurencedata != null) 
                {
                    var vm = new OutstationinsurenceVM();
                    vm.OutstationinsurenceId = outstationinsurencedata.OutstationinsurenceId;
                    vm.onewayPrice= outstationinsurencedata.onewayPrice;
                    vm.Onewaypercentage = outstationinsurencedata.Onewaypercentage;
                    var onewayprice = vm.onewayPrice*vm.Onewaypercentage/100;
                    vm.Onewaytaxprice = onewayprice;
                    vm.RoundtripPrice = outstationinsurencedata.RoundtripPrice;
                    vm.Roundtrippercentage= outstationinsurencedata.Roundtrippercentage;
                    var roundtripprice = vm.RoundtripPrice*vm.Roundtrippercentage/100;
                    vm.Roundtriptaxprice = roundtripprice;

                    return Ok(vm);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // PUT: api/Outstationinsurence/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<IActionResult> PutOutstationinsurence(int id, Outstationinsurence Outstationinsurence)
        {
            if (id != Outstationinsurence.OutstationinsurenceId)
            {
                return BadRequest();
            }

            _context.Entry(Outstationinsurence).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OutstationinsurenceExists(id))
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


        // DELETE: api/Outstationinsurence/5
        [HttpPost("{id}/delete")]
        public async Task<IActionResult> DeleteOutstationinsurence(int id)
        {
            if (_context.Outstationinsurence == null)
            {
                return NotFound();
            }
            var Outstationinsurence = await _context.Outstationinsurence.FindAsync(id);
            if (Outstationinsurence == null)
            {
                return NotFound();
            }

            _context.Outstationinsurence.Remove(Outstationinsurence);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OutstationinsurenceExists(int id)
        {
            return (_context.Outstationinsurence?.Any(e => e.OutstationinsurenceId == id)).GetValueOrDefault();
        }
    }
}
