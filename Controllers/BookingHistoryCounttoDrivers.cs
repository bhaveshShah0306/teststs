using GoChauffeurWebApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingHistoryCounttoDrivers : ControllerBase
    {
        private readonly DataContext _context;

        public BookingHistoryCounttoDrivers(DataContext context)
        {
            _context = context;
        }
        private class Bookingcountbyid
        {
            public DateTime? Date { get; set; }
            public int? Count { get; set; }
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<dynamic>>> Getbookingcount(int driverId, int count)
        {
            try
            {
                var date = DateTime.Now.AddDays(-count).Date;
                var today = DateTime.Now.Date;

                var bookings = _context.Trips
                    .Where(c => c.DriverId == driverId && c.IsTripCompByDriver == true && c.StartDateTime.HasValue && c.StartDateTime.Value.Date >= date && c.StartDateTime.Value.Date <= today)
                    .Select(c => new { c.StartDateTime.Value.Date, Type = "Trip" })
                    .ToList();

                var flexis = _context.Flexis
                    .Where(c => c.DriverId == driverId && c.CloseTrip == true && c.FlexiSelectedDateTime.HasValue && c.FlexiSelectedDateTime.Value.Date >= date && c.FlexiSelectedDateTime.Value.Date <= today)
                    .Select(c => new { c.FlexiSelectedDateTime.Value.Date, Type = "Flexi" })
                    .ToList();

                var monthly = _context.Monthlies
                    .Where(c => c.DriverId == driverId && c.CloseTrip == true && c.SelectedDate.HasValue && c.SelectedDate.Value.Date >= date && c.SelectedDate.Value.Date <= today)
                    .Select(c => new { c.SelectedDate.Value.Date, Type = "Monthly" })
                    .ToList();

                var allBookings = bookings
                    .Concat(flexis)
                    .Concat(monthly)
                    .GroupBy(b => b.Date)
                    .Select(g => new Bookingcountbyid
                    {
                        Date = g.Key,
                        Count = g.Count()
                    })
                    .ToList();

                return Ok(allBookings);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
