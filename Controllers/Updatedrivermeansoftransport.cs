using GoChauffeurWebApi.Data;
using GoChauffeurWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Updatedrivermeansoftransport : ControllerBase
    {
        private readonly DataContext _context;

        public Updatedrivermeansoftransport(DataContext context)
        {
            _context = context;
        }


        [HttpPost]
        public async Task<ActionResult<Trip>> Update(int tripId, string transport, int flag)
        {
            if (flag == 0)
            {
                var tripData = _context.Trips.Find(tripId);
                if (tripData == null)
                {
                    return BadRequest("Trip does not exist");
                }

                tripData.DriverMeansOfTransport = transport;
                tripData.Isonroute = true;
                _context.Entry(tripData).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(tripData);
            }
            else if (flag == 1)
            {
                var flexiData = _context.Flexis.Find(tripId);
                if (flexiData == null)
                {
                    return BadRequest("Flexi does not exist");
                }

                flexiData.DriverMeansOfTransport = transport;
                flexiData.Isonroute = true;
                _context.Entry(flexiData).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(flexiData);
            }
            else
            {
                var montliData = _context.Monthlies.Find(tripId);
                if (montliData == null)
                {
                    return BadRequest("Monthly does not exist");
                }

                montliData.DriverMeansOfTransport = transport;
                montliData.Isonroute = true;
                _context.Entry(montliData).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(montliData);
            }
        }

    }
}
