using GoChauffeurWebApi.Data;
using GoChauffeurWebApi.Models;
using GoChauffeurWebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverDashboard : ControllerBase
    {
        private readonly DataContext _context;

        public DriverDashboard(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<Driver>> Getdriverdata(int id)
        {
            try
            {
                var driverdata = await _context.Drivers.FindAsync(id);
                if (driverdata != null)
                {
                    var driverName = driverdata.DriverName;
                    var driverimage = driverdata.Image;
                    var driverID = driverdata.DriverRecID;
                    var tripscount = _context.Trips.Count(c => c.DriverId == id && (c.IsTripCompByDriver ?? false)&&c.EndDateTime.HasValue && c.EndDateTime.Value.Date==DateTime.Now.Date);
                    var completedFlexisIds = await _context.Flexis
       .Where(f => f.DriverId == id && f.IsTripCompByDriver)
       .Select(f => f.FlexiId)
       .ToListAsync();

                    // Count all entries in FlexiDatesLists that are related to the completed Flexis and check the conditions.
                    var countClosedTripsToday = await _context.FlexiDatesLists
                        .CountAsync(fdl => completedFlexisIds.Contains(fdl.FlexiId.Value) && fdl.Date.HasValue && fdl.Date.Value.Date== DateTime.Now.Date);
                    var flexicount = _context.Flexis.Count(c => c.DriverId == id && (c.IsTripCompByDriver == true));
                    var completedMonthliesIds = await _context.Monthlies
       .Where(f => f.DriverId == id && f.IsTripCompByDriver)
       .Select(f => f.MonthlyId)
       .ToListAsync();

                    // Count all entries in FlexiDatesLists that are related to the completed Flexis and check the conditions.
                    var countClosedMonthliesTripsToday = await _context.MonthlyDateLists
                        .CountAsync(fdl => completedMonthliesIds.Contains(fdl.MonthlyId.Value) && fdl.Date.HasValue && fdl.Date.Value.Date == DateTime.Now.Date);
                    var monthlycount = _context.Monthlies.Count(c => c.DriverId == id && (c.IsTripCompByDriver == true));
                    var tripsdata = tripscount + countClosedTripsToday + countClosedMonthliesTripsToday;
                        

                    // First, retrieve the data from the database
                    var tripPrices = _context.Trips
                        .Where(c => c.DriverId == id && c.DriversPrice != null &&c.EndDateTime.HasValue&&c.EndDateTime.Value.Date == DateTime.Now.Date)
                        .Select(c => c.DriversPrice)
                        .ToList();
                    
                    // Then, process it in memory
                    var tripsearning = tripPrices
                        .Select(price => price)
                        .Sum() ?? 0; 

              

                    // Trips hours
                    var trips = _context.Trips
                        .Where(c => c.DriverId == id && (c.IsTripCompByDriver ?? false))
                        .ToList();
                    var hoursListtrip = trips.Where(c=>c.EndDateTime.HasValue && c.EndDateTime.Value.Date == DateTime.Now.Date).Select(c => c.NoOfHoursActual).ToList();
                    var summedHourstrip = hoursListtrip.Sum(hours => hours ?? 0);

                    // Flexis hours
                    var Flexistrips = _context.Flexis
                       .Where(c => c.DriverId == id && (c.IsTripCompByDriver == true))
                       .ToList();
                    var hoursFlexistrips = Flexistrips.Select(c => c.EstimatedHours).ToList();
                    var summedHoursflexi = hoursFlexistrips.Sum(hours => hours ?? 0);

                    //montly hours
                  var Monthlies = _context.Monthlies
                      .Where(c => c.DriverId == id && (c.IsTripCompByDriver == true))
                      .ToList();
                    var hoursMonthlies = Monthlies.Select(c => c.EstimatedHours).ToList();
                    var summedHourshoursMonthlies = hoursMonthlies.Sum(hours => hours = 0);
                    // total sum
                    var summedHours =Math.Abs( summedHourstrip + summedHoursflexi + summedHourshoursMonthlies);


                    //trip rating
                    var totalTripsRating = trips.Select(c => c.DriverRating).Sum(rating => rating ?? 0);
                    double averageOfFive = totalTripsRating / 5.0;

                    var data = new
                    {
                        id = driverID,
                        name = driverName,
                        image = driverimage,
                        todaysTripCount = tripsdata,
                        rating = averageOfFive,
                        hoursSpent = summedHours,
                        earnings = Math.Round(tripsearning,0)
                    };

                    return Ok(data);
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

        [HttpGet("Overall")]
        public async Task<ActionResult<Driver>> Getdriverdataoverall(int id)
        {
            try
            {
                var driverdata = await _context.Drivers.FindAsync(id);
                if (driverdata != null)
                {
                    var driverName = driverdata.DriverName;
                    var driverimage = driverdata.Image;
                    var driverID = driverdata.DriverRecID;
                    var tripscount = _context.Trips.Count(c => c.DriverId == id && (c.IsTripCompByDriver ?? false));
                    var flexicount = _context.Flexis.Count(c => c.DriverId == id && (c.IsTripCompByDriver == true));
                    var monthlycount = _context.Monthlies.Count(c => c.DriverId == id && (c.IsTripCompByDriver == true));
                    var tripsdata = tripscount + flexicount + monthlycount;


                    // First, retrieve the data from the database
                    var tripPrices = _context.Trips
                        .Where(c => c.DriverId == id && c.DriversPrice != null )
                        .Select(c => c.DriversPrice)
                        .ToList();

                    // Then, process it in memory
                    var tripsearning = tripPrices
                        .Select(price => price)
                        .Sum() ?? 0;



                    // Trips hours
                    var trips = _context.Trips
                        .Where(c => c.DriverId == id && (c.IsTripCompByDriver ?? false))
                        .ToList();
                    var hoursListtrip = trips.Select(c => c.NoOfHoursActual).ToList();
                    var summedHourstrip = hoursListtrip.Sum(hours => hours ?? 0);

                    // Flexis hours
                    var Flexistrips = _context.Flexis
                       .Where(c => c.DriverId == id && (c.IsTripCompByDriver == true))
                       .ToList();
                    var hoursFlexistrips = Flexistrips.Select(c => c.EstimatedHours).ToList();
                    var summedHoursflexi = hoursFlexistrips.Sum(hours => hours ?? 0);

                    //montly hours
                    var Monthlies = _context.Monthlies
                        .Where(c => c.DriverId == id && (c.IsTripCompByDriver == true))
                        .ToList();
                    var hoursMonthlies = Monthlies.Select(c => c.EstimatedHours).ToList();
                    var summedHourshoursMonthlies = hoursMonthlies.Sum(hours => hours = 0);
                    // total sum
                    var summedHours = summedHourstrip + summedHoursflexi + summedHourshoursMonthlies;


                    //trip rating
                    var totalTripsRating = trips.Select(c => c.DriverRating).Sum(rating => rating ?? 0);
                    double averageOfFive = totalTripsRating / 5.0;

                    var data = new
                    {
                        id = driverID,
                        name = driverName,
                        image = driverimage,
                        todaysTripCount = tripsdata,
                        rating = averageOfFive,
                        hoursSpent = summedHours,
                        earnings = Math.Round(tripsearning, 0)
                    };

                    return Ok(data);
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

    }
}
