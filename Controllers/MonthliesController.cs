using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GoChauffeurWebApi.Data;
using GoChauffeurWebApi.Models;
using GoChauffeurWebApi.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonthliesController : ControllerBase
    {
        private readonly DataContext _context;

        public MonthliesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Monthlies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MonthlyVM>>> GetMonthlies()
        {
            try
            {
                var monthlydatalist = await _context.Monthlies.ToListAsync();
                var monthlyVMList = new List<MonthlyVM>();
                if (monthlydatalist.Count > 0)
                {
                    foreach (var monthly in monthlydatalist)
                    {
                        var monthlyVM = new MonthlyVM();
                        monthlyVM.MonthlyId = monthly.MonthlyId;
                        monthlyVM.UniqueMonthlyId = monthly.UniqueMonthlyId;
                        monthlyVM.TripVarientId = monthly.TripVarientId;
                        monthlyVM.PickupMapURL = monthly.PickupMapURL;
                        monthlyVM.IsdriverArrived = monthly.IsdriverArrived;
                        monthlyVM.istripcompleted = monthly.IsTripCompByDriver;
                        monthlyVM.IsTripStarted = monthly.IsTripStarted;
                        monthlyVM.IsProcessing = monthly.IsProcessing;
                        monthlyVM.IsReserved = monthly.IsCancelled;
                        monthlyVM.IsAccepted = monthly.IsAccepted;
                        monthlyVM.FinalPrice = monthly.FinalPrice;

                        var driverdata = _context.Drivers.Find(monthlyVM.DriverId);
                        if (driverdata != null)
                        {
                            monthlyVM.driverPhoneNumber = driverdata.PhoneNumber;
                            monthlyVM.DriverName = driverdata.DriverName;
                        }
                        monthlyVM.UsersTripsCancelResonsId = monthly.UsersTripsCancelResonsId;
                        var UsersTripsCancelResons = await _context.UsersTripsCancelResons.FindAsync(monthlyVM.UsersTripsCancelResonsId);
                        if (UsersTripsCancelResons != null)
                        {
                            monthlyVM.UserTripsCancelResonsName = UsersTripsCancelResons.UserTripsCancelResonsName;
                        }

                        monthlyVM.PickUpLocation = monthly.PickUpLocation;

                        monthlyVM.PickUpLocationCoordinates = monthly.PickUpLocationCoordinates;

                        monthlyVM.DriverMeansOfTransport = monthly.DriverMeansOfTransport;

                        monthlyVM.NoofDays = monthly.NoofDays;

                        monthlyVM.Pickuptime = monthly.Pickuptime;

                        monthlyVM.VehicleTypeId = monthly.VehicleTypeId;

                        var vehicledata = await _context.VehicleTypes.FindAsync(monthlyVM.VehicleTypeId);
                        if (vehicledata != null)
                        {
                            monthlyVM.VehicleTypeName = vehicledata.VehicleTypeName;
                            monthlyVM.VehicleTypeImage = vehicledata.Icon;
                        }

                        monthlyVM.TransmissionId = monthly.TransmissionId;

                        var transmissiondata = await _context.TransmissionTypes.FindAsync(monthlyVM.TransmissionId);
                        if (transmissiondata != null)
                        {
                            monthlyVM.TransmissinName = transmissiondata.TransmissionName;
                        }

                        monthlyVM.UserId = monthly.UserId;
                        var userdata = _context.Users.Find(monthly.UserId);
                        if (userdata != null)
                        {
                            monthlyVM.Name = userdata.Name;
                            monthlyVM.PhoneNumber = userdata.PhoneNumber;


                        }

                        monthlyVM.EstimatedHours = monthly.EstimatedHours;

                        monthlyVM.Estimatedprice = monthly.Estimatedprice;

                        monthlyVM.IsAdvancedPayment = monthly.IsAdvancedPayment;

                        var datelistVM = new List<MonthlyDateListVM>();

                        var datelists = await _context.MonthlyDateLists.Where(c => c.MonthlyId == monthlyVM.MonthlyId).ToListAsync();
                        if (datelists.Count > 0)
                        {
                            foreach (var datelist in datelists)
                            {
                                var dateVM = new MonthlyDateListVM();

                                dateVM.MonthlyDateListId = datelist.MonthlyDateListId;

                                dateVM.Date = datelist.Date;

                                dateVM.MonthlyId = datelist.MonthlyId;

                                dateVM.IsTripCompByDriver = datelist.IsTripCompByDriver;

                                dateVM.ActualHours = datelist.ActualHours;

                                dateVM.ActualPrice = datelist.ActualPrice;

                                dateVM.ActualTaxValue = datelist.ActualTaxValue;

                                dateVM.CuponId = datelist.CuponId;

                                dateVM.ActualCuponPrice = datelist.ActualCuponPrice;

                                dateVM.IsTripStarted = datelist.IsTripStarted;

                                datelistVM.Add(dateVM);
                            }
                        }
                        monthlyVM.DateList = datelistVM;



                        monthlyVMList.Add(monthlyVM);
                    }
                    return Ok(monthlyVMList);
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

        // GET: api/Monthlies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Monthly>> GetMonthly(int id)
        {
            try
            {
                var monthly = await _context.Monthlies.FindAsync(id);
                var monthlyVM = new MonthlyVM();
                if (monthly != null)
                {
                    monthlyVM.MonthlyId = monthly.MonthlyId;
                    monthlyVM.UniqueMonthlyId = monthly.UniqueMonthlyId;
                    monthlyVM.TripVarientId = monthly.TripVarientId;
                    monthlyVM.PickupMapURL = monthly.PickupMapURL;
                    monthlyVM.IsdriverArrived = monthly.IsdriverArrived;
                    monthlyVM.istripcompleted = monthly.IsTripCompByDriver;
                    monthlyVM.IsTripStarted = monthly.IsTripStarted;
                    monthlyVM.IsProcessing = monthly.IsProcessing;
                    monthlyVM.IsAccepted = monthly.IsAccepted;
                    monthlyVM.IsCancelled = monthly.IsCancelled;
                    monthlyVM.Isonroute = monthly.Isonroute;
                    monthlyVM.IsdriverArrived = monthly.IsdriverArrived;
                    monthlyVM.IsAccepted = monthly.IsAccepted;
                    monthlyVM.FinalPrice = monthly.FinalPrice;
                    monthlyVM.DriverId = monthly.DriverId;
                    var driverdata = _context.Drivers.Find(monthlyVM.DriverId);
                    if (driverdata != null)
                    {
                        monthlyVM.driverPhoneNumber = driverdata.PhoneNumber;
                        monthlyVM.DriverName = driverdata.DriverName;
                    }
                    monthlyVM.UsersTripsCancelResonsId = monthly.UsersTripsCancelResonsId;
                    var UsersTripsCancelResons = await _context.UsersTripsCancelResons.FindAsync(monthlyVM.UsersTripsCancelResonsId);
                    if (UsersTripsCancelResons != null)
                    {
                        monthlyVM.UserTripsCancelResonsName = UsersTripsCancelResons.UserTripsCancelResonsName;
                    }
                    var tripvariantdata = await _context.TripVariants.FindAsync(monthlyVM.TripVarientId);
                    if (tripvariantdata != null)
                    {
                        monthlyVM.TripVarientName = tripvariantdata.TripVariantName;
                    }

                    monthlyVM.PickUpLocation = monthly.PickUpLocation;

                    monthlyVM.PickUpLocationCoordinates = monthly.PickUpLocationCoordinates;

                    monthlyVM.NoofDays = monthly.NoofDays;

                    monthlyVM.Pickuptime = monthly.Pickuptime;

                    monthlyVM.VehicleTypeId = monthly.VehicleTypeId;
                    var anonymusdata = _context.Anonymoustripcharges.Where(c => c.TriptypeId == 4).FirstOrDefault();
                    if (anonymusdata != null)
                    {
                        monthlyVM.Anonymuscharge = anonymusdata.Amount.ToString();
                    }
                    var vehicledata = await _context.VehicleTypes.FindAsync(monthlyVM.VehicleTypeId);
                    if (vehicledata != null)
                    {
                        monthlyVM.VehicleTypeName = vehicledata.VehicleTypeName;
                        monthlyVM.VehicleTypeImage = vehicledata.Icon;
                    }

                    monthlyVM.TransmissionId = monthly.TransmissionId;

                    var transmissiondata = await _context.TransmissionTypes.FindAsync(monthlyVM.TransmissionId);
                    if (transmissiondata != null)
                    {
                        monthlyVM.TransmissinName = transmissiondata.TransmissionName;
                    }

                    monthlyVM.UserId = monthly.UserId;
                    var userdata = _context.Users.Find(monthly.UserId);
                    if (userdata != null)
                    {
                        monthlyVM.Name = userdata.Name;
                        monthlyVM.PhoneNumber = userdata.PhoneNumber;
                    }

                    monthlyVM.EstimatedHours = monthly.EstimatedHours;

                    monthlyVM.Estimatedprice = monthly.Estimatedprice;
                    monthlyVM.FinalPrice = monthly.FinalPrice;
                    
                    monthlyVM.IsAdvancedPayment = monthly.IsAdvancedPayment;
                    var price = 0;
                    var datelistVM = new List<MonthlyDateListVM>();

                    var datelists = await _context.MonthlyDateLists.Where(c => c.MonthlyId == monthlyVM.MonthlyId).ToListAsync();
                    if (datelists.Count > 0)
                    {
                        
                        foreach (var datelist in datelists)
                        {
                            var dateVM = new MonthlyDateListVM();

                            dateVM.MonthlyDateListId = datelist.MonthlyDateListId;

                            dateVM.Date = datelist.Date;

                            dateVM.MonthlyId = datelist.MonthlyId;

                            dateVM.IsTripCompByDriver = datelist.IsTripCompByDriver;

                            dateVM.ActualHours = datelist.ActualHours;

                            dateVM.ActualPrice = datelist.ActualPrice;

                            dateVM.ActualTaxValue = datelist.ActualTaxValue;

                            dateVM.CuponId = datelist.CuponId;

                            dateVM.ActualCuponPrice = datelist.ActualCuponPrice;

                            dateVM.IsTripStarted = datelist.IsTripStarted;
                            dateVM.selecteddateListvalue = datelist.selecteddateListvalue;
                            dateVM.Isonroute = datelist.Isonroute;
                            if (datelist.IsTripCompByDriver != false)
                            {
                                price += Convert.ToInt32(dateVM.ActualPrice);
                            }
                            datelistVM.Add(dateVM);
                        }
                    }
                    monthlyVM.FinalPrice = price;
                    monthlyVM.DateList = datelistVM;
                    if (monthlyVM.FinalPrice < Convert.ToDecimal(monthlyVM.Estimatedprice))
                    {
                        monthlyVM.FinalPrice = Convert.ToDecimal(monthlyVM.Estimatedprice);
                    }
                    var taxdata = await _context.Taxes.FirstOrDefaultAsync();
                    if (taxdata != null)
                    {
                        var taxvalue = await _context.Taxes.FirstOrDefaultAsync();
                        if (taxvalue != null)
                        {
                            var percentage = taxvalue.Percentage;
                            var taxvalues = monthlyVM.FinalPrice * percentage / 100;
                            monthlyVM.Taxvalue = taxvalues.ToString();
                        }

                    }
                    return Ok(monthlyVM);
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


        // GET: api/Monthlies
        [HttpGet("{driverId}/Monthliesdata")]
        public async Task<ActionResult<IEnumerable<MonthlyVM>>> GetMonthliesDriverData(int driverId)
            {
            try
            {
                var driver = await _context.Drivers.Where(c => c.DriverId == driverId && c.IsDriverActive == true).FirstOrDefaultAsync();
                if (driver == null) return NotFound("Driver not found");

                var ignoreTrips = await _context.IgnoreMontlyTrips.Where(c => c.DriverId == driverId).ToListAsync();
                var distanceToGetTripsData = await _context.Driversurroundingtrips.FirstOrDefaultAsync();
                int distancetocover = distanceToGetTripsData?.Distance ?? 30;

                if (string.IsNullOrEmpty(driver.TransmissionTypeId) || string.IsNullOrEmpty(driver.VehicleTypeIds))
                {
                    return BadRequest("Please contact admin to update vehicle types");
                }

                var transmissionTypes = driver.TransmissionTypeId.Split(',').Select(int.Parse).ToList();
                var vehicleTypes = driver.VehicleTypeIds.Split(',').Select(int.Parse).ToList();

                var flexiData = await _context.Monthlies
                    .Where(c => c.DriverId == 0 || c.DriverId == null || c.DriverId == driverId)
                    .ToListAsync();

                var monthlyDataList = flexiData
                    .Where(c => transmissionTypes.Contains(Convert.ToInt32(c.TransmissionId))
                                && vehicleTypes.Contains(Convert.ToInt32(c.VehicleTypeId)))
                    .ToList();
                var driverwallet = _context.Driverwallets.Where(c => c.DriverId == driverId).FirstOrDefault();
                var minimumwalletbalance = 500;
                var tripminimumvalue = _context.Withdrawamountvalue.FirstOrDefault();
                if (tripminimumvalue != null)
                {
                    minimumwalletbalance = Convert.ToInt32(tripminimumvalue.MinimumAmount);
                }
                
                if (driverwallet!=null&&driverwallet.WalletBalance == "")
                {
                    driverwallet.WalletBalance=0.ToString();
                }

                if (driverwallet != null && Convert.ToInt32(driverwallet.WalletBalance) > -100)
                {

                        if (!monthlyDataList.Any()) return NoContent();

                    var monthlyVMList = new List<MonthlyVM>();

                    foreach (var monthly in monthlyDataList)
                    {
                        var ignoredTrip = ignoreTrips.FirstOrDefault(c => c.MonthlyId == monthly.MonthlyId);
                        if (ignoredTrip != null) continue;

                        var locationTrip = monthly.PickUpLocationCoordinates.Split(',').ToList();
                        var driverDistance = distance(
                            Convert.ToDouble(locationTrip[0]),
                            Convert.ToDouble(locationTrip[1]),
                            Convert.ToDouble(driver.Latitude),
                            Convert.ToDouble(driver.Longitude),
                            'K');

                        if (driverDistance >= distancetocover) continue;

                        var monthlyVM = new MonthlyVM
                        {
                            MonthlyId = monthly.MonthlyId,
                            UniqueMonthlyId = monthly.UniqueMonthlyId,
                            TripVarientId = monthly.TripVarientId,
                            UsersTripsCancelResonsId = monthly.UsersTripsCancelResonsId,
                            PickUpLocation = monthly.PickUpLocation,
                            PickUpLocationCoordinates = monthly.PickUpLocationCoordinates,
                            DriverMeansOfTransport = monthly.DriverMeansOfTransport,
                            NoofDays = monthly.NoofDays,
                            Pickuptime = monthly.Pickuptime,
                            VehicleTypeId = monthly.VehicleTypeId,
                            TransmissionId = monthly.TransmissionId,
                            UserId = monthly.UserId,
                            EstimatedHours = monthly.EstimatedHours,
                            Estimatedprice = monthly.Estimatedprice,
                            IsAdvancedPayment = monthly.IsAdvancedPayment,
                            IsAccepted = monthly.IsAccepted,
                            FinalPrice = monthly.FinalPrice,
                            IsReserved = monthly.IsReserved,
                            IsTimeScheduled = monthly.IsTimeScheduled,
                            DateList = await _context.MonthlyDateLists
                                .Where(c => c.MonthlyId == monthly.MonthlyId)
                                .Select(datelist => new MonthlyDateListVM
                                {
                                    MonthlyDateListId = datelist.MonthlyDateListId,
                                    Date = datelist.Date,
                                    MonthlyId = datelist.MonthlyId,
                                    IsTripCompByDriver = datelist.IsTripCompByDriver,
                                    ActualHours = datelist.ActualHours,
                                    ActualPrice = datelist.ActualPrice,
                                    ActualTaxValue = datelist.ActualTaxValue,
                                    CuponId = datelist.CuponId,
                                    ActualCuponPrice = datelist.ActualCuponPrice,
                                    IsTripStarted = datelist.IsTripStarted,
                                    selecteddateListvalue = datelist.selecteddateListvalue,
                             
                                    Isonroute = datelist.Isonroute,
                                }).ToListAsync()
                        };

                        var userTripsCancelReasons = await _context.UsersTripsCancelResons.FindAsync(monthlyVM.UsersTripsCancelResonsId);
                        if (userTripsCancelReasons != null)
                        {
                            monthlyVM.UserTripsCancelResonsName = userTripsCancelReasons.UserTripsCancelResonsName;
                        }

                        var tripVariantData = await _context.TripVariants.FindAsync(monthlyVM.TripVarientId);
                        if (tripVariantData != null)
                        {
                            monthlyVM.TripVarientName = tripVariantData.TripVariantName;
                        }

                        var vehicleData = await _context.VehicleTypes.FindAsync(monthlyVM.VehicleTypeId);
                        if (vehicleData != null)
                        {
                            monthlyVM.VehicleTypeName = vehicleData.VehicleTypeName;
                            monthlyVM.VehicleTypeImage = vehicleData.Icon;
                        }

                        var transmissionData = await _context.TransmissionTypes.FindAsync(monthlyVM.TransmissionId);
                        if (transmissionData != null)
                        {
                            monthlyVM.TransmissinName = transmissionData.TransmissionName;
                        }
                        var anonymuscharges = await _context.Anonymoustripcharges.Where(c => c.TriptypeId == 4).FirstOrDefaultAsync();
                        var driversubdata = await _context.Driversubscriptions
                                     .Where(c => c.DriverId == driverId && c.Expirydate <= DateTime.Now)
                                     .OrderByDescending(c => c.DriversubscriptionId) // Sort by the latest date first
                                     .FirstOrDefaultAsync();
                        var taxdata = _context.Taxes.FirstOrDefault();
                        var taxpercentage = 0;
                        if (taxdata != null)
                        {
                            taxpercentage = Convert.ToInt32(taxdata.Percentage);
                        }
                        if (driversubdata != null)
                        {
                            var subdata = _context.Subscriptions.Find(driversubdata.SubscriptionId);
                            if (subdata != null && subdata.Percentage != null && subdata.Percentage != 0)
                            {
                                var percetage = subdata.Percentage;
                                int taxvalue = Convert.ToInt32(monthlyVM.Estimatedprice * taxpercentage / 100);


                                if (anonymuscharges != null && anonymuscharges.Amount.HasValue)
                                {
                                    monthlyVM.DriverPrice = Convert.ToInt32(monthlyVM.Estimatedprice - taxvalue - Convert.ToInt32(anonymuscharges.Amount.Value) * percetage / 100);
                                }
                                else
                                {
                                    // Handle the case when anonymuscharges or Amount is null
                                    monthlyVM.DriverPrice = Convert.ToInt32(monthlyVM.Estimatedprice - taxvalue * percetage / 100);
                                }
                            }
                            else
                            {
                                var taxvalue = monthlyVM.Estimatedprice * taxpercentage / 100;


                                if (anonymuscharges != null && anonymuscharges.Amount.HasValue)
                                {
                                    monthlyVM.DriverPrice = Convert.ToInt32(Convert.ToInt32(monthlyVM.Estimatedprice )- taxvalue - Convert.ToInt32(anonymuscharges.Amount.Value));
                                }
                                else
                                {
                                    // Handle the case when anonymuscharges or Amount is null
                                    monthlyVM.DriverPrice = Convert.ToInt32(Convert.ToInt32(monthlyVM.Estimatedprice) - taxvalue);
                                }
                            }



                        }
                        else
                        {
                            var percetage = 80;
                            var taxvalue = monthlyVM.Estimatedprice * taxpercentage / 100;


                            // Handle the case when anonymuscharges or Amount is null
                            monthlyVM.DriverPrice = Convert.ToInt32(monthlyVM.Estimatedprice - taxvalue * percetage / 100);

                        }
                        var userData = await _context.Users.FindAsync(monthly.UserId);
                        if (userData != null)
                        {
                            monthlyVM.Name = userData.Name;
                            monthlyVM.PhoneNumber = userData.PhoneNumber;
                        }
                   
                            var dataconfiguretime = _context.Schuduletriptimechagemodel.Where(c => c.TripTypeId == 5).FirstOrDefault();
                            if (dataconfiguretime != null)
                            {
                                TimeZoneInfo istTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                                DateTime istStartDateTime = TimeZoneInfo.ConvertTimeFromUtc(monthly.Pickuptime.Value, istTimeZone);
                                DateTime currentIstTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, istTimeZone);

                                DateTime currentIstTimePlus45Minutes = currentIstTime.AddMinutes(Convert.ToInt32(dataconfiguretime.SchuduletripDuration));
                                if (monthly.IsReserved == true)
                                {

                                DateTime? startTimes = monthlyVM.Pickuptime; // Nullable DateTime for the time part
                                DateTime? startDateNullable = monthlyVM.DateList[0].Date; // Nullable DateTime for the date part

                                if (startDateNullable.HasValue)
                                {
                                    DateTime startDate = startDateNullable.Value; // Get the date part

                                    if (startTimes.HasValue)
                                    {
                                        // Extract the time part from startTimes
                                        TimeSpan startTimeOfDay = startTimes.Value.TimeOfDay;

                                        // Combine the date from startDate and the time from startTimes into a single DateTime object
                                        DateTime combinedDateTime = startDate.Date.Add(startTimeOfDay);

                                        // Assign the combined DateTime to flexiVm.PickUpTime
                                        monthlyVM.Pickuptime = combinedDateTime;

                                        // Format the combined DateTime to a string if needed
                                        var starttimesFormatted = combinedDateTime.ToString("h:mm tt");
                                    }
                                }


                                // Convert StartTime string to DateTime
                                DateTime startTimeDateTime = Convert.ToDateTime(monthlyVM.Pickuptime);

                                    // Convert the duration to an integer
                                    int duration = Convert.ToInt32(dataconfiguretime.SchuduletripDuration);

                                    // Calculate the time before the duration
                                    DateTime beforeDurationTime = monthlyVM.Pickuptime.HasValue
    ? monthlyVM.Pickuptime.Value.AddMinutes(-duration)
    : default(DateTime);
                                // Define AcceptTimeTo as beforeDurationTime
                                DateTime AcceptTimeTo = beforeDurationTime;

                         
                                    // Define AcceptTimeFrom as the beginning of the hour of beforeDurationTime
                                    DateTime AcceptTimeFrom = new DateTime(beforeDurationTime.Year, beforeDurationTime.Month, beforeDurationTime.Day, beforeDurationTime.Hour, 0, 0);

                                    // Output the results (if needed for debugging)
                                    Console.WriteLine($"AcceptTimeFrom: {AcceptTimeFrom}");
                                    Console.WriteLine($"AcceptTimeTo: {AcceptTimeTo}");

                                    // You can assign these values to the appropriate properties in your view model if needed
                                    monthlyVM.Accepttimefrom = AcceptTimeTo.AddMinutes(-15);
                                    monthlyVM.AccepttimeTo = AcceptTimeTo;
                                    if (monthlyVM.IsAccepted != true)
                                    {
                                        DateTime currentTime = DateTime.Now;


                                        if (currentTime > monthlyVM.Accepttimefrom && currentTime > monthlyVM.AccepttimeTo)
                                        {
                                            monthly.DriverId = 0;
                                            monthly.IsReserved = false;
                                            monthly.IsTimeScheduled = false;
                                            _context.Entry(monthly).State = EntityState.Modified;
                                            await _context.SaveChangesAsync();
                                            monthlyVM.IsTimeScheduled = monthly.IsTimeScheduled;
                                            var ignore = new IgnoreMontlyTrips();
                                            ignore.MonthlyId = monthly.MonthlyId;
                                            ignore.DriverId = driverId;
                                            _context.IgnoreMontlyTrips.Add(ignore);
                                            await _context.SaveChangesAsync();
                                        }

                                    }

                            }
                            else
                            {
                                DateTime? startTimes = monthlyVM.Pickuptime; // Nullable DateTime for the time part
                                DateTime? startDateNullable = monthlyVM.DateList[0].Date; // Nullable DateTime for the date part

                                if (startDateNullable.HasValue)
                                {
                                    DateTime startDate = startDateNullable.Value; // Get the date part

                                    if (startTimes.HasValue)
                                    {
                                        // Extract the time part from startTimes
                                        TimeSpan startTimeOfDay = startTimes.Value.TimeOfDay;

                                        // Combine the date from startDate and the time from startTimes into a single DateTime object
                                        DateTime combinedDateTime = startDate.Date.Add(startTimeOfDay);

                                        // Assign the combined DateTime to flexiVm.PickUpTime
                                        monthlyVM.Pickuptime = combinedDateTime;

                                        // Format the combined DateTime to a string if needed
                                        var starttimesFormatted = combinedDateTime.ToString("h:mm tt");
                                    }
                                }


                                // Convert the duration to an integer
                                int duration = Convert.ToInt32(dataconfiguretime.SchuduletripDuration);

                                // Calculate the time before the duration
                                DateTime beforeDurationTime = monthly.Pickuptime.HasValue
    ? monthly.Pickuptime.Value.AddHours(4)
    : default(DateTime);
                                if (beforeDurationTime < DateTime.Now)
                                {
                                    var ignoreMonthlyTrips = new Models.IgnoreMontlyTrips();
                                    ignoreMonthlyTrips.MonthlyId= monthly.MonthlyId;
                                    ignoreMonthlyTrips.DriverId = driverId;
                                    ignoreMonthlyTrips.IgnoretripresonsId = 0;
                                    _context.IgnoreMontlyTrips.Add(ignoreMonthlyTrips);
                                    await _context.SaveChangesAsync();
                                    monthly.IsCancelled = true;
                                    _context.Entry(monthly).State = EntityState.Modified;
                                    await _context.SaveChangesAsync();
                                    var userdata = _context.Users.Find(monthly.UserId);

                                    if (userdata != null)
                                    {
                                        string text = "Dear Patron, Your Booking ID:" + monthly.UniqueMonthlyId+ " has been cancelled. Contact us via app Chat/Call for any further assistance. Team Gochauffeurs here to assist you!";
                                        byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes(text);
                                        var s_unicode2 = System.Text.Encoding.UTF8.GetString(utf8Bytes);
                                        var url = "http://text.justsms.co.in/api.php?username=varaahi&apikey=cb2968ba7b0cbc38adb4&senderid=GOCHFF&templateid=1707171475698792818&mobile=" + userdata.PhoneNumber + "&message= " + s_unicode2;

                                        HttpClient client = new HttpClient();

                                        client.BaseAddress = new Uri(url);
                                        HttpResponseMessage response = client.GetAsync(url).Result;
                                    }
                                    continue;
                                }
                            }
                            if (istStartDateTime < currentIstTimePlus45Minutes)
                                {
                                    await Udatetriptoinstant(monthly.MonthlyId);
                                    monthlyVM.IsTimeScheduled = false;
                                }
                            }
                   
                        monthlyVMList.Add(monthlyVM);
                    }

                    return Ok(monthlyVMList);
                }
                else
                {
                    if (driverwallet != null)
                    {

                        return BadRequest($"Your wallet balance is {driverwallet.WalletBalance}, Please recharge the wallet in order to get the trips.");
                    }
                    else
                    {
                        return BadRequest($"Your wallet balance is 0, Please recharge the wallet in order to get the trips.");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        private async Task<ActionResult<Monthly>> Udatetriptoinstant(int MonthlyId)
        {
            var tripdata = await _context.Monthlies.FindAsync(MonthlyId);
            if (tripdata != null)
            {
                tripdata.IsTimeScheduled = false;
            }
            _context.Entry(tripdata).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return tripdata;
        }








        // PUT: api/Monthlies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMonthly(int id, Monthly monthly)
        {
            if (id != monthly.MonthlyId)
            {
                return BadRequest();
            }

            _context.Entry(monthly).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MonthlyExists(id))
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

        // POST: api/Monthlies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Monthly>> PostMonthly(Monthly monthly)
        {
            if (_context.Monthlies == null)
            {
                return Problem("Entity set 'DataContext.Monthlies'  is null.");
            }
            _context.Monthlies.Add(monthly);
            await _context.SaveChangesAsync();

            // Get the offerId as a string and pad it with leading zeros if needed
            string MonthlyId = monthly.MonthlyId.ToString().PadLeft(1, '0');

            // Get current month and day as strings
            string currentMonth = DateTime.Today.Month.ToString().PadLeft(2, '0');
            string currentDay = DateTime.Today.Day.ToString().PadLeft(2, '0');

            // Construct the offer code following the pattern:
            // 3 chars from OfferName, '0', OfferId, '0', currentMonth, currentDay
            var monthlyCode = $"Go{MonthlyId}Monthly{currentDay}{currentMonth}";
            monthly.UniqueMonthlyId = monthlyCode;
            _context.Entry(monthly).State = EntityState.Modified;
            await _context.SaveChangesAsync();



            return CreatedAtAction("GetMonthly", new { id = monthly.MonthlyId }, monthly);
        }

        // DELETE: api/Monthlies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMonthly(int id)
        {
            if (_context.Monthlies == null)
            {
                return NotFound();
            }
            var monthly = await _context.Monthlies.FindAsync(id);
            if (monthly == null)
            {
                return NotFound();
            }

            _context.Monthlies.Remove(monthly);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MonthlyExists(int id)
        {
            return (_context.Monthlies?.Any(e => e.MonthlyId == id)).GetValueOrDefault();
        }
        private double distance(double lat1, double lon1, double lat2, double lon2, char unit)
        {
            // Replace these coordinates with your actual latitude and longitude values
            var origin = new GeoCoordinate(lat1, lon1); // San Francisco, CA
            var destination = new GeoCoordinate(lat2, lon2); // Los Angeles, CA

            // Calculate aerial distance
            double distance = CalculateAerialDistance(origin, destination);

            return distance;
        }

        static double CalculateAerialDistance(GeoCoordinate origin, GeoCoordinate destination)
        {
            const double EarthRadius = 6371.0; // Radius of the Earth in kilometers

            double dLat = ToRadians(destination.Latitude - origin.Latitude);
            double dLon = ToRadians(destination.Longitude - origin.Longitude);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(ToRadians(origin.Latitude)) * Math.Cos(ToRadians(destination.Latitude)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return EarthRadius * c;
        }

        static double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }











        // GET: api/Monthlies
        [HttpGet("{driverId}/acceptedswitchMonthliesdata")]
        public async Task<ActionResult<IEnumerable<MonthlyVM>>> GetacceptedSwitchDriverData(int driverId)
        {
            try
            {
                var driver = await _context.Drivers.FindAsync(driverId);
                if (driver == null) return NotFound("Driver not found");

                var ignoreTrips = await _context.IgnoreMontlyTrips.Where(c => c.DriverId == driverId).ToListAsync();
                var distanceToGetTripsData = await _context.Driversurroundingtrips.FirstOrDefaultAsync();
                int distancetocover = distanceToGetTripsData?.Distance ?? 30;

                if (string.IsNullOrEmpty(driver.TransmissionTypeId) || string.IsNullOrEmpty(driver.VehicleTypeIds))
                {
                    return BadRequest("Please contact admin to update vehicle types");
                }

                var transmissionTypes = driver.TransmissionTypeId.Split(',').Select(int.Parse).ToList();
                var vehicleTypes = driver.VehicleTypeIds.Split(',').Select(int.Parse).ToList();

                var monthlyData = await _context.Monthlies
                    .Where(c => c.DriverId == 0 || c.DriverId == null || c.DriverId.HasValue)
                    .ToListAsync();

                var monthlyDataList = monthlyData
                    .Where(c => transmissionTypes.Contains(Convert.ToInt32(c.TransmissionId))
                                && vehicleTypes.Contains(Convert.ToInt32(c.VehicleTypeId)))
                    .ToList();

                // Remove accepted trips for all other drivers
                monthlyData = monthlyData.Where(t => !(t.IsAccepted == true && t.DriverId != driverId)).ToList();

                // Filter based on acceptance status
                var hasAcceptedTripsForDriver = monthlyData
                    .Any(t => t.DriverId == driverId && t.IsAccepted == true);

                if (hasAcceptedTripsForDriver)
                {
                    // If there are accepted trips for the specified DriverId, filter to show only those trips that are not accepted by any driver
                    monthlyData = monthlyData.Where(t => t.IsAccepted != true).ToList();
                }

                if (!monthlyDataList.Any()) return NoContent();

                var monthlyVMList = new List<MonthlyVM>();

                foreach (var monthly in monthlyDataList)
                {
                    var ignoredTrip = ignoreTrips.FirstOrDefault(c => c.MonthlyId == monthly.MonthlyId);
                    if (ignoredTrip != null) continue;

                    var locationTrip = monthly.PickUpLocationCoordinates.Split(',').ToList();
                    var driverDistance = distance(
                        Convert.ToDouble(locationTrip[0]),
                        Convert.ToDouble(locationTrip[1]),
                        Convert.ToDouble(driver.Latitude),
                        Convert.ToDouble(driver.Longitude),
                        'K');

                    if (driverDistance >= distancetocover) continue;

                    var monthlyVM = new MonthlyVM
                    {
                        MonthlyId = monthly.MonthlyId,
                        UniqueMonthlyId = monthly.UniqueMonthlyId,
                        TripVarientId = monthly.TripVarientId,
                        UsersTripsCancelResonsId = monthly.UsersTripsCancelResonsId,
                        PickUpLocation = monthly.PickUpLocation,
                        PickUpLocationCoordinates = monthly.PickUpLocationCoordinates,
                        DriverMeansOfTransport = monthly.DriverMeansOfTransport,
                        NoofDays = monthly.NoofDays,
                        Pickuptime = monthly.Pickuptime,
                        VehicleTypeId = monthly.VehicleTypeId,
                        TransmissionId = monthly.TransmissionId,
                        UserId = monthly.UserId,
                        EstimatedHours = monthly.EstimatedHours,
                        Estimatedprice = monthly.Estimatedprice,
                        IsAdvancedPayment = monthly.IsAdvancedPayment,
                        IsAccepted = monthly.IsAccepted,
                        FinalPrice = monthly.FinalPrice,
                        DateList = await _context.MonthlyDateLists
                            .Where(c => c.MonthlyId == monthly.MonthlyId)
                            .Select(datelist => new MonthlyDateListVM
                            {
                                MonthlyDateListId = datelist.MonthlyDateListId,
                                Date = datelist.Date,
                                MonthlyId = datelist.MonthlyId,
                                IsTripCompByDriver = datelist.IsTripCompByDriver,
                                ActualHours = datelist.ActualHours,
                                ActualPrice = datelist.ActualPrice,
                                ActualTaxValue = datelist.ActualTaxValue,
                                CuponId = datelist.CuponId,
                                ActualCuponPrice = datelist.ActualCuponPrice,
                                IsTripStarted = datelist.IsTripStarted,
                                selecteddateListvalue = datelist.selecteddateListvalue,

                                Isonroute = datelist.Isonroute,
                            }).ToListAsync()
                    };

                    var userTripsCancelReasons = await _context.UsersTripsCancelResons.FindAsync(monthlyVM.UsersTripsCancelResonsId);
                    if (userTripsCancelReasons != null)
                    {
                        monthlyVM.UserTripsCancelResonsName = userTripsCancelReasons.UserTripsCancelResonsName;
                    }

                    var tripVariantData = await _context.TripVariants.FindAsync(monthlyVM.TripVarientId);
                    if (tripVariantData != null)
                    {
                        monthlyVM.TripVarientName = tripVariantData.TripVariantName;
                    }

                    var vehicleData = await _context.VehicleTypes.FindAsync(monthlyVM.VehicleTypeId);
                    if (vehicleData != null)
                    {
                        monthlyVM.VehicleTypeName = vehicleData.VehicleTypeName;
                        monthlyVM.VehicleTypeImage = vehicleData.Icon;
                    }

                    var transmissionData = await _context.TransmissionTypes.FindAsync(monthlyVM.TransmissionId);
                    if (transmissionData != null)
                    {
                        monthlyVM.TransmissinName = transmissionData.TransmissionName;
                    }

                    var userData = await _context.Users.FindAsync(monthly.UserId);
                    if (userData != null)
                    {
                        monthlyVM.Name = userData.Name;
                        monthlyVM.PhoneNumber = userData.PhoneNumber;
                    }
                    if (monthly.IsTimeScheduled == true)
                    {
                        var dataconfiguretime = _context.Schuduletriptimechagemodel.Where(c => c.TripTypeId == 5).FirstOrDefault();
                        if (dataconfiguretime != null)
                        {
                            TimeZoneInfo istTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                            DateTime istStartDateTime = TimeZoneInfo.ConvertTimeFromUtc(monthly.Pickuptime.Value, istTimeZone);
                            DateTime currentIstTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, istTimeZone);

                            DateTime currentIstTimePlus45Minutes = currentIstTime.AddMinutes(Convert.ToInt32(dataconfiguretime.SchuduletripDuration));
                            if (monthly.IsReserved == true)
                            {

                                DateTime? startTimes = monthly.Pickuptime;

                                var starttimes = string.Format("{0:h:mm tt}", startTimes);

                                currentIstTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, istTimeZone);
                                // Assuming tripVM.StartTime is a string representing a date and time
                                monthlyVM.Pickuptime = Convert.ToDateTime(starttimes);

                                // Convert StartTime string to DateTime
                                DateTime startTimeDateTime = Convert.ToDateTime(monthlyVM.Pickuptime);

                                // Convert the duration to an integer
                                int duration = Convert.ToInt32(dataconfiguretime.SchuduletripDuration);

                                // Calculate the time before the duration
                                DateTime beforeDurationTime = startTimeDateTime.AddMinutes(-duration);

                                // Define AcceptTimeTo as beforeDurationTime
                                DateTime AcceptTimeTo = beforeDurationTime;


                                // Define AcceptTimeFrom as the beginning of the hour of beforeDurationTime
                                DateTime AcceptTimeFrom = new DateTime(beforeDurationTime.Year, beforeDurationTime.Month, beforeDurationTime.Day, beforeDurationTime.Hour, 0, 0);

                                // Output the results (if needed for debugging)
                                Console.WriteLine($"AcceptTimeFrom: {AcceptTimeFrom}");
                                Console.WriteLine($"AcceptTimeTo: {AcceptTimeTo}");

                                // You can assign these values to the appropriate properties in your view model if needed
                                monthlyVM.AccepttimeTo = AcceptTimeTo;
                                monthlyVM.Accepttimefrom = AcceptTimeTo.AddMinutes(-15); ;

                            }

                            if (istStartDateTime < currentIstTimePlus45Minutes)
                            {
                                await Udatetriptoinstant(monthly.MonthlyId);
                                monthlyVM.IsTimeScheduled = false;
                            }
                        }
                    }
                    monthlyVMList.Add(monthlyVM);
                }

                return Ok(monthlyVMList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
