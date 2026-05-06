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
    public class PostFLexi : ControllerBase
    {
        private readonly DataContext _context;

        public PostFLexi(DataContext context)
        {
            _context = context;
        }



        [HttpPost]
        public async Task<ActionResult<FlexiPostVM>> PostValetparkingStaff(FlexiPostVM flexi)
        {
            try
            {
                var flexipost = new Flexi();
                // Map properties from FlexiPostVM to Flexi entity
                flexipost.PickupLocation = flexi.PickupLocation;
                flexipost.PickUPMapURL = flexi.PickupMapURL;
                flexipost.Coordinates = flexi.Coordinates;
                flexipost.EstimatedHours = flexi.EstimatedHours;
                flexipost.EstimatedPrice = flexi.EstimatedPrice;
                flexipost.VehicleTypeId = flexi.VehicleTypeId;
                flexipost.TransmissionId = flexi.TransmissionId;
                flexipost.PickUpTime = flexi.PickUpTime;
                flexipost.Language = flexi.Language;
                flexipost.UserId = flexi.UserId;
                flexipost.DriverId = flexi.DriverId;
                flexipost.IsDriverAssigned = false;
                flexipost.EstimatedTaxValue = flexi.EstimatedTaxValue;
                flexipost.EstimatedCuponPrice = flexi.EstimatedCuponPrice;
                flexipost.IsAdvancePaid = flexi.IsAdvancePaid;
                flexipost.AdvancePaid = flexi.AdvancePaid;
                flexipost.NoofDays = flexi.NoofDays;
                flexipost.FlexiSelectedDateTime = flexi.FlexiSelectedDate;
                // Add Flexi entity to context and save changes
                _context.Flexis.Add(flexipost);
                await _context.SaveChangesAsync();
                // Generate unique flexi code
                string flexiId = flexipost.FlexiId.ToString().PadLeft(1, '0');
                string currentMonth = DateTime.Today.Month.ToString().PadLeft(2, '0');
                string currentDay = DateTime.Today.Day.ToString().PadLeft(2, '0');
                var flexiCode = $"Go{flexiId}flexi{currentDay}{currentMonth}";
                flexipost.UniqueflexiId = flexiCode;

                // Update Flexi entity with unique code and save changes again
                await _context.SaveChangesAsync();

                // Process FlexiDatesList if provided
                if (flexi.Datelists != null && flexi.Datelists.Count > 0)
                {
                    foreach (var flexidate in flexi.Datelists)
                    {
                        var flexidatelist = new FlexiDatesList();

                        // Combine date and time
                        DateTime? flexxidatetimecombined = null;
                        if (flexidate.Date.HasValue && !string.IsNullOrEmpty(flexidate.Time))
                        {
                            string combinedDateTimeStr = $"{flexidate.Date.Value.ToShortDateString()} {flexidate.Time}";
                            if (DateTime.TryParse(combinedDateTimeStr, out DateTime combinedDateTime))
                            {
                                flexxidatetimecombined = Convert.ToDateTime(combinedDateTimeStr);
                            }
                        }

                        // Convert date to IST (India Standard Time)
                        if (flexidate.Date.HasValue)
                        {
                            TimeZoneInfo istTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                            DateTime istStartDateTime = TimeZoneInfo.ConvertTimeFromUtc(flexidate.Date.Value, istTimeZone);
                            flexidatelist.Date = istStartDateTime;
                        }

                        // Map FlexiDatesList properties
                        flexidatelist.FlexiId = flexipost.FlexiId;
                        flexidatelist.IsTripCompByDriver = flexidate.IsTripCompByDriver;
                        flexidatelist.ActualHours = flexidate.ActualHours;
                        flexidatelist.ActualPrice = flexidate.ActualPrice;
                        flexidatelist.ActualTaxValue = flexidate.ActualTaxValue;
                        flexidatelist.CuponId = flexidate.CuponId;
                        flexidatelist.ActualCuponPrice = flexidate.ActualCuponPrice;
                        flexidatelist.IsTripStarted = flexidate.IsTripStarted;

                        // Add FlexiDatesList entity to context and save changes
                        _context.FlexiDatesLists.Add(flexidatelist);
                        await _context.SaveChangesAsync();
                    }
                }

                // Return successful response with Flexi entity
                return Ok(flexipost);
            }
            catch (Exception ex)
            {
                // Log the full exception details
                Console.WriteLine(ex.ToString());  // Replace with your preferred logging method

                // Return BadRequest with error message
                return BadRequest(ex.Message);
            }
        }

    }
}
