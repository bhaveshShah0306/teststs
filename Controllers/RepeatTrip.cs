using GoChauffeurWebApi.Data;
using GoChauffeurWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepeatTrip : ControllerBase
    {
        private readonly DataContext _context;

        public RepeatTrip(DataContext context)
        {
            _context = context;
        }



[HttpPost]
    public async Task<ActionResult<Trip>> PostTrip(int tripId, DateTime requestedDateTime ,int noHoursSelected , DateTime startdatetime)
    {
        try
        {
            var existingTrip = await _context.Trips.FindAsync(tripId);
            if (existingTrip == null)
            {
                return NotFound($"Trip with ID {tripId} not found.");
            }

            var newTrip = new Trip
            {
                IsTripStarted = false,
                IsTripCompByDriver = false,
                CreatedDate = DateTime.UtcNow, // Set to UTC, adjust if necessary
                RequstedDateTime = requestedDateTime,
                NoOfHoursSelected = noHoursSelected,
                StartDateTime = startdatetime
            };

            // Copy properties, excluding specific ones
            foreach (PropertyInfo propertyInfo in existingTrip.GetType().GetProperties())
            {
                if (propertyInfo.CanWrite && !new HashSet<string> { "TripId", "StartDateTime", "DriverId", "NoOfHoursSelected", "IsTripStarted", "IsTripCompByDriver", "ImageUrlsList", "CuponId", "CreatedDate", "RequestedDateTime", "EndDateTime" }.Contains(propertyInfo.Name))
                {
                    var value = propertyInfo.GetValue(existingTrip);
                    propertyInfo.SetValue(newTrip, value);
                }
            }

            // Additional Logic
            newTrip.IsTripStarted = false;
            newTrip.IsTripCompByDriver = false;

            // Adjust StartDateTime to IST and calculate EndDateTime
            if (newTrip.StartDateTime.HasValue)
            {
                TimeZoneInfo istTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime istStartDateTime = TimeZoneInfo.ConvertTimeFromUtc(newTrip.StartDateTime.Value, istTimeZone);
                newTrip.StartDateTime = istStartDateTime;

                if (newTrip.NoOfHoursSelected.HasValue)
                {
                    var endTime = newTrip.StartDateTime.Value.AddHours(Convert.ToDouble(newTrip.NoOfHoursSelected.Value));
                    newTrip.EndDateTime = endTime;
                }
            }

            _context.Trips.Add(newTrip);
            await _context.SaveChangesAsync();

            return Ok(newTrip);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


}
}
