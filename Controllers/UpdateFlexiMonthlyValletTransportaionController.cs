using GoChauffeurWebApi.Data;
using GoChauffeurWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateFlexiMonthlyValletTransportaionController : ControllerBase
    {
        private readonly DataContext _context;

        public UpdateFlexiMonthlyValletTransportaionController(DataContext context)
        {
            _context = context;
        }


        [HttpPost]
        public async Task<ActionResult> updatetransportaion(int Id, int flag, string transport)
        {
            if (flag == 1)
            {
                var Flexidata = await _context.Flexis.FindAsync(Id);
                if (Flexidata == null)
                {
                    return BadRequest("Trip does not exist");
                }

                Flexidata.DriverMeansOfTransport = transport;
                _context.Entry(Flexidata).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(Flexidata);
            }
            else if (flag == 2)
            {
                var Monthliesdata = await _context.Monthlies.FindAsync(Id);
                if (Monthliesdata == null)
                {
                    return BadRequest("Monthlies do not exist");
                }

                Monthliesdata.DriverMeansOfTransport = transport;
                _context.Entry(Monthliesdata).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(Monthliesdata);
            }
            else if (flag == 3)
            {
                var ValetParkingsdata = await _context.ValetParkings.FindAsync(Id);
                if (ValetParkingsdata == null)
                {
                    return BadRequest("ValetParkings do not exist");
                }

                ValetParkingsdata.DriverMeansOfTransport = transport;
                _context.Entry(ValetParkingsdata).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(ValetParkingsdata);
            }
            else
            {
                return BadRequest("Invalid flag value");
            }
        }

    }
}
