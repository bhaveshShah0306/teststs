using GoChauffeurWebApi.Data;
using GoChauffeurWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest;
using System.Globalization;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminDashboardController : ControllerBase
    {
        private readonly DataContext _context;

        public AdminDashboardController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public dynamic Getdashboarddata()
        {
            try
            {
                var customercount = _context.Users.ToList().Count;
                var drivercount = _context.Drivers.ToList().Count;

                var tripcount = _context.Trips.Where(c => c.IsTripCompByDriver == true).ToList().Count;

                var total = _context.Trips.Where(v => v.IsTripCompByDriver == true).Sum(c => Math.Round(Convert.ToDecimal(c.TotalTripValue), 0));

                var flexitriptotal = _context.Flexis.Where(v => v.CloseTrip == true).Join(_context.FlexiDatesLists, flexi => flexi.FlexiId,flexiDate => flexiDate.FlexiId, (flexi, flexiDate) => new { flexi, flexiDate }).Count();

                var flexitotal = _context.Flexis.Where(v => v.CloseTrip == true).Join(_context.FlexiDatesLists,flexi => flexi.FlexiId, flexiDate => flexiDate.FlexiId,(flexi, flexiDate) => new { flexi, flexiDate }).Sum(x => x.flexiDate.ActualPrice);

                var monthly = _context.Monthlies.Where(v => v.CloseTrip == true).Join(_context.MonthlyDateLists,
                    month => month.MonthlyId,
                    monthdate => monthdate.MonthlyId,(month, monthdate) => new { month, monthdate }).Sum(x => x.monthdate.ActualPrice);

                var monthlytriptotal = _context.Monthlies.Where(v => v.CloseTrip == true).Join(_context.MonthlyDateLists,
                    month => month.MonthlyId,monthdate => monthdate.MonthlyId,(month, monthdate) => new { month, monthdate }).Count();


                var onewaytripcount = _context.Trips.Where(c => c.IsTripCompByDriver == true && c.TripTypeId == 1).ToList().Count;
                var roundtripwaytripcount = _context.Trips.Where(c => c.IsTripCompByDriver == true && c.TripTypeId == 2).ToList().Count;
                var outstationonewaytripcount = _context.Trips.Where(c => c.IsTripCompByDriver == true && c.TripTypeId == 3).ToList().Count;
                var outstationroundtripcount = _context.Trips.Where(c => c.IsTripCompByDriver == true && c.TripTypeId == 4).ToList().Count;
                var totalprice = Convert.ToInt32(total) + Convert.ToInt32(flexitotal) + Convert.ToInt32(monthly);
                var totaltrips = tripcount + flexitriptotal + monthlytriptotal;
                var data = new
                {
                    Totalusers = customercount,
                    Totaldrivers = drivercount,
                    Totalrides = totaltrips,
                    Totalearnings = totalprice,
                    Oneway = onewaytripcount,
                    Roundtrip = roundtripwaytripcount,
                    Outstationoneway = outstationonewaytripcount,
                    Outstationroundtrip = outstationroundtripcount,
                    Flexi = flexitriptotal,
                    Monthly = monthlytriptotal,
                    OnewayPercentage = Math.Round(totaltrips > 0 ? (double)onewaytripcount / totaltrips * 100 : 0, 1),
                    RoundtripPercentage = Math.Round(totaltrips > 0 ? (double)roundtripwaytripcount / totaltrips * 100 : 0, 1),
                    OutstationOnewayPercentage = Math.Round(totaltrips > 0 ? (double)outstationonewaytripcount / totaltrips * 100 : 0, 1),
                    OutstationRoundtripPercentage = Math.Round(totaltrips > 0 ? (double)outstationroundtripcount / totaltrips * 100 : 0, 1),
                    FlexiPercentage = Math.Round(totaltrips > 0 ? (double)flexitriptotal / totaltrips * 100 : 0, 1),
                    MonthlyPercentage = Math.Round(totaltrips > 0 ? (double)monthlytriptotal / totaltrips * 100 : 0, 1),
                };
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        public class HourlyTripCount
        {
            public int Hour { get; set; }
            public int TripCount { get; set; }
        }
        public class WeeklyTripCount
        {
            public int WeekNumber { get; set; }
            public int TripCount { get; set; }
        }
        public class QuarterlyTripCount
        {
            public int QuarterNumber { get; set; }
            public int TripCount { get; set; }
        }
        [HttpGet("analytics")]
        public async Task<ActionResult<IEnumerable<dynamic>>> Analytics(int flag, int type, int year, int month, DateTime date)
        {
            try
            {
                 // flag1= Current day
                // flag 2 = monthly
                // flag 3 = yearlyf
                if (flag == 1)
                {
                    if (type == 1)
                    {
                        var hourlyTripCounts = await _context.Trips
                         .Where(c => c.IsTripCompByDriver == true
                                  && c.StartDateTime.HasValue
                                  && c.StartDateTime.Value.Date == date.Date
                                  && c.TripTypeId == 1)
                         .GroupBy(c => c.StartDateTime.Value.Hour)
                         .Select(g => new HourlyTripCount
                         {
                             Hour = g.Key,
                             TripCount = g.Count()
                         })
                         .ToListAsync();

                        // Create a list of all hours from 0 to 23 (representing 12 AM to 11:59 PM)
                        var allHours = Enumerable.Range(0, 24).ToList();

                        // Join the list of all hours with the hourly trip counts to include all hours in the result
                        var result = allHours
                            .GroupJoin(hourlyTripCounts,
                                       hour => hour,
                                       count => count.Hour,
                                       (hour, counts) => new HourlyTripCount
                                       {
                                           Hour = hour,
                                           TripCount = counts.FirstOrDefault()?.TripCount ?? 0
                                       })
                            .ToList();

                        // Create a JSON object from the result
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                        return Ok(json);
                    }
                    else if (type == 2)
                    {
                        var hourlyTripCounts = await _context.Trips
                        .Where(c => c.IsTripCompByDriver == true
                                 && c.StartDateTime.HasValue
                                 && c.StartDateTime.Value.Date == date.Date
                                 && c.TripTypeId == 2)
                        .GroupBy(c => c.StartDateTime.Value.Hour)
                        .Select(g => new HourlyTripCount
                        {
                            Hour = g.Key,
                            TripCount = g.Count()
                        })
                        .ToListAsync();

                        // Create a list of all hours from 0 to 23 (representing 12 AM to 11:59 PM)
                        var allHours = Enumerable.Range(0, 24).ToList();

                        // Join the list of all hours with the hourly trip counts to include all hours in the result
                        var result = allHours
                            .GroupJoin(hourlyTripCounts,
                                       hour => hour,
                                       count => count.Hour,
                                       (hour, counts) => new HourlyTripCount
                                       {
                                           Hour = hour,
                                           TripCount = counts.FirstOrDefault()?.TripCount ?? 0
                                       })
                            .ToList();

                        // Create a JSON object from the result
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                        return Ok(json);
                    }
                    else if (type == 3)
                    {
                        var hourlyTripCounts = await _context.Trips
                        .Where(c => c.IsTripCompByDriver == true
                                 && c.StartDateTime.HasValue
                                 && c.StartDateTime.Value.Date == date.Date
                                 && c.TripTypeId == 3)
                        .GroupBy(c => c.StartDateTime.Value.Hour)
                        .Select(g => new HourlyTripCount
                        {
                            Hour = g.Key,
                            TripCount = g.Count()
                        })
                        .ToListAsync();

                        // Create a list of all hours from 0 to 23 (representing 12 AM to 11:59 PM)
                        var allHours = Enumerable.Range(0, 24).ToList();

                        // Join the list of all hours with the hourly trip counts to include all hours in the result
                        var result = allHours
                            .GroupJoin(hourlyTripCounts,
                                       hour => hour,
                                       count => count.Hour,
                                       (hour, counts) => new HourlyTripCount
                                       {
                                           Hour = hour,
                                           TripCount = counts.FirstOrDefault()?.TripCount ?? 0
                                       })
                            .ToList();

                        // Create a JSON object from the result
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                        return Ok(json);
                    }
                    else if (type == 4)
                    {
                        var hourlyTripCounts = await _context.Flexis
                        .Where(c => c.CloseTrip == true)
                        .Join(_context.FlexiDatesLists,
                              flexi => flexi.FlexiId, // Join on FlexiId from Flexis table
                              flexiDate => flexiDate.FlexiId, // Join on FlexiId from FlexiDatesLists table
                              (flexi, flexiDate) => new { flexi, flexiDate })
                        .Where(joined => joined.flexiDate.Date.HasValue
                                      && joined.flexiDate.Date.Value.Date == date.Date.Date)
                        .GroupBy(joined => joined.flexiDate.Date.Value.Hour)
                        .Select(g => new HourlyTripCount
                        {
                            Hour = g.Key,
                            TripCount = g.Count()
                        })
                        .ToListAsync();

                        // Create a list of all hours from 0 to 23 (representing 12 AM to 11:59 PM)
                        var allHours = Enumerable.Range(0, 24).ToList();

                        // Join the list of all hours with the hourly trip counts to include all hours in the result
                        var result = allHours
                            .GroupJoin(hourlyTripCounts,
                                       hour => hour,
                                       count => count.Hour,
                                       (hour, counts) => new HourlyTripCount
                                       {
                                           Hour = hour,
                                           TripCount = counts.FirstOrDefault()?.TripCount ?? 0
                                       })
                            .ToList();

                        // Create a JSON object from the result
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                        return Ok(json);
                    }
                    else if (type == 5)
                    {
                        var hourlyTripCounts = await _context.Monthlies
                       .Where(c => c.CloseTrip == true)
                       .Join(_context.MonthlyDateLists,
                             flexi => flexi.MonthlyId, // Join on FlexiId from Flexis table
                             flexiDate => flexiDate.MonthlyId, // Join on FlexiId from FlexiDatesLists table
                             (flexi, flexiDate) => new { flexi, flexiDate })
                       .Where(joined => joined.flexiDate.Date.HasValue
                                     && joined.flexiDate.Date.Value.Date == date.Date)
                       .GroupBy(joined => joined.flexiDate.Date.Value.Hour)
                       .Select(g => new HourlyTripCount
                       {
                           Hour = g.Key,
                           TripCount = g.Count()
                       })
                       .ToListAsync();

                        // Create a list of all hours from 0 to 23 (representing 12 AM to 11:59 PM)
                        var allHours = Enumerable.Range(0, 24).ToList();

                        // Join the list of all hours with the hourly trip counts to include all hours in the result
                        var result = allHours
                            .GroupJoin(hourlyTripCounts,
                                       hour => hour,
                                       count => count.Hour,
                                       (hour, counts) => new HourlyTripCount
                                       {
                                           Hour = hour,
                                           TripCount = counts.FirstOrDefault()?.TripCount ?? 0
                                       })
                            .ToList();

                        // Create a JSON object from the result
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                        return Ok(json);
                    }
                    else if (type == 6)
                    {
                        var hourlyTripCounts = await _context.ValetParkings
                         .Where(c => c.IsAccepted == true
                                  && c.EndDatetime.HasValue
                                  && c.EndDatetime.Value.Date == date.Date)
                         .GroupBy(c => c.EndDatetime.Value.Hour)
                         .Select(g => new HourlyTripCount
                         {
                             Hour = g.Key,
                             TripCount = g.Count()
                         })
                         .ToListAsync();

                        // Create a list of all hours from 0 to 23 (representing 12 AM to 11:59 PM)
                        var allHours = Enumerable.Range(0, 24).ToList();

                        // Join the list of all hours with the hourly trip counts to include all hours in the result
                        var result = allHours
                            .GroupJoin(hourlyTripCounts,
                                       hour => hour,
                                       count => count.Hour,
                                       (hour, counts) => new HourlyTripCount
                                       {
                                           Hour = hour,
                                           TripCount = counts.FirstOrDefault()?.TripCount ?? 0
                                       })
                            .ToList();

                        // Create a JSON object from the result
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                        return Ok(json);
                    }
                    else
                    {
                        return NoContent();
                    }
                }
                else if (flag == 2)
                {
                    if (type == 1)
                    {
                        var currentDate = DateTime.Now;
                        var currentMonth = month;
                        var currentYear = year;

                        // Get the number of days in the current month
                        var daysInMonth = DateTime.DaysInMonth(currentYear, currentMonth);

                        // Generate a list of all days in the current month
                        var allDaysInMonth = Enumerable.Range(1, daysInMonth).ToList();

                        // Group the days into weeks (assuming each week starts on Sunday)
                        var weeksInMonth = allDaysInMonth
                        .Select(day => new
                        {
                            Day = day,
                            WeekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                                new DateTime(currentYear, currentMonth, day), CalendarWeekRule.FirstDay, DayOfWeek.Sunday)
                        })
                        .GroupBy(x => x.WeekNumber)
                        .Select(g => g.Key)
                        .ToList();
                        var trips = await _context.Trips
        .Where(c => c.IsTripCompByDriver == true
                    && c.StartDateTime.HasValue
                    && c.StartDateTime.Value.Month == currentMonth
                    && c.StartDateTime.Value.Year == currentYear
                    && c.TripTypeId == 1)
        .ToListAsync();
                        // Query to get weekly trip counts
                        var weeklyTripCounts = trips
                            .Where(c => c.IsTripCompByDriver == true
                                     && c.StartDateTime.HasValue
                                     && c.StartDateTime.Value.Month == currentMonth
                                     && c.StartDateTime.Value.Year == currentYear
                                     && c.TripTypeId == 1)
                            .GroupBy(c => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                                c.StartDateTime.Value, CalendarWeekRule.FirstDay, DayOfWeek.Sunday))
                            .Select(g => new WeeklyTripCount
                            {
                                WeekNumber = g.Key,
                                TripCount = g.Count()
                            })
                            .ToList();

                        // Join the list of all weeks with the weekly trip counts to include all weeks in the result
                        var result = weeksInMonth
                     .GroupJoin(weeklyTripCounts,
                                week => week,
                                count => count.WeekNumber,
                                (week, counts) => new WeeklyTripCount
                                {
                                    WeekNumber = week,
                                    TripCount = counts.FirstOrDefault()?.TripCount ?? 0
                                })
                     .ToList();
                        // Create a JSON object from the result
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                        return Ok(json);
                    }
                    else if (type == 2)
                    {
                        var currentDate = DateTime.Now;
                        var currentMonth = month;
                        var currentYear = year;

                        // Get the number of days in the current month
                        var daysInMonth = DateTime.DaysInMonth(currentYear, currentMonth);

                        // Generate a list of all days in the current month
                        var allDaysInMonth = Enumerable.Range(1, daysInMonth).ToList();

                        // Group the days into weeks (assuming each week starts on Sunday)
                        var weeksInMonth = allDaysInMonth
                       .Select(day => new
                       {
                           Day = day,
                           WeekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                               new DateTime(currentYear, currentMonth, day), CalendarWeekRule.FirstDay, DayOfWeek.Sunday)
                       })
                       .GroupBy(x => x.WeekNumber)
                       .Select(g => g.Key)
                       .ToList();
                        var trips = await _context.Trips
        .Where(c => c.IsTripCompByDriver == true
                    && c.StartDateTime.HasValue
                    && c.StartDateTime.Value.Month == currentMonth
                    && c.StartDateTime.Value.Year == currentYear
                    && c.TripTypeId == 2)
        .ToListAsync();
                        // Query to get weekly trip counts
                        var weeklyTripCounts = trips
                            .Where(c => c.IsTripCompByDriver == true
                                     && c.StartDateTime.HasValue
                                     && c.StartDateTime.Value.Month == currentMonth
                                     && c.StartDateTime.Value.Year == currentYear
                                     && c.TripTypeId == 2)
                            .GroupBy(c => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                                c.StartDateTime.Value, CalendarWeekRule.FirstDay, DayOfWeek.Sunday))
                            .Select(g => new WeeklyTripCount
                            {
                                WeekNumber = g.Key,
                                TripCount = g.Count()
                            })
                            .ToList();

                        // Join the list of all weeks with the weekly trip counts to include all weeks in the result
                        var result = weeksInMonth
                     .GroupJoin(weeklyTripCounts,
                                week => week,
                                count => count.WeekNumber,
                                (week, counts) => new WeeklyTripCount
                                {
                                    WeekNumber = week,
                                    TripCount = counts.FirstOrDefault()?.TripCount ?? 0
                                })
                     .ToList();

                        // Create a JSON object from the result
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                        return Ok(json);
                    }
                    else if (type == 3)
                    {
                        var currentDate = DateTime.Now;
                        var currentMonth = month;
                        var currentYear = year;

                        // Get the number of days in the current month
                        var daysInMonth = DateTime.DaysInMonth(currentYear, currentMonth);

                        // Generate a list of all days in the current month
                        var allDaysInMonth = Enumerable.Range(1, daysInMonth).ToList();

                        // Group the days into weeks (assuming each week starts on Sunday)
                        var weeksInMonth = allDaysInMonth
                       .Select(day => new
                       {
                           Day = day,
                           WeekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                               new DateTime(currentYear, currentMonth, day), CalendarWeekRule.FirstDay, DayOfWeek.Sunday)
                       })
                       .GroupBy(x => x.WeekNumber)
                       .Select(g => g.Key)
                       .ToList();
                        var trips = await _context.Trips
        .Where(c => c.IsTripCompByDriver == true
                    && c.StartDateTime.HasValue
                    && c.StartDateTime.Value.Month == currentMonth
                    && c.StartDateTime.Value.Year == currentYear
                    && c.TripTypeId == 3)
        .ToListAsync();
                        // Query to get weekly trip counts
                        var weeklyTripCounts = trips
                            .Where(c => c.IsTripCompByDriver == true
                                     && c.StartDateTime.HasValue
                                     && c.StartDateTime.Value.Month == currentMonth
                                     && c.StartDateTime.Value.Year == currentYear
                                     && c.TripTypeId == 3)
                            .GroupBy(c => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                                c.StartDateTime.Value, CalendarWeekRule.FirstDay, DayOfWeek.Sunday))
                            .Select(g => new WeeklyTripCount
                            {
                                WeekNumber = g.Key,
                                TripCount = g.Count()
                            })
                            .ToList();

                        // Join the list of all weeks with the weekly trip counts to include all weeks in the result
                        var result = weeksInMonth
                     .GroupJoin(weeklyTripCounts,
                                week => week,
                                count => count.WeekNumber,
                                (week, counts) => new WeeklyTripCount
                                {
                                    WeekNumber = week,
                                    TripCount = counts.FirstOrDefault()?.TripCount ?? 0
                                })
                     .ToList();

                        // Create a JSON object from the result
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                        return Ok(json);
                    }
                    else if (type == 4)
                    {
                        var currentDate = DateTime.Now;
                        var currentMonth = month;
                        var currentYear = year;

                        // Get the number of days in the current month
                        var daysInMonth = DateTime.DaysInMonth(currentYear, currentMonth);

                        // Generate a list of all days in the current month
                        var allDaysInMonth = Enumerable.Range(1, daysInMonth).ToList();

                        // Group the days into weeks (assuming each week starts on Sunday)
                        var weeksInMonth = allDaysInMonth
                       .Select(day => new
                       {
                           Day = day,
                           WeekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                               new DateTime(currentYear, currentMonth, day), CalendarWeekRule.FirstDay, DayOfWeek.Sunday)
                       })
                       .GroupBy(x => x.WeekNumber)
                       .Select(g => g.Key)
                       .ToList();

                        var flexiData = await _context.Flexis.Where(c => c.CloseTrip == true)
                       .Join(_context.FlexiDatesLists,
                             flexi => flexi.FlexiId,
                             flexiDate => flexiDate.FlexiId,
                             (flexi, flexiDate) => new { Flexi = flexi, FlexiDate = flexiDate })
                       .Where(joined => joined.FlexiDate.Date.HasValue
                                     && joined.FlexiDate.Date.Value.Month == currentMonth
                                     && joined.FlexiDate.Date.Value.Year == currentYear)
                       .ToListAsync();

                        // Group the data in-memory
                        var weeklyTripCounts = flexiData
                           .GroupBy(joined => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                               joined.FlexiDate.Date.Value, CalendarWeekRule.FirstDay, DayOfWeek.Sunday))
                           .Select(g => new WeeklyTripCount
                           {
                               WeekNumber = g.Key,
                               TripCount = g.Count()
                           })
                           .ToList();

                        // Join the list of all weeks with the weekly trip counts to include all weeks in the result
                        var result = weeksInMonth
                        .GroupJoin(weeklyTripCounts,
                                   week => week,
                                   count => count.WeekNumber,
                                   (week, counts) => new WeeklyTripCount
                                   {
                                       WeekNumber = week,
                                       TripCount = counts.FirstOrDefault()?.TripCount ?? 0
                                   })
                        .ToList();

                        // Create a JSON object from the result
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                        return Ok(json);
                    }
                    else if (type == 5)
                    {
                        var currentDate = DateTime.Now;
                        var currentMonth = month;
                        var currentYear = year;

                        // Get the number of days in the current month
                        var daysInMonth = DateTime.DaysInMonth(currentYear, currentMonth);

                        // Generate a list of all days in the current month
                        var allDaysInMonth = Enumerable.Range(1, daysInMonth).ToList();

                        // Group the days into weeks (assuming each week starts on Sunday)
                        var weeksInMonth = allDaysInMonth
                       .Select(day => new
                       {
                           Day = day,
                           WeekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                               new DateTime(currentYear, currentMonth, day), CalendarWeekRule.FirstDay, DayOfWeek.Sunday)
                       })
                       .GroupBy(x => x.WeekNumber)
                       .Select(g => g.Key)
                       .ToList();
                        var monthlydata = await _context.Monthlies.Where(c => c.CloseTrip == true)
                       .Join(_context.MonthlyDateLists,
                             flexi => flexi.MonthlyId,
                             flexiDate => flexiDate.MonthlyId,
                             (flexi, flexiDate) => new { Flexi = flexi, FlexiDate = flexiDate })
                       .Where(joined => joined.FlexiDate.Date.HasValue
                                     && joined.FlexiDate.Date.Value.Month == currentMonth
                                     && joined.FlexiDate.Date.Value.Year == currentYear)
                       .ToListAsync();
                        // Query to get weekly trip counts
                        var weeklyTripCounts = monthlydata
                           .GroupBy(joined => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                               joined.FlexiDate.Date.Value, CalendarWeekRule.FirstDay, DayOfWeek.Sunday))
                           .Select(g => new WeeklyTripCount
                           {
                               WeekNumber = g.Key,
                               TripCount = g.Count()
                           })
                           .ToList();

                        // Join the list of all weeks with the weekly trip counts to include all weeks in the result
                        var result = weeksInMonth
                        .GroupJoin(weeklyTripCounts,
                                   week => week,
                                   count => count.WeekNumber,
                                   (week, counts) => new WeeklyTripCount
                                   {
                                       WeekNumber = week,
                                       TripCount = counts.FirstOrDefault()?.TripCount ?? 0
                                   })
                        .ToList();

                        // Create a JSON object from the result
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                        return Ok(json);
                    }
                    else if (type == 6)
                    {
                        var currentDate = DateTime.Now;
                        var currentMonth = month;
                        var currentYear = year;

                        // Get the number of days in the current month
                        var daysInMonth = DateTime.DaysInMonth(currentYear, currentMonth);

                        // Generate a list of all days in the current month
                        var allDaysInMonth = Enumerable.Range(1, daysInMonth).ToList();

                        // Group the days into weeks (assuming each week starts on Sunday)
                        var weeksInMonth = allDaysInMonth
                            .Select(day => new
                            {
                                Day = day,
                                WeekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                                    new DateTime(currentYear, currentMonth, day), CalendarWeekRule.FirstDay, DayOfWeek.Sunday)
                            })
                            .GroupBy(x => x.WeekNumber)
                            .Select(g => g.Key)
                            .ToList();

                        // Query to get weekly trip counts

                        var valetparking = await _context.ValetParkings
                            .Where(c => c.IsAccepted == true
                                     && c.StartDatetime.HasValue
                                     && c.StartDatetime.Value.Month == currentMonth
                                     && c.StartDatetime.Value.Year == currentYear).ToListAsync();

                        var weeklyTripCounts = valetparking
                            .GroupBy(c => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                                c.StartDatetime.Value, CalendarWeekRule.FirstDay, DayOfWeek.Sunday))
                            .Select(g => new WeeklyTripCount
                            {
                                WeekNumber = g.Key,
                                TripCount = g.Count()
                            })
                            .ToList();

                        // Join the list of all weeks with the weekly trip counts to include all weeks in the result
                        var result = weeksInMonth
                    .GroupJoin(weeklyTripCounts,
                               week => week,
                               count => count.WeekNumber,
                               (week, counts) => new WeeklyTripCount
                               {
                                   WeekNumber = week,
                                   TripCount = counts.FirstOrDefault()?.TripCount ?? 0
                               })
                    .ToList();
                        // Create a JSON object from the result
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                        return Ok(json);
                    }
                    else
                    {
                        return NoContent();
                    }
                }
                else if (flag == 3)
                {
                    if (type == 1)
                    {
                        var yearlyTripCounts = await _context.Trips
                         .Where(c => c.IsTripCompByDriver == true
                                  && c.StartDateTime.HasValue
                                  && c.StartDateTime.Value.Year == year
                                  && c.TripTypeId == 1)
                         .GroupBy(c => (c.StartDateTime.Value.Month - 1) / 3 + 1)
                         .Select(g => new QuarterlyTripCount
                         {
                             QuarterNumber = g.Key,
                             TripCount = g.Count()
                         })
                         .ToListAsync();

                        // Create a list of all quarters (Q1, Q2, Q3, Q4)
                        var allQuarters = Enumerable.Range(1, 4).ToList();

                        // Join the list of all quarters with the yearly trip counts to include all quarters in the result
                        var result = allQuarters
                            .GroupJoin(yearlyTripCounts,
                                       quarter => quarter,
                                       count => count.QuarterNumber,
                                       (quarter, counts) => new QuarterlyTripCount
                                       {
                                           QuarterNumber = quarter,
                                           TripCount = counts.FirstOrDefault()?.TripCount ?? 0
                                       })
                            .ToList();

                        // Create a JSON object from the result
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                        return Ok(json);
                    }
                    else if (type == 2)
                    {
                        var yearlyTripCounts = await _context.Trips
                        .Where(c => c.IsTripCompByDriver == true
                                 && c.StartDateTime.HasValue
                                 && c.StartDateTime.Value.Year == year
                                 && c.TripTypeId == 2)
                        .GroupBy(c => (c.StartDateTime.Value.Month - 1) / 3 + 1)
                        .Select(g => new QuarterlyTripCount
                        {
                            QuarterNumber = g.Key,
                            TripCount = g.Count()
                        })
                        .ToListAsync();

                        // Create a list of all quarters (Q1, Q2, Q3, Q4)
                        var allQuarters = Enumerable.Range(1, 4).ToList();

                        // Join the list of all quarters with the yearly trip counts to include all quarters in the result
                        var result = allQuarters
                            .GroupJoin(yearlyTripCounts,
                                       quarter => quarter,
                                       count => count.QuarterNumber,
                                       (quarter, counts) => new QuarterlyTripCount
                                       {
                                           QuarterNumber = quarter,
                                           TripCount = counts.FirstOrDefault()?.TripCount ?? 0
                                       })
                            .ToList();

                        // Create a JSON object from the result
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                        return Ok(json);
                    }
                    else if (type == 3)
                    {

                        var yearlyTripCounts = await _context.Trips
                        .Where(c => c.IsTripCompByDriver == true
                                 && c.StartDateTime.HasValue
                                 && c.StartDateTime.Value.Year == year
                                 && c.TripTypeId == 3)
                        .GroupBy(c => (c.StartDateTime.Value.Month - 1) / 3 + 1)
                        .Select(g => new QuarterlyTripCount
                        {
                            QuarterNumber = g.Key,
                            TripCount = g.Count()
                        })
                        .ToListAsync();

                        // Create a list of all quarters (Q1, Q2, Q3, Q4)
                        var allQuarters = Enumerable.Range(1, 4).ToList();

                        // Join the list of all quarters with the yearly trip counts to include all quarters in the result
                        var result = allQuarters
                            .GroupJoin(yearlyTripCounts,
                                       quarter => quarter,
                                       count => count.QuarterNumber,
                                       (quarter, counts) => new QuarterlyTripCount
                                       {
                                           QuarterNumber = quarter,
                                           TripCount = counts.FirstOrDefault()?.TripCount ?? 0
                                       })
                            .ToList();

                        // Create a JSON object from the result
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                        return Ok(json);
                    }
                    else if (type == 4)
                    {
                        var flexiData = await _context.Flexis.Where(c => c.CloseTrip == true)
                      .Join(_context.FlexiDatesLists,
                            flexi => flexi.FlexiId,
                            flexiDate => flexiDate.FlexiId,
                            (flexi, flexiDate) => new { Flexi = flexi, FlexiDate = flexiDate })
                      .Where(joined => joined.FlexiDate.Date.HasValue
                                    && joined.FlexiDate.Date.Value.Year == year)
                      .ToListAsync();
                        var yearlyTripCounts = flexiData
                        .GroupBy(c => (c.FlexiDate.Date.Value.Month - 1) / 3 + 1)
                        .Select(g => new QuarterlyTripCount
                        {
                            QuarterNumber = g.Key,
                            TripCount = g.Count()
                        })
                        .ToList();

                        // Create a list of all quarters (Q1, Q2, Q3, Q4)
                        var allQuarters = Enumerable.Range(1, 4).ToList();

                        // Join the list of all quarters with the yearly trip counts to include all quarters in the result
                        var result = allQuarters
                            .GroupJoin(yearlyTripCounts,
                                       quarter => quarter,
                                       count => count.QuarterNumber,
                                       (quarter, counts) => new QuarterlyTripCount
                                       {
                                           QuarterNumber = quarter,
                                           TripCount = counts.FirstOrDefault()?.TripCount ?? 0
                                       })
                            .ToList();

                        // Create a JSON object from the result
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                        return Ok(json);
                    }
                    else if (type == 5)
                    {
                        var monthlyData = await _context.Monthlies.Where(c => c.CloseTrip == true)
                      .Join(_context.MonthlyDateLists,
                            flexi => flexi.MonthlyId,
                            flexiDate => flexiDate.MonthlyId,
                            (flexi, flexiDate) => new { Flexi = flexi, FlexiDate = flexiDate })
                      .Where(joined => joined.FlexiDate.Date.HasValue
                                    && joined.FlexiDate.Date.Value.Year == year)
                      .ToListAsync();


                        var yearlyTripCounts = monthlyData
                        .GroupBy(c => (c.FlexiDate.Date.Value.Month - 1) / 3 + 1)
                        .Select(g => new QuarterlyTripCount
                        {
                            QuarterNumber = g.Key,
                            TripCount = g.Count()
                        })
                        .ToList();

                        // Create a list of all quarters (Q1, Q2, Q3, Q4)
                        var allQuarters = Enumerable.Range(1, 4).ToList();

                        // Join the list of all quarters with the yearly trip counts to include all quarters in the result
                        var result = allQuarters
                            .GroupJoin(yearlyTripCounts,
                                       quarter => quarter,
                                       count => count.QuarterNumber,
                                       (quarter, counts) => new QuarterlyTripCount
                                       {
                                           QuarterNumber = quarter,
                                           TripCount = counts.FirstOrDefault()?.TripCount ?? 0
                                       })
                            .ToList();

                        // Create a JSON object from the result
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                        return Ok(json);
                    }
                    else if (type == 6)
                    {
                        var yearlyTripCounts = await _context.ValetParkings
                        .Where(c => c.IsAccepted == true
                                 && c.StartDatetime.HasValue
                                 && c.StartDatetime.Value.Year == year)
                        .GroupBy(c => (c.StartDatetime.Value.Month - 1) / 3 + 1)
                        .Select(g => new QuarterlyTripCount
                        {
                            QuarterNumber = g.Key,
                            TripCount = g.Count()
                        })
                        .ToListAsync();

                        // Create a list of all quarters (Q1, Q2, Q3, Q4)
                        var allQuarters = Enumerable.Range(1, 4).ToList();

                        // Join the list of all quarters with the yearly trip counts to include all quarters in the result
                        var result = allQuarters
                            .GroupJoin(yearlyTripCounts,
                                       quarter => quarter,
                                       count => count.QuarterNumber,
                                       (quarter, counts) => new QuarterlyTripCount
                                       {
                                           QuarterNumber = quarter,
                                           TripCount = counts.FirstOrDefault()?.TripCount ?? 0
                                       })
                            .ToList();

                        // Create a JSON object from the result
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                        return Ok(json);
                    }
                    else
                    {
                        return NoContent();
                    }
                }
                else
                {
                    return NoContent();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
