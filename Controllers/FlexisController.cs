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
using Nest;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlexisController : ControllerBase
    {
        private readonly DataContext _context;

        public FlexisController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Flexis
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FlexisVm>>> GetFlexis()
        {
            try
            {
                var flexiModelList = _context.Flexis.ToList();
                var flexiVmList = new List<FlexisVm>();
                if (flexiModelList.Count > 0)
                {
                    foreach (var flexiModel in flexiModelList)
                    {
                        var flexivm = new FlexisVm();
                        flexivm.FlexiId = flexiModel.FlexiId;
                        flexivm.UniqueflexiId = flexiModel.UniqueflexiId;
                        flexivm.PickupLocation = flexiModel.PickupLocation;
                        flexivm.Coordinates = flexiModel.Coordinates;
                        flexivm.EstimatedHours = flexiModel.EstimatedHours;
                        flexivm.PickUpTime = flexiModel.PickUpTime;
                        flexivm.Language = flexiModel.Language;
                        flexivm.IsdriverArrived = flexiModel.IsdriverArrived;
                        flexivm.istripcompleted = flexiModel.IsTripCompByDriver;
                        flexivm.IsTripStarted = flexiModel.IsTripStarted;
                        flexivm.IsProcessing = flexiModel.IsProcessing;
                        flexivm.IsReserved = flexiModel.IsReserved;
                        flexivm.IsAccepted = flexiModel.IsAccepted;
                        flexivm.selecteddateListvalue = flexiModel.selecteddateListvalue;
                        flexivm.IsCancelled = flexiModel.IsCancelled;
                        flexivm.PickUPMapURL = flexiModel.PickUPMapURL;
                        flexivm.DriverMeansOfTransport = flexiModel.DriverMeansOfTransport;
                        flexivm.VehicleTypeId = flexiModel.VehicleTypeId;

                        var vehicleTypedata = _context.VehicleTypes.Find(flexivm.VehicleTypeId);
                        if (vehicleTypedata != null)
                        {
                            flexivm.VehicleTypeName = vehicleTypedata.VehicleTypeName;
                        }

                        flexivm.TransmissionId = flexiModel.TransmissionId;
                        var transmissionData = _context.TransmissionTypes.Find(flexivm.TransmissionId);
                        if (transmissionData != null)
                        {
                            flexivm.TransmissionName = transmissionData.TransmissionName;
                        }



                        flexivm.UsersTripsCancelResonsId = flexiModel.UsersTripsCancelResonsId;
                        var datausertripcancelresondata = _context.UsersTripsCancelResons.Find(flexivm.UsersTripsCancelResonsId);
                        if (datausertripcancelresondata != null)
                        {
                            flexivm.UserTripsCancelResonsName = datausertripcancelresondata.UserTripsCancelResonsName;
                        }
                        flexivm.UserId = flexiModel.UserId;
                        var userdata = _context.Users.Find(flexivm.UserId);
                        if (userdata != null)
                        {
                            flexivm.Name = userdata.Name;
                            flexivm.PhoneNumber = userdata.PhoneNumber;


                        }
                        flexivm.FinalPrice = flexiModel.FinalPrice;
                        if(flexivm.FinalPrice < flexivm.EstimatedPrice)
                        {
                            flexivm.FinalPrice = Convert.ToDecimal(flexivm.EstimatedPrice);
                        }
                        flexivm.IsDriverAssigned = flexiModel.IsDriverAssigned;
                        flexivm.EstimatedPrice = flexiModel.EstimatedPrice;
                        flexivm.EstimatedTaxValue = flexiModel.EstimatedTaxValue;
                        flexivm.EstimatedCuponPrice = flexiModel.EstimatedCuponPrice;
                        flexivm.DriverId = flexiModel.DriverId;
                        var driverData = _context.Drivers.Find(flexivm.DriverId);
                        if (driverData != null)
                        {
                            flexivm.DriverName = driverData.DriverName;
                            flexivm.driverPhoneNumber = driverData.PhoneNumber;
                        }
                        flexivm.IsAdvancePaid = flexiModel.IsAdvancePaid;
                        flexivm.AdvancePaid = flexiModel.AdvancePaid;
                        flexivm.NoofDays = flexiModel.NoofDays;
                        flexivm.FlexiSelectedDateTime = flexiModel.FlexiSelectedDateTime;
                        flexiVmList.Add(flexivm);
                    }
                    return Ok(flexiVmList);
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        // GET: api/Flexis/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FlexisVm>> GetFlexi(int id)
        {
            try
            {
                var flexiModel = _context.Flexis.Find(id);
                var flexiVmList = new List<FlexisVm>();
                if (flexiModel != null)
                {
                    var flexivm = new FlexisVm();

                    flexivm.FlexiId = flexiModel.FlexiId;
                    flexivm.UniqueflexiId = flexiModel.UniqueflexiId;
                    flexivm.PickupLocation = flexiModel.PickupLocation;
                    flexivm.PickUPMapURL = flexiModel.PickUPMapURL;
                    flexivm.Coordinates = flexiModel.Coordinates;
                    flexivm.EstimatedHours = flexiModel.EstimatedHours;
                    flexivm.PickUpTime = flexiModel.PickUpTime;
                    flexivm.Language = flexiModel.Language;
                    flexivm.IsdriverArrived = flexiModel.IsdriverArrived;
                    var anonymusdata = _context.Anonymoustripcharges.Where(c => c.TriptypeId == 4).FirstOrDefault();
                    if (anonymusdata != null)
                    {
                        flexivm.Anonymuscharge = anonymusdata.Amount.ToString();
                    }
                    flexivm.istripcompleted = flexiModel.IsTripCompByDriver;
                    flexivm.IsTripStarted = flexiModel.IsTripStarted;
                    flexivm.IsProcessing = flexiModel.IsProcessing;
                    flexivm.IsReserved = flexiModel.IsReserved;
                    flexivm.IsCancelled = flexiModel.IsCancelled;
                    flexivm.IsAccepted = flexiModel.IsAccepted;
                    flexivm.DriverMeansOfTransport = flexiModel.DriverMeansOfTransport;
                    flexivm.Isonroute = flexiModel.Isonroute;
                    flexivm.VehicleTypeId = flexiModel.VehicleTypeId;
                    flexivm.selecteddateListvalue = flexiModel.selecteddateListvalue;

                    flexivm.UsersTripsCancelResonsId = flexiModel.UsersTripsCancelResonsId;
                    var datausertripcancelresondata = _context.UsersTripsCancelResons.Find(flexivm.UsersTripsCancelResonsId);
                    if (datausertripcancelresondata != null)
                    {
                        flexivm.UserTripsCancelResonsName = datausertripcancelresondata.UserTripsCancelResonsName;
                    }

                    var vehicleTypedata = _context.VehicleTypes.Find(flexivm.VehicleTypeId);
                    if (vehicleTypedata != null)
                    {
                        flexivm.VehicleTypeName = vehicleTypedata.VehicleTypeName;
                    }

                    flexivm.TransmissionId = flexiModel.TransmissionId;
                    var transmissionData = _context.TransmissionTypes.Find(flexivm.TransmissionId);
                    if (transmissionData != null)
                    {
                        flexivm.TransmissionName = transmissionData.TransmissionName;
                    }

                    flexivm.UserId = flexiModel.UserId;
                    var userdata = _context.Users.Find(flexivm.UserId);
                    if (userdata != null)
                    {
                        flexivm.Name = userdata.Name;
                        flexivm.PhoneNumber = userdata.PhoneNumber;
                    }

                    flexivm.IsDriverAssigned = flexiModel.IsDriverAssigned;
                    flexivm.IsAccepted = flexiModel.IsAccepted;
                    flexivm.EstimatedPrice = flexiModel.EstimatedPrice;
                    flexivm.FinalPrice = flexiModel.FinalPrice;
                    
                    flexivm.EstimatedTaxValue = flexiModel.EstimatedTaxValue;
                    flexivm.EstimatedCuponPrice = flexiModel.EstimatedCuponPrice;
                    flexivm.DriverId = flexiModel.DriverId;
                    var driverData = _context.Drivers.Find(flexivm.DriverId);
                    if (driverData != null)
                    {
                        flexivm.DriverName = driverData.DriverName;
                        flexivm.driverPhoneNumber = driverData.PhoneNumber;
                    }
                    flexivm.IsAdvancePaid = flexiModel.IsAdvancePaid;
                    flexivm.AdvancePaid = flexiModel.AdvancePaid;
                    flexivm.NoofDays = flexiModel.NoofDays;
                    flexivm.FlexiSelectedDateTime = flexiModel.FlexiSelectedDateTime;
                    var price = 0;
                    // Declare DateList
                    var DateList = await _context.FlexiDatesLists
                        .Where(c => c.FlexiId == flexiModel.FlexiId)
                        .Select(datelist => new FlexiDateListVM
                        {
                            FlexiDatesListId = datelist.FlexiDatesListId,
                            Date = datelist.Date,
                            FlexiId = datelist.FlexiId,
                            IsTripCompByDriver = datelist.IsTripCompByDriver,
                            ActualHours = datelist.ActualHours,
                            ActualPrice = datelist.ActualPrice,
                            ActualTaxValue = datelist.ActualTaxValue,
                            CuponId = datelist.CuponId,
                            ActualCuponPrice = datelist.ActualCuponPrice,
                            IsTripStarted = datelist.IsTripStarted,
                            selecteddateListvalue = datelist.selecteddateListvalue,
                            
                                 Isonroute = datelist.Isonroute,
                        }).ToListAsync();
                    price = DateList
    .Where(d => d.IsTripCompByDriver == true)
    .Sum(d => Convert.ToInt32(d.ActualPrice));
                    flexivm.FinalPrice = price;
                    if (flexivm.FinalPrice < Convert.ToDecimal(flexivm.EstimatedPrice))
                    {
                        flexivm.FinalPrice = Convert.ToDecimal(flexivm.EstimatedPrice);
                    }

                    var taxdata = await _context.Taxes.FirstOrDefaultAsync();
                    if (taxdata != null) 
                    {
                        var taxvalue = await _context.Taxes.FirstOrDefaultAsync();
                        if (taxvalue != null)
                        {
                            var percentage = taxvalue.Percentage;
                            var taxvalues = flexivm.FinalPrice * percentage / 100;
                            flexivm.Taxvalue = taxvalues.ToString();
                        }

                    }
                    flexivm.DateList = DateList; // Assign DateList to the flexivm object

                    flexiVmList.Add(flexivm);

                    return Ok(flexiVmList);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        // GET: api/Flexis
        [HttpGet("{driverId}/flexidata")]
        public async Task<ActionResult<IEnumerable<FlexisVm>>> GetMonthliesDriverData(int driverId)
        {
            try
            {
                var driver = await _context.Drivers.Where(c => c.DriverId == driverId && c.IsDriverActive == true).FirstOrDefaultAsync(); ;
                if (driver == null) return NotFound("Driver not found");

                var ignoreTrips = await _context.IgnoreFlexiTrips.Where(c => c.DriverId == driverId).ToListAsync();
                var distanceToGetTripsData = await _context.Driversurroundingtrips.FirstOrDefaultAsync();
                int distancetocover = distanceToGetTripsData?.Distance ?? 30;

                if (string.IsNullOrEmpty(driver.TransmissionTypeId) || string.IsNullOrEmpty(driver.VehicleTypeIds))
                {
                    return BadRequest("Please contact admin to update vehicle types");
                }

                var transmissionTypes = driver.TransmissionTypeId.Split(',').Select(int.Parse).ToList();
                var vehicleTypes = driver.VehicleTypeIds.Split(',').Select(int.Parse).ToList();

                var flexiData = await _context.Flexis
                    .Where(c => c.DriverId == 0 || c.DriverId == null || c.DriverId ==driverId)
                    .ToListAsync();

                var flexiModelList = flexiData
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
                if (driverwallet != null && driverwallet.WalletBalance == "")
                {
                    driverwallet.WalletBalance = 0.ToString();
                }
                if (driverwallet != null && Convert.ToInt32(driverwallet.WalletBalance) > -100)
                {

                        if (!flexiModelList.Any()) return NoContent();

                    var flexiVmList = new List<FlexisVm>();

                    foreach (var flexiModel in flexiModelList)
                    {
                        var ignoredTrip = ignoreTrips.FirstOrDefault(c => c.FlexiId == flexiModel.FlexiId);
                        if (ignoredTrip != null) continue;

                        var locationTrip = flexiModel.Coordinates.Split(',').ToList();
                        var driverDistance = distance(
                            Convert.ToDouble(locationTrip[0]),
                            Convert.ToDouble(locationTrip[1]),
                            Convert.ToDouble(driver.Latitude),
                            Convert.ToDouble(driver.Longitude),
                            'K');

                        if (driverDistance >= distancetocover) continue;

                        var flexiVm = new FlexisVm
                        {
                            FlexiId = flexiModel.FlexiId,
                            UniqueflexiId = flexiModel.UniqueflexiId,
                            PickupLocation = flexiModel.PickupLocation,
                            Coordinates = flexiModel.Coordinates,
                            EstimatedHours = flexiModel.EstimatedHours,
                            PickUpTime = flexiModel.PickUpTime,
                            Language = flexiModel.Language,
                            PickUPMapURL = flexiModel.PickUPMapURL,
                            DriverMeansOfTransport = flexiModel.DriverMeansOfTransport,
                            VehicleTypeId = flexiModel.VehicleTypeId,
                            TransmissionId = flexiModel.TransmissionId,
                            UsersTripsCancelResonsId = flexiModel.UsersTripsCancelResonsId,
                            UserId = flexiModel.UserId,
                            IsDriverAssigned = flexiModel.IsDriverAssigned,
                            EstimatedPrice = flexiModel.EstimatedPrice,
                            EstimatedTaxValue = flexiModel.EstimatedTaxValue,
                            EstimatedCuponPrice = flexiModel.EstimatedCuponPrice,
                            FinalPrice = flexiModel.FinalPrice,
                            DriverId = flexiModel.DriverId,
                            IsAccepted = flexiModel.IsAccepted,
                            IsAdvancePaid = flexiModel.IsAdvancePaid,
                            AdvancePaid = flexiModel.AdvancePaid,
                            NoofDays = flexiModel.NoofDays,
                            IsReserved = flexiModel.IsReserved,
                            IsTimeScheduled= flexiModel.IsTimeScheduled,
                            IsCancelled = flexiModel.IsCancelled,
                            FlexiSelectedDateTime = flexiModel.FlexiSelectedDateTime,
                            DateList = await _context.FlexiDatesLists
                                .Where(c => c.FlexiId == flexiModel.FlexiId)
                                .Select(datelist => new FlexiDateListVM
                                {
                                    FlexiDatesListId = datelist.FlexiDatesListId,
                                    Date = datelist.Date,
                                    FlexiId = datelist.FlexiId,
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

                        var anonymuscharges = await _context.Anonymoustripcharges.Where(c => c.TriptypeId == 4).FirstOrDefaultAsync();
                        var driversubdata = await _context.Driversubscriptions
                                     .Where(c => c.DriverId == driverId&& c.Expirydate <= DateTime.Now)
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
                                var taxvalue = flexiVm.EstimatedPrice * taxpercentage / 100;


                                if (anonymuscharges != null && anonymuscharges.Amount.HasValue)
                                {
                                    flexiVm.DriverPrice = flexiVm.EstimatedPrice - taxvalue - Convert.ToInt32(anonymuscharges.Amount.Value)* percetage/100;
                                }
                                else
                                {
                                    // Handle the case when anonymuscharges or Amount is null
                                    flexiVm.DriverPrice = flexiVm.EstimatedPrice - taxvalue*percetage / 100;
                                }
                            }
                            else
                            {
                                var taxvalue = flexiVm.EstimatedPrice * taxpercentage / 100;


                                if (anonymuscharges != null && anonymuscharges.Amount.HasValue)
                                {
                                    flexiVm.DriverPrice = flexiVm.EstimatedPrice - taxvalue - Convert.ToInt32(anonymuscharges.Amount.Value);
                                }
                                else
                                {
                                    // Handle the case when anonymuscharges or Amount is null
                                    flexiVm.DriverPrice = flexiVm.EstimatedPrice - taxvalue ;
                                }
                            }



                        }
                        else
                        {
                            var percetage = 80;
                            var taxvalue = flexiVm.EstimatedPrice * taxpercentage / 100;


                                flexiVm.DriverPrice = flexiVm.EstimatedPrice - taxvalue - Convert.ToInt32(anonymuscharges.Amount.Value) * percetage / 100;
                               // Handle the case when anonymuscharges or Amount is null
                                flexiVm.DriverPrice = flexiVm.EstimatedPrice - taxvalue * percetage / 100;
                            
                        }
                        var driverdata = _context.Drivers.Find(flexiModel.DriverId);
                        if (driverdata != null)
                        {
                            flexiVm.DriverName = driverdata.DriverName;
                            flexiVm.PhoneNumber = driverdata.PhoneNumber;
                        }

                        var vehicleTypeData = await _context.VehicleTypes.FindAsync(flexiVm.VehicleTypeId);
                        if (vehicleTypeData != null)
                        {
                            flexiVm.VehicleTypeName = vehicleTypeData.VehicleTypeName;
                        }

                        var transmissionData = await _context.TransmissionTypes.FindAsync(flexiVm.TransmissionId);
                        if (transmissionData != null)
                        {
                            flexiVm.TransmissionName = transmissionData.TransmissionName;
                        }

                        var userTripCancelReasonData = await _context.UsersTripsCancelResons.FindAsync(flexiVm.UsersTripsCancelResonsId);
                        if (userTripCancelReasonData != null)
                        {
                            flexiVm.UserTripsCancelResonsName = userTripCancelReasonData.UserTripsCancelResonsName;
                        }

                        var userData = await _context.Users.FindAsync(flexiVm.UserId);
                        if (userData != null)
                        {
                            flexiVm.Name = userData.Name;
                            flexiVm.PhoneNumber = userData.PhoneNumber;
                        }

                        var dataconfiguretime = _context.Schuduletriptimechagemodel.Where(c => c.TripTypeId == 4).FirstOrDefault();
                        if (dataconfiguretime != null)
                        {
                            TimeZoneInfo istTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                            DateTime istStartDateTime = TimeZoneInfo.ConvertTimeFromUtc(flexiModel.PickUpTime.Value, istTimeZone);
                            DateTime currentIstTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, istTimeZone);

                            DateTime currentIstTimePlus45Minutes = currentIstTime.AddMinutes(Convert.ToInt32(dataconfiguretime.SchuduletripDuration));
                            if (flexiModel.IsReserved == true)
                            {

                                DateTime? startTimes = flexiModel.PickUpTime; // Nullable DateTime for the time part
                                DateTime? startDateNullable = flexiVm.DateList[0].Date; // Nullable DateTime for the date part

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
                                        flexiVm.PickUpTime = combinedDateTime;

                                        // Format the combined DateTime to a string if needed
                                        var starttimesFormatted = combinedDateTime.ToString("h:mm tt");
                                    }
                                }

                                    // Convert the duration to an integer
                                    int duration = Convert.ToInt32(dataconfiguretime.SchuduletripDuration);

                                // Calculate the time before the duration
                                DateTime beforeDurationTime = flexiVm.PickUpTime.HasValue
    ? flexiVm.PickUpTime.Value.AddMinutes(-duration)
    : default(DateTime);

                                // Define AcceptTimeTo as beforeDurationTime
                                DateTime AcceptTimeTo = beforeDurationTime;


                                // Define AcceptTimeFrom as the beginning of the hour of beforeDurationTime
                                DateTime AcceptTimeFrom = new DateTime(beforeDurationTime.Year, beforeDurationTime.Month, beforeDurationTime.Day, beforeDurationTime.Hour, 0, 0);

                                // Output the results (if needed for debugging)
                                Console.WriteLine($"AcceptTimeFrom: {AcceptTimeFrom}");
                                Console.WriteLine($"AcceptTimeTo: {AcceptTimeTo}");

                                // You can assign these values to the appropriate properties in your view model if needed
                                flexiVm.Accepttimefrom = AcceptTimeTo.AddMinutes(-15);
                                flexiVm.AccepttimeTo = AcceptTimeTo;
                                if (flexiVm.IsAccepted != true)
                                {
                                    DateTime currentTime = DateTime.Now;


                                    if (currentTime > flexiVm.Accepttimefrom && currentTime > flexiVm.AccepttimeTo)
                                    {
                                        flexiModel.DriverId = 0;
                                        flexiModel.IsReserved = false;
                                        flexiModel.IsTimeScheduled = false;
                                        _context.Entry(flexiModel).State = EntityState.Modified;
                                        await _context.SaveChangesAsync();
                                        flexiVm.IsTimeScheduled = flexiModel.IsTimeScheduled;
                                        var ignore = new IgnoreFlexiTrips();
                                        ignore.FlexiId = flexiModel.FlexiId;
                                        ignore.DriverId = driverId;
                                        _context.IgnoreFlexiTrips.Add(ignore);
                                        await _context.SaveChangesAsync();
                                    }

                                }
                            }
                            else
                            {
                                DateTime? startTimes = flexiModel.PickUpTime; // Nullable DateTime for the time part
                                DateTime? startDateNullable = flexiVm.DateList[0].Date; // Nullable DateTime for the date part

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
                                        flexiVm.PickUpTime = combinedDateTime;

                                        // Format the combined DateTime to a string if needed
                                        var starttimesFormatted = combinedDateTime.ToString("h:mm tt");
                                    }
                                }

                                // Convert the duration to an integer
                                int duration = Convert.ToInt32(dataconfiguretime.SchuduletripDuration);

                                // Calculate the time before the duration
                                DateTime beforeDurationTime = flexiVm.PickUpTime.HasValue
    ? flexiVm.PickUpTime.Value.AddHours(4)
    : default(DateTime);        
                                if (beforeDurationTime < DateTime.Now) 
                                {
                                    var IgnoreFlexiTrips = new Models.IgnoreFlexiTrips();
                                    IgnoreFlexiTrips.FlexiId = flexiModel.FlexiId;
                                    IgnoreFlexiTrips.DriverId = driverId;
                                    IgnoreFlexiTrips.IgnoretripresonsId = 0;
                                    _context.IgnoreFlexiTrips.Add(IgnoreFlexiTrips);
                                    await _context.SaveChangesAsync();
                                    flexiModel.IsCancelled = true;
                                    _context.Entry(flexiModel).State = EntityState.Modified;
                                    await _context.SaveChangesAsync();
                                    var userdata = _context.Users.Find(flexiModel.UserId);

                                    if (userdata != null)
                                    {
                                        string text = "Dear Patron, Your Booking ID:" + flexiModel.UniqueflexiId + " has been cancelled. Contact us via app Chat/Call for any further assistance. Team Gochauffeurs here to assist you!";
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
                                await Udatetriptoinstant(flexiModel.FlexiId);
                                flexiVm.IsTimeScheduled = false;
                            }
                        }

                        flexiVmList.Add(flexiVm);
                    }

                    return Ok(flexiVmList);
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

        private async Task<ActionResult<Flexi>> Udatetriptoinstant(int flexiId)
        {
            var tripdata = await _context.Flexis.FindAsync(flexiId);
            if (tripdata != null)
            {
                tripdata.IsTimeScheduled = false;
            }
            _context.Entry(tripdata).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return tripdata;
        }



        // PUT: api/Flexis/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        public async Task<IActionResult> PutFlexi(int id, Flexi flexi)
        {
            if (id != flexi.FlexiId)
            {
                return BadRequest();
            }

            _context.Entry(flexi).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlexiExists(id))
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

        // POST: api/Flexis
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Flexi>> PostFlexi(Flexi flexi)
        {
            if (_context.Flexis == null)
            {
                return Problem("Entity set 'DataContext.Flexis'  is null.");
            }
            _context.Flexis.Add(flexi);
            await _context.SaveChangesAsync();

            // Get the offerId as a string and pad it with leading zeros if needed
            string flexiId = flexi.FlexiId.ToString().PadLeft(1, '0');

            // Get current month and day as strings
            string currentMonth = DateTime.Today.Month.ToString().PadLeft(2, '0');
            string currentDay = DateTime.Today.Day.ToString().PadLeft(2, '0');

            // Construct the offer code following the pattern:
            // 3 chars from OfferName, '0', OfferId, '0', currentMonth, currentDay
            var flexiCode = $"Go{flexiId}flexi{currentDay}{currentMonth}";
            flexi.UniqueflexiId = flexiCode;
            _context.Entry(flexi).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetFlexi", new { id = flexi.FlexiId }, flexi);
        }


        // DELETE: api/Flexis/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlexi(int id)
        {
            if (_context.Flexis == null)
            {
                return NotFound();
            }
            var flexi = await _context.Flexis.FindAsync(id);
            if (flexi == null)
            {
                return NotFound();
            }

            _context.Flexis.Remove(flexi);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FlexiExists(int id)
        {
            return (_context.Flexis?.Any(e => e.FlexiId == id)).GetValueOrDefault();
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

    }
}
