
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
    public class PostMonthly : ControllerBase
    {
        private readonly DataContext _context;

        public PostMonthly(DataContext context)
        {
            _context = context;
        }


        [HttpPost]
        public async Task<ActionResult<MonthlyVM>> PostValetparkingStaff(MonthlyVM data)
        {
            try
            {
                var month = new Monthly();
                month.TripVarientId = data.TripVarientId;
                month.NoofDays = data.NoofDays;
                month.PickUpLocationCoordinates = data.PickUpLocationCoordinates;
                month.PickupMapURL = data.PickupMapURL;
                month.NoofDays = data.NoofDays;
                month.Pickuptime = data.Pickuptime;
                month.VehicleTypeId = data.VehicleTypeId;
                month.TransmissionId = data.TransmissionId;
                month.EstimatedHours = data.EstimatedHours;
                month.Estimatedprice = data.Estimatedprice;
                month.UserId = data.UserId;
                month.PickUpLocation = data.PickUpLocation;
                month.Estimatedprice = data.Estimatedprice;
                month.IsAdvancedPayment = data.IsAdvancedPayment;
                month.IsPermanent = data.IsPermanent;
                month.SelectedDate = data.SelectedDate;

                await _context.Monthlies.AddAsync(month);
                await _context.SaveChangesAsync();
                // Get the offerId as a string and pad it with leading zeros if needed
                string MonthlyId = month.MonthlyId.ToString().PadLeft(1, '0');

                // Get current month and day as strings
                string currentMonth = DateTime.Today.Month.ToString().PadLeft(2, '0');
                string currentDay = DateTime.Today.Day.ToString().PadLeft(2, '0');

                // Construct the offer code following the pattern:
                // 3 chars from OfferName, '0', OfferId, '0', currentMonth, currentDay
                var monthlyCode = $"Go{MonthlyId}Monthly{currentDay}{currentMonth}";
                month.UniqueMonthlyId = monthlyCode;
                _context.Entry(month).State = EntityState.Modified;
                await _context.SaveChangesAsync();



                if (data.DateList!= null && data.DateList.Count > 0)
                {
                    foreach(var date in data.DateList)
                    {
                        var monthdate = new MonthlyDateList();

                        DateTime? flexidatetocombine = date.Date;

                        string? flexitimetocombine = date.Time;

                        DateTime? flexxidatetimecombined = null;
                        if (date.Date.HasValue && !string.IsNullOrEmpty(date.Time))
                        {
                            string combinedDateTimeStr = $"{date.Date.Value.ToShortDateString()} {date.Time}";
                            if (DateTime.TryParse(combinedDateTimeStr, out DateTime combinedDateTime))
                            {
                                flexxidatetimecombined = combinedDateTime;
                            }
                        }

                        // Convert date to IST (India Standard Time)
                        if (date.Date.HasValue)
                        {
                            TimeZoneInfo istTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                            DateTime istStartDateTime = TimeZoneInfo.ConvertTimeFromUtc(date.Date.Value, istTimeZone);
                            monthdate.Date = istStartDateTime;
                        }

                        //monthdate.Date = flexxidatetimecombined;

                        monthdate.MonthlyId = month.MonthlyId ;

                        monthdate.IsTripCompByDriver = date.IsTripCompByDriver;
                        
                        monthdate.ActualHours = date.ActualHours;
                        
                        monthdate.ActualPrice = date.ActualPrice;
                        
                        monthdate.ActualTaxValue = date.ActualTaxValue;
                        
                        monthdate.CuponId = date.CuponId;
                        
                        monthdate.ActualCuponPrice = date.ActualCuponPrice;
                        
                        monthdate.IsTripStarted = date.IsTripStarted;
                        await _context.MonthlyDateLists.AddAsync(monthdate);
                        await _context.SaveChangesAsync();


                    }

                }
                return Ok(month);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
