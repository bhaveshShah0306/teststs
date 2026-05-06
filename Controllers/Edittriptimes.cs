using GoChauffeurWebApi.Data;
using GoChauffeurWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Edittriptimes : ControllerBase
    {
        private readonly DataContext _context;

        public Edittriptimes(DataContext context)
        {
            _context = context;
        }


        [HttpPost("{Time}/{Id}")]
        public async Task<ActionResult<IEnumerable<Trip>>> Updatetrip(DateTime? Time , int Id)
        {
            try
            {
                var tripData =await _context.Trips.FindAsync(Id);
                if (tripData != null)
                {
                    if (Time.HasValue)
                    {
                        TimeZoneInfo istTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                        DateTime istStartDateTime = TimeZoneInfo.ConvertTimeFromUtc(Time.Value, istTimeZone);

                        Time = istStartDateTime;

                    }

                    tripData.StartDateTime = Time;
                    var numberofhours = tripData.NoOfHoursSelected;

                    if (numberofhours != null)
                    {
                        var endtime = tripData.StartDateTime?.AddHours(Convert.ToDouble(numberofhours));
                        tripData.EndDateTime = endtime;
                    }
                    _context.Entry(tripData).State = EntityState.Modified;

                        await _context.SaveChangesAsync();
                    return Ok(tripData);
                }
                else { return NoContent(); } 
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
