using GoChauffeurWebApi.Data;
using GoChauffeurWebApi.Models;
using GoChauffeurWebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrentBookings : ControllerBase
    {
        private readonly DataContext _context;
        private Dictionary<string, int> googleDistanceCache = new Dictionary<string, int>();

        public CurrentBookings(DataContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<TripVM>>> GetTrips(int id)
        {
            try
            {
                var tripdata = await _context.Trips.Where(c => c.DriverId == id && c.IsPaymentdone != true && c.IsCancelled != true && c.IsAccepted == true && c.IsPaymentdone != true).ToListAsync();

                var tripVMList = new List<TripVM>();
                var driverwallet = _context.Driverwallets.Where(c => c.DriverId == id).FirstOrDefault();
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

                    if (tripdata.Count > 0)
                    {
                        var usersById = _context.Users.ToDictionary(u => u.Id);
                        var driversById = _context.Drivers.ToDictionary(d => d.DriverId);
                        foreach (var trip in tripdata)
                        {
                            var tripVM = new TripVM();

                            tripVM.TripId = trip.TripId;

                            tripVM.NoOfHoursSelected = trip.NoOfHoursSelected;
                            tripVM.IsATDrop = trip.IsAtDrop;
                            tripVM.Istakenpics = trip.Istakenpics;
                            tripVM.IsEndPicsTaken = trip.IsEndPicsTaken;
                            tripVM.IsPaymentdone = trip.IsPaymentdone;
                            //var userData = usersById[Convert.ToInt32(trip.UserId)];

                            tripVM.UserId = trip.UserId;
                            var userData = _context.Users.Find(tripVM.UserId);
                            if (userData != null)
                            {

                                tripVM.UserName = userData.Name;
                                tripVM.UserPhoneNumber = userData.PhoneNumber;
                                tripVM.Image = userData.UserImage;
                            }


                            tripVM.DriverId = trip.DriverId;
                            var driverData = driversById[Convert.ToInt32(trip.DriverId)];
                            tripVM.DriverName = driverData.DriverName;
                            tripVM.DriverImage = driverData.Image;
                            tripVM.Experience = driverData.Experiance;
                            tripVM.TripsUniqueId = trip.TripsUniqueId;
                            tripVM.PickUPMapURL = trip.PickUPMapURL;
                            tripVM.DropUPMapURL = trip.DropUPMapURL;
                            tripVM.IsTimeScheduled = trip.IsTimeScheduled;
                            tripVM.StartDate = trip.StartDateTime;
                            tripVM.FromLocationName = trip.FromLocationName;
                            tripVM.ToLocationName = trip.ToLocationName;
                            tripVM.EstimatedPrice = trip.EstimatedPrice.ToString();
                            tripVM.IsdriverArrived = trip.IsdriverArrived;
                            tripVM.IsTripStarted = trip.IsTripStarted;
                            tripVM.IsATDrop = trip.IsAtDrop;
                            tripVM.IsCancelled = trip.IsCancelled;

                            tripVM.IsAccepted = trip.IsAccepted;
                            tripVM.IsReserved = trip.IsReserved;

                            tripVM.TripsUniqueId = trip.TripsUniqueId;

                            tripVM.Isonroute = trip.Isonroute;
                            tripVM.IsSelfBook = trip.IsSelfBook;

                            DateTime? startTime = tripVM.StartDate;

                            var starttime = String.Format("{0:h:mm tt}", startTime);

                            tripVM.StartTime = starttime;

                            tripVM.EndDate = trip.EndDateTime;

                            DateTime? endTime = tripVM.EndDate;

                            var endtime = String.Format("{0:h:mm tt}", endTime);
                            tripVM.EndTime = endtime;

                            tripVM.ActualEndDate = trip.ActualEndTime;

                            DateTime? actualendTime = tripVM.ActualEndDate;

                            var actualendtime = String.Format("{0:h:mm tt}", actualendTime);

                            tripVM.ActualEndTime = actualendtime;

                            if (tripVM.StartDate.HasValue && tripVM.ActualEndDate.HasValue)
                            {
                                TimeSpan duration = tripVM.ActualEndDate.Value - tripVM.StartDate.Value;

                                double minutes = duration.TotalMinutes;
                                tripVM.ActualTotalMinutes = minutes;
                            }

                            if (tripVM.StartDate.HasValue && tripVM.EndDate.HasValue)
                            {
                                TimeSpan duration = tripVM.EndDate.Value - tripVM.StartDate.Value;

                                double minutes = duration.TotalMinutes;

                                tripVM.TotalMinutes = minutes;
                            }

                            tripVM.RequestedDate = trip.RequstedDateTime;

                            DateTime? requestedTime = tripVM.RequestedDate;

                            var requestedtime = String.Format("{0:h:mm tt}", requestedTime);

                            tripVM.RequestedTime = requestedtime;

                            tripVM.TripTypeId = trip.TripTypeId;

                            tripVM.IsTripCompByDriver = trip.IsTripCompByDriver;
                            tripVM.NoOfHoursSelected = trip.NoOfHoursSelected;
                            var tripTypeData = _context.TripTypes.Find(trip.TripTypeId);

                            if (tripTypeData != null)
                            {

                                tripVM.TripTypeName = tripTypeData.TripName;
                            }

                            tripVM.TripvarientId = trip.TripvarientId;

                            var triptypevariantdata = _context.TripVariants.Find(trip.TripvarientId);

                            if (triptypevariantdata != null)
                            {

                                tripVM.BasePrice = triptypevariantdata.BasePrice;

                                tripVM.KilometerLimit = triptypevariantdata.KilometerLimit;

                                tripVM.NightCharges = triptypevariantdata.NightCharges;

                                tripVM.ChargesperMinute = triptypevariantdata.ChargesperMinute;

                                tripVM.PricePerKilometers = triptypevariantdata.PricePerKilometers;

                                tripVM.IsTripOneway = triptypevariantdata.IsTripOneway;
                            }

                            tripVM.VehicleId = trip.VehicleId;

                            if (trip.VehicleId != 0)
                            {

                                var vehicledata = _context.Vehicles.Find(trip.VehicleId);

                                if (vehicledata != null)
                                {

                                    tripVM.VehicleNo = vehicledata.VehicleNo;

                                    if (!string.IsNullOrEmpty(vehicledata.Images))
                                    {

                                        var images = vehicledata.Images.Split(',').ToList();

                                        var vehicleimagelist = new List<VehicleImages>();

                                        if (images.Count > 0)
                                        {

                                            foreach (var image in images)
                                            {

                                                var imagevm = new VehicleImages();

                                                imagevm.VechileImage = image;

                                                vehicleimagelist.Add(imagevm);
                                            }
                                        }

                                        tripVM.VehicleImages = vehicleimagelist;
                                    }

                                    tripVM.VehicleTypeId = vehicledata.VehicleTypeId;

                                    if (tripVM.VehicleTypeId == 0)
                                    {

                                        var vehicletypedata = _context.VehicleTypes.Find(tripVM.VehicleTypeId);

                                        if (vehicletypedata != null)
                                        {

                                            tripVM.VehicleTypeName = vehicletypedata.VehicleTypeName;
                                        }
                                    }
                                    tripVM.TransmissionTypeId = trip.TransmissionTypeId;
                                    if (tripVM.TransmissionTypeId != 0 && tripVM.TransmissionTypeId != null)
                                    {
                                        var transmissiontypedata = _context.TransmissionTypes.Find(tripVM.TransmissionTypeId);

                                        if (transmissiontypedata != null)
                                        {
                                            tripVM.TransmissionTypeName = transmissiontypedata.TransmissionName;
                                        }
                                    }

                                }

                            }
                            if (!string.IsNullOrEmpty(trip.FromLocation))
                            {

                                var fromLocations = trip.FromLocation.Split(',').ToList();

                                if (fromLocations.Count > 0)
                                {

                                    tripVM.FromLatitude = fromLocations[0];

                                    tripVM.FromLongitude = fromLocations[1];

                                }
                            }
                            if (!string.IsNullOrEmpty(trip.ToLocation))
                            {

                                var toLocations = trip.ToLocation.Split(',').ToList();

                                if (toLocations.Count > 0)
                                {

                                    tripVM.ToLatitude = toLocations[0];

                                    tripVM.ToLongitude = toLocations[1];
                                }
                            }

                            int travelledonroaddistance = 0;

                            var drivertracking = _context.DriverTrackings.Where(c => c.DriverId == tripVM.DriverId && c.TripId == tripVM.TripId).ToList();

                            if (drivertracking.Count > 1)
                            {

                                var currentEntry = drivertracking.FirstOrDefault();

                                var nextEntry = drivertracking.Last();


                                Decimal? currentLatitudeStr = currentEntry.Latitude;

                                Decimal? currentLongitudeStr = currentEntry.Longitude;

                                Decimal? nextLatitudeStr = nextEntry.Latitude;

                                Decimal? nextLongitudeStr = nextEntry.Longitude;

                                int? googleDistanceResult = await GetCachedGoogleDistance(currentLatitudeStr.ToString(), currentLongitudeStr.ToString(), nextLatitudeStr.ToString(), nextLongitudeStr.ToString());

                                // Extract the integer value from the result
                                if (googleDistanceResult.HasValue)
                                {
                                    int actualGoogleDistance = googleDistanceResult.Value;
                                    // Use actualGoogleDistance as needed
                                    Console.WriteLine($"Actual Google Distance: {actualGoogleDistance}");
                                    travelledonroaddistance += actualGoogleDistance;
                                }
                                else
                                {
                                    // Handle the case where the result is null or an error occurred
                                    Console.WriteLine("Failed to retrieve Google Distance.");
                                }


                            }
                            tripVM.DistanceTravelled = travelledonroaddistance;
                            var entityDistance = distance(Convert.ToDouble(tripVM.FromLatitude), Convert.ToDouble(tripVM.FromLongitude), Convert.ToDouble(tripVM.ToLatitude), Convert.ToDouble(tripVM.ToLongitude), 'K');


                            tripVM.AerialDistance = Math.Round(Convert.ToDecimal(entityDistance), 2);

                            var aerialDistancedata = _context.AerialDistancePrices.ToList();

                            if (aerialDistancedata.Count > 0)
                            {

                                tripVM.Aerialpriceperkilometer = Convert.ToDecimal(aerialDistancedata[0].AerialDistancePriceperKilometer);
                            }

                            tripVM.ExpectedDistance = trip.ExpectedDistance;

                            tripVM.CuponId = tripVM.CuponId;

                            var cupondata = _context.Cupons.Find(tripVM.CuponId);

                            if (cupondata != null)
                            {

                                tripVM.CuponName = cupondata.CuponName;

                                tripVM.CuponCode = cupondata.CuponCode;

                                tripVM.CuponPercentage = cupondata.Percentage;
                            }
                            else
                            {
                                tripVM.CuponPercentage = 0;
                            }

                            tripVM.ImageUrlsList = trip.ImageUrlsList;
                            tripVM.Isonroute = trip.Isonroute;
                            tripVM.IsSecuredTrip = trip.IsSecuredTrip;

                            if (tripVM.IsTripOneway == true)
                            {

                                var expectedextradiatancethanbaseLimit = Convert.ToDecimal(tripVM.ExpectedDistance) - Convert.ToDecimal(tripVM.KilometerLimit);

                                var expectedtotalpriceforextradistance = expectedextradiatancethanbaseLimit * tripVM.PricePerKilometers;

                                var expectedtotalprice = expectedtotalpriceforextradistance + (tripVM.BasePrice * tripVM.NoOfHoursSelected) + tripVM.Aerialpriceperkilometer;

                                var cuponpercentagevalue = expectedtotalprice * tripVM.CuponPercentage / 100;

                                var expectedtotalvaluewithouttax = expectedtotalprice - cuponpercentagevalue;

                                Decimal? securetaxvalue = 0;

                                if (tripVM.IsSecuredTrip == true)
                                {

                                    var insurencedata = _context.InsurenceTaxandPrice.FirstOrDefault();

                                    if (insurencedata != null)
                                    {

                                        var taxpercentage = insurencedata.TaxPercentage;

                                        var insurencetaxvalue = expectedtotalvaluewithouttax * taxpercentage / 100;

                                        securetaxvalue = insurencetaxvalue;
                                    }
                                }

                                Decimal travelledtotaltaxvalue = 0;

                                var taxesdata = _context.Taxes.ToList();

                                if (taxesdata.Count > 0)
                                {
                                    foreach (var tax in taxesdata)
                                    {

                                        var taxpercentage = Convert.ToInt32(tax.Percentage);
                                        tripVM.Percentage = taxpercentage;
                                        var taxvalue = Convert.ToDecimal(expectedtotalvaluewithouttax) * taxpercentage / 100;

                                        var travelledtaxvalue = Convert.ToDecimal(expectedtotalvaluewithouttax) * taxpercentage / 100;

                                        travelledtotaltaxvalue += travelledtaxvalue;

                                    }
                                }
                                tripVM.ExpectedTotalTripvalue = (expectedtotalvaluewithouttax + travelledtotaltaxvalue + securetaxvalue + tripVM.NightCharges);
                                Decimal? totalvaluewithouttax = 0;
                                if (tripVM.IsTripCompByDriver == true)
                                {

                                    var extradiatancethanbaseLimit = Convert.ToDecimal(travelledonroaddistance) - Convert.ToDecimal(tripVM.KilometerLimit);

                                    var totalpriceforextradistance = extradiatancethanbaseLimit * tripVM.PricePerKilometers;

                                    var totalprice = totalpriceforextradistance + (tripVM.BasePrice * tripVM.NoOfHoursActual) + tripVM.Aerialpriceperkilometer;

                                    var actualcuponpercentagevalue = totalprice * tripVM.CuponPercentage / 100;

                                    totalvaluewithouttax = totalprice - actualcuponpercentagevalue;

                                }
                                tripVM.TotalTripValue = Convert.ToString(totalvaluewithouttax + travelledtotaltaxvalue + securetaxvalue + tripVM.NightCharges);


                            }
                            else
                            {
                                var expectedMinutes = tripVM.TotalMinutes;

                                var expectedPrice = tripVM.ChargesperMinute * Convert.ToDecimal(expectedMinutes);
                                var cuponpercentagevalue = expectedPrice * tripVM.CuponPercentage / 100;
                                var expectedTotalPricewithouttax = expectedPrice + tripVM.NightCharges - cuponpercentagevalue;


                                Decimal? securetaxvalue = 0;

                                if (tripVM.IsSecuredTrip == true)
                                {
                                    Outstationinsurence insurenceData = null;
                                    double percentage = 0.0;

                                    if (trip.TripTypeId == 3)
                                    {
                                        insurenceData = _context.Outstationinsurence.FirstOrDefault();

                                        if (insurenceData != null)
                                        {
                                            percentage = trip.TripvarientId switch
                                            {
                                                3 => insurenceData.Onewaypercentage,
                                                4 => insurenceData.Roundtrippercentage,
                                                _ => percentage
                                            };
                                        }
                                    }
                                    else
                                    {
                                        var insurenceTaxAndPriceData = _context.InsurenceTaxandPrice.FirstOrDefault();

                                        if (insurenceTaxAndPriceData != null)
                                        {
                                            percentage = Convert.ToDouble(insurenceTaxAndPriceData.TaxPercentage);
                                        }
                                    }

                                    if (insurenceData != null || _context.InsurenceTaxandPrice.FirstOrDefault() != null)
                                    {
                                        var taxpercentage = percentage;

                                        // Convert percentage to decimal before multiplication
                                        var insurencetaxvalue = expectedTotalPricewithouttax * (decimal)taxpercentage / 100;

                                        securetaxvalue = insurencetaxvalue;
                                    }
                                }

                                Decimal travelledtotaltaxvalue = 0;

                                var taxesdata = _context.Taxes.ToList();

                                if (taxesdata.Count > 0)
                                {
                                    foreach (var tax in taxesdata)
                                    {

                                        var taxpercentage = Convert.ToInt32(tax.Percentage);
                                        tripVM.Percentage = taxpercentage;
                                        var taxvalue = Convert.ToDecimal(expectedTotalPricewithouttax) * taxpercentage / 100;

                                        var travelledtaxvalue = Convert.ToDecimal(expectedTotalPricewithouttax) * taxpercentage / 100;

                                        travelledtotaltaxvalue += travelledtaxvalue;
                                    }
                                }
                                tripVM.ExpectedTotalTripvalue = (expectedTotalPricewithouttax + travelledtotaltaxvalue + securetaxvalue + tripVM.NightCharges);
                                Decimal? totalvaluewithouttax = 0;
                                if (tripVM.IsTripCompByDriver == true)
                                {

                                    var totalMinutes = tripVM.TotalMinutes;

                                    var price = tripVM.ChargesperMinute * Convert.ToDecimal(expectedMinutes);
                                    var totalPricewithouttax = price + tripVM.NightCharges - cuponpercentagevalue;

                                }
                                tripVM.TotalTripValue = Convert.ToString(totalvaluewithouttax + travelledtotaltaxvalue + securetaxvalue + tripVM.NightCharges);


                            }

                            tripVMList.Add(tripVM);

                        }

                    }
                    var flexiModelList = _context.Flexis.Where(c => c.DriverId == id && c.IsAccepted == true && c.IsCancelled != true && c.IsPaymentdone != true).ToList();
                    var flexiVmList = new List<FlexisVm>();

                    if (flexiModelList.Count > 0)
                    {
                        foreach (var flexiModel in flexiModelList)
                        {
                            var flexivm = new FlexisVm
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
                                Isonroute = flexiModel.Isonroute,
                                IsdriverArrived = flexiModel.IsdriverArrived,
                                TransmissionId = flexiModel.TransmissionId,
                                CloseTrip = flexiModel.CloseTrip,
                                UserId = flexiModel.UserId,

                                IsAccepted = flexiModel.IsAccepted,
                                IsCancelled = flexiModel.IsCancelled,
                                IsDriverAssigned = flexiModel.IsDriverAssigned,
                                IsProcessing = flexiModel.IsProcessing,
                                IsReserved = flexiModel.IsReserved,
                                EstimatedPrice = flexiModel.EstimatedPrice,
                                EstimatedTaxValue = flexiModel.EstimatedTaxValue,
                                EstimatedCuponPrice = flexiModel.EstimatedCuponPrice,
                                DriverId = flexiModel.DriverId,
                                IsAdvancePaid = flexiModel.IsAdvancePaid,
                                AdvancePaid = flexiModel.AdvancePaid,
                                NoofDays = flexiModel.NoofDays,
                                FlexiSelectedDateTime = flexiModel.FlexiSelectedDateTime,
                                selecteddateListvalue = flexiModel.selecteddateListvalue,


                            };

                            var vehicleTypedata = _context.VehicleTypes.Find(flexivm.VehicleTypeId);
                            if (vehicleTypedata != null)
                            {
                                flexivm.VehicleTypeName = vehicleTypedata.VehicleTypeName;
                            }

                            var transmissionData = _context.TransmissionTypes.Find(flexivm.TransmissionId);
                            if (transmissionData != null)
                            {
                                flexivm.TransmissionName = transmissionData.TransmissionName;
                            }

                            var userdata = _context.Users.Find(flexivm.UserId);
                            if (userdata != null)
                            {
                                flexivm.Name = userdata.Name;
                                flexivm.PhoneNumber = userdata.PhoneNumber;
                            }


                            var datelistvm = new List<FlexiDateListVM>();


                            var datelist = await _context.FlexiDatesLists
                                .FirstOrDefaultAsync(c => c.FlexiId == flexiModel.FlexiId && c.selecteddateListvalue == flexiModel.selecteddateListvalue);

                            if (datelist != null)
                            {
                                var datelistVM = new FlexiDateListVM
                                {
                                    FlexiDatesListId = datelist.FlexiDatesListId,
                                    Date = datelist.Date,
                                    FlexiId = datelist.FlexiId,
                                    IsTripCompByDriver = datelist.IsTripCompByDriver,
                                    ActualHours = datelist.ActualHours,
                                    ActualPrice = datelist.ActualPrice,
                                    ImageUrlsList = datelist.ImageUrlsList,
                                    ActualTaxValue = datelist.ActualTaxValue,
                                    IsDriverArrival = datelist.IsDriverArrival,
                                    CuponId = datelist.CuponId,
                                    ActualCuponPrice = datelist.ActualCuponPrice,
                                    IsTripStarted = datelist.IsTripStarted,
                                    IsAtDrop = datelist.IsAtDrop,
                                    selecteddateListvalue = datelist.selecteddateListvalue,
                                    Isonroute = datelist.Isonroute,
                                    Istakenpics = datelist.Istakenpics,
                                    IsEndPicsTaken = datelist.IsEndPicsTaken,
                                };
                                datelistvm.Add(datelistVM);
                            }


                            var datelist1 = await _context.FlexiDatesLists
                         .Where(c => c.FlexiId == flexiModel.FlexiId)
                         .OrderBy(c => c.Date) // Ensure dates are ordered
                         .ToListAsync();

                            var today = DateTime.Today;

                            // Get the last date from the list
                            var lastDate = datelist1.LastOrDefault()?.Date;

                            if (lastDate != null)
                            {
                                // Check if the last date is less than or equal to today
                                if (lastDate.Value.Date < today)
                                {

                                    flexivm.isalltripscompleted = true;
                                }
                                else if (lastDate.Value.Date == today)
                                {
                                    // If the last date is today, check if the last trip is completed
                                    bool isLastDateCompleted = datelist1.Last().IsTripCompByDriver.GetValueOrDefault(false);

                                    if (isLastDateCompleted)
                                    {
                                        // Check if all trips are completed
                                        bool isAllTripsCompleted = datelist1.All(t => t.IsTripCompByDriver.GetValueOrDefault(true));
                                        flexivm.isalltripscompleted = isAllTripsCompleted;
                                    }
                                    else
                                    {
                                        flexivm.isalltripscompleted = false;
                                    }
                                }
                                else
                                {
                                    // If the last date is greater than today, not all trips are completed
                                    flexivm.isalltripscompleted = false;
                                }
                            }
                            else
                            {
                                // If no dates are present, you can handle this case as needed
                                flexivm.isalltripscompleted = false;
                            }

                            flexivm.DateList = datelistvm; // Ensure you have a DateList property in FlexisVm

                            flexiVmList.Add(flexivm);
                        }
                    }
                    // Now you have flexiVmList populated with the necessary data

                    var monthlydatalist = await _context.Monthlies.Where(c => c.DriverId == id && c.IsAccepted == true && c.IsCancelled != true && c.IsPaymentdone != true).ToListAsync();
                    var monthlyVMList = new List<MonthlyVM>();
                    if (monthlydatalist.Count > 0)
                    {
                        foreach (var monthly in monthlydatalist)
                        {
                            var monthlyVM = new MonthlyVM();
                            monthlyVM.MonthlyId = monthly.MonthlyId;
                            monthlyVM.UniqueMonthlyId = monthly.UniqueMonthlyId;
                            monthlyVM.TripVarientId = monthly.TripVarientId;
                            monthlyVM.Isonroute = monthly.Isonroute;
                            monthlyVM.IsAccepted = monthly.IsAccepted;
                            monthlyVM.IsCancelled = monthly.IsCancelled;

                            monthlyVM.IsDriverAssigned = monthly.IsDriverAssigned;
                            monthlyVM.IsProcessing = monthly.IsProcessing;
                            monthlyVM.IsReserved = monthly.IsReserved;
                            monthlyVM.IsTripStarted = monthly.IsTripStarted;
                            monthlyVM.CloseTrip = monthly.CloseTrip;
                            monthlyVM.selecteddateListvalue = monthly.selecteddateListvalue;
                            var tripvariantdata = await _context.TripVariants.FindAsync(monthlyVM.TripVarientId);
                            if (tripvariantdata != null)
                            {
                                monthlyVM.TripVarientName = tripvariantdata.TripVariantName;
                            }

                            monthlyVM.PickUpLocation = monthly.PickUpLocation;

                            monthlyVM.PickUpLocationCoordinates = monthly.PickUpLocationCoordinates;

                            monthlyVM.DriverMeansOfTransport = monthly.DriverMeansOfTransport;

                            monthlyVM.NoofDays = monthly.NoofDays;

                            monthlyVM.Pickuptime = monthly.Pickuptime;

                            monthlyVM.VehicleTypeId = monthly.VehicleTypeId;
                            monthlyVM.IsdriverArrived = monthly.IsdriverArrived;
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

                            var datelist = await _context.MonthlyDateLists.FirstOrDefaultAsync(c => c.MonthlyId == monthly.MonthlyId && c.selecteddateListvalue == monthly.selecteddateListvalue);

                            if (datelist != null)
                            {
                                var dateVM = new MonthlyDateListVM
                                {
                                    MonthlyDateListId = datelist.MonthlyDateListId,
                                    Date = datelist.Date,
                                    MonthlyId = datelist.MonthlyId,
                                    IsTripCompByDriver = datelist.IsTripCompByDriver,
                                    ActualHours = datelist.ActualHours,
                                    ActualPrice = datelist.ActualPrice,
                                    ImageUrlsList = datelist.ImageUrlsList,
                                    IsDriverArrival = datelist.IsDriverArrival,
                                    ActualTaxValue = datelist.ActualTaxValue,
                                    CuponId = datelist.CuponId,
                                    IsAtDrop = datelist.IsAtDrop,
                                    ActualCuponPrice = datelist.ActualCuponPrice,
                                    IsTripStarted = datelist.IsTripStarted,
                                    Isonroute = datelist.Isonroute,
                                    selecteddateListvalue = datelist.selecteddateListvalue,
                                    Istakenpics = datelist.Istakenpics,
                                    IsEndPicsTaken = datelist.IsEndPicsTaken,
                                };

                                var datelist1 = await _context.MonthlyDateLists
                             .Where(c => c.MonthlyId == monthly.MonthlyId)
                             .OrderBy(c => c.Date) // Ensure dates are ordered
                             .ToListAsync();

                                var today = DateTime.Today;

                                // Get the last date from the list
                                var lastDate = datelist1.LastOrDefault()?.Date;

                                if (lastDate != null)
                                {
                                    // Check if the last date is less than or equal to today
                                    if (lastDate.Value.Date < today)
                                    {

                                        monthlyVM.isalltripscompleted = true;
                                    }
                                    else if (lastDate.Value.Date == today)
                                    {
                                        // If the last date is today, check if the last trip is completed
                                        bool isLastDateCompleted = datelist1.Last().IsTripCompByDriver.GetValueOrDefault(false);

                                        if (isLastDateCompleted)
                                        {
                                            // Check if all trips are completed
                                            bool isAllTripsCompleted = true;
                                            monthlyVM.isalltripscompleted = isAllTripsCompleted;
                                        }
                                        else
                                        {
                                            monthlyVM.isalltripscompleted = false;
                                        }
                                    }
                                    else
                                    {
                                        // If the last date is greater than today, not all trips are completed
                                        monthlyVM.isalltripscompleted = false;
                                    }
                                }
                                else
                                {
                                    // If no dates are present, you can handle this case as needed
                                    monthlyVM.isalltripscompleted = false;
                                }


                                datelistVM.Add(dateVM);
                            }

                            monthlyVM.DateList = datelistVM;

                            monthlyVMList.Add(monthlyVM);

                        }

                    }
                    return Ok(new { Trip = tripVMList, Flexi = flexiVmList, Monthly = monthlyVMList });
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

        [HttpGet("{id}/driverid")]
        public async Task<ActionResult<IEnumerable<TripVM>>> GetTrip(int id)
        {
            try
            {
                var tripdata = _context.Trips.Where(c => c.DriverId == id && (c.IsAccepted == true || c.IsTripStarted == true || c.IsReserved == true) && c.IsTripCompByDriver != true).ToList();

                var tripVMList = new List<TripVM>();

                if (tripdata.Count > 0)
                {

                    foreach (var trip in tripdata)
                    {
                        var tripVM = new TripVM();

                        tripVM.TripId = trip.TripId;
                        tripVM.IsATDrop = trip.IsAtDrop;

                        tripVM.UserId = trip.UserId;
                        tripVM.NoOfHoursSelected = trip.NoOfHoursSelected;
                        var userdata = _context.Users.Find(tripVM.UserId);

                        if (userdata != null)
                        {

                            tripVM.UserName = userdata.Name;

                            tripVM.UserPhoneNumber = userdata.PhoneNumber;

                            tripVM.Image = userdata.UserImage;
                        }

                        tripVM.DriverId = trip.DriverId;

                        var driverdata = _context.Drivers.Find(tripVM.DriverId);

                        if (driverdata != null)
                        {

                            tripVM.DriverName = driverdata.DriverName;

                            tripVM.DriverImage = driverdata.Image;

                            tripVM.Experience = driverdata.Experiance;
                        }

                        tripVM.PickUPMapURL = trip.PickUPMapURL;
                        tripVM.DropUPMapURL = trip.DropUPMapURL;
                        tripVM.IsTimeScheduled = trip.IsTimeScheduled;
                        tripVM.StartDate = trip.StartDateTime;
                        tripVM.FromLocationName = trip.FromLocationName;
                        tripVM.ToLocationName = trip.ToLocationName;
                        tripVM.EstimatedPrice = trip.EstimatedPrice.ToString();
                        tripVM.IsTripStarted = trip.IsTripStarted;

                        DateTime? startTime = tripVM.StartDate;

                        var starttime = String.Format("{0:h:mm tt}", startTime);

                        tripVM.StartTime = starttime;

                        tripVM.EndDate = trip.EndDateTime;

                        DateTime? endTime = tripVM.EndDate;

                        var endtime = String.Format("{0:h:mm tt}", endTime);
                        tripVM.EndTime = endtime;

                        tripVM.ActualEndDate = trip.ActualEndTime;

                        DateTime? actualendTime = tripVM.ActualEndDate;

                        var actualendtime = String.Format("{0:h:mm tt}", actualendTime);

                        tripVM.ActualEndTime = actualendtime;

                        if (tripVM.StartDate.HasValue && tripVM.ActualEndDate.HasValue)
                        {
                            TimeSpan duration = tripVM.ActualEndDate.Value - tripVM.StartDate.Value;

                            double minutes = duration.TotalMinutes;
                            tripVM.ActualTotalMinutes = minutes;
                        }

                        if (tripVM.StartDate.HasValue && tripVM.EndDate.HasValue)
                        {
                            TimeSpan duration = tripVM.EndDate.Value - tripVM.StartDate.Value;

                            double minutes = duration.TotalMinutes;

                            tripVM.TotalMinutes = minutes;
                        }

                        tripVM.RequestedDate = trip.RequstedDateTime;

                        DateTime? requestedTime = tripVM.RequestedDate;

                        var requestedtime = String.Format("{0:h:mm tt}", requestedTime);

                        tripVM.RequestedTime = requestedtime;

                        tripVM.TripTypeId = trip.TripTypeId;

                        tripVM.IsTripCompByDriver = trip.IsTripCompByDriver;

                        var tripTypeData = _context.TripTypes.Find(trip.TripTypeId);

                        if (tripTypeData != null)
                        {

                            tripVM.TripTypeName = tripTypeData.TripName;
                        }

                        tripVM.TripvarientId = trip.TripvarientId;

                        var triptypevariantdata = _context.TripVariants.Find(trip.TripvarientId);

                        if (triptypevariantdata != null)
                        {

                            tripVM.BasePrice = triptypevariantdata.BasePrice;

                            tripVM.KilometerLimit = triptypevariantdata.KilometerLimit;

                            tripVM.NightCharges = triptypevariantdata.NightCharges;

                            tripVM.ChargesperMinute = triptypevariantdata.ChargesperMinute;

                            tripVM.PricePerKilometers = triptypevariantdata.PricePerKilometers;

                            tripVM.IsTripOneway = triptypevariantdata.IsTripOneway;
                        }

                        tripVM.VehicleId = trip.VehicleId;

                        if (trip.VehicleId != 0)
                        {

                            var vehicledata = _context.Vehicles.Find(trip.VehicleId);

                            if (vehicledata != null)
                            {

                                tripVM.VehicleNo = vehicledata.VehicleNo;

                                if (!string.IsNullOrEmpty(vehicledata.Images))
                                {

                                    var images = vehicledata.Images.Split(',').ToList();

                                    var vehicleimagelist = new List<VehicleImages>();

                                    if (images.Count > 0)
                                    {

                                        foreach (var image in images)
                                        {

                                            var imagevm = new VehicleImages();

                                            imagevm.VechileImage = image;

                                            vehicleimagelist.Add(imagevm);
                                        }
                                    }

                                    tripVM.VehicleImages = vehicleimagelist;
                                }

                                tripVM.VehicleTypeId = vehicledata.VehicleTypeId;

                                if (tripVM.VehicleTypeId == 0)
                                {

                                    var vehicletypedata = _context.VehicleTypes.Find(tripVM.VehicleTypeId);

                                    if (vehicletypedata != null)
                                    {

                                        tripVM.VehicleTypeName = vehicletypedata.VehicleTypeName;
                                    }
                                }
                                tripVM.TransmissionTypeId = trip.TransmissionTypeId;
                                if (tripVM.TransmissionTypeId != 0 && tripVM.TransmissionTypeId != null)
                                {
                                    var transmissiontypedata = _context.TransmissionTypes.Find(tripVM.TransmissionTypeId);

                                    if (transmissiontypedata != null)
                                    {
                                        tripVM.TransmissionTypeName = transmissiontypedata.TransmissionName;
                                    }
                                }

                            }

                        }
                        if (!string.IsNullOrEmpty(trip.FromLocation))
                        {

                            var fromLocations = trip.FromLocation.Split(',').ToList();

                            if (fromLocations.Count > 0)
                            {

                                tripVM.FromLatitude = fromLocations[0];

                                tripVM.FromLongitude = fromLocations[1];

                            }
                        }
                        if (!string.IsNullOrEmpty(trip.ToLocation))
                        {

                            var toLocations = trip.ToLocation.Split(',').ToList();

                            if (toLocations.Count > 0)
                            {

                                tripVM.ToLatitude = toLocations[0];

                                tripVM.ToLongitude = toLocations[1];
                            }
                        }

                        int travelledonroaddistance = 0;

                        var drivertracking = _context.DriverTrackings.Where(c => c.DriverId == tripVM.DriverId && c.TripId == tripVM.TripId).ToList();

                        if (drivertracking.Count > 1)
                        {

                            var currentEntry = drivertracking.FirstOrDefault();

                            var nextEntry = drivertracking.Last();


                            Decimal? currentLatitude = currentEntry.Latitude;

                            Decimal? currentLongitude = currentEntry.Longitude;

                            Decimal? nextLatitude = nextEntry.Latitude;

                            Decimal? nextLongitude = nextEntry.Longitude;

                            var actualgoogledistanceresult = await GetCachedGoogleDistance(currentLatitude.ToString(), currentLongitude.ToString(), nextLatitude.ToString(), nextLongitude.ToString());

                            int actualgoogledistance = 0;
                            if (actualgoogledistanceresult == null)
                            {

                                actualgoogledistance = Convert.ToInt32(actualgoogledistanceresult.Value);
                            }
                            else
                            {

                                actualgoogledistance = Convert.ToInt32(actualgoogledistanceresult);
                            }

                            travelledonroaddistance += actualgoogledistance;

                        }

                        tripVM.DistanceTravelled = travelledonroaddistance;
                        var entityDistance = distance(Convert.ToDouble(tripVM.FromLatitude), Convert.ToDouble(tripVM.FromLongitude), Convert.ToDouble(tripVM.ToLatitude), Convert.ToDouble(tripVM.ToLongitude), 'K');


                        tripVM.AerialDistance = Math.Round(Convert.ToDecimal(entityDistance), 2);

                        var aerialDistancedata = _context.AerialDistancePrices.ToList();

                        if (aerialDistancedata.Count > 0)
                        {

                            tripVM.Aerialpriceperkilometer = Convert.ToDecimal(aerialDistancedata[0].AerialDistancePriceperKilometer);
                        }

                        tripVM.ExpectedDistance = trip.ExpectedDistance;

                        tripVM.CuponId = tripVM.CuponId;

                        var cupondata = _context.Cupons.Find(tripVM.CuponId);

                        if (cupondata != null)
                        {

                            tripVM.CuponName = cupondata.CuponName;

                            tripVM.CuponCode = cupondata.CuponCode;

                            tripVM.CuponPercentage = cupondata.Percentage;
                        }

                        Decimal? taxpercentages = 0;
                        var taxdata = await _context.Taxes.FirstOrDefaultAsync();
                        if (taxdata != null)
                        {
                            taxpercentages = taxdata.Percentage;

                        }

                        decimal? careprice = 0;
                        if (trip.TripTypeId != 3)
                        {

                            var insurencedata = await _context.InsurenceTaxandPrice.FirstOrDefaultAsync();
                            if (insurencedata != null)
                            {
                                var percentage = insurencedata.TaxPercentage;
                                var price = insurencedata.Price;
                                var value = price * (percentage / 100);

                                careprice = price + value;
                            }
                        }
                        else
                        {
                            var insurencedata = await _context.InsurenceTaxandPriceoutoffcity.FirstOrDefaultAsync();
                            if (insurencedata != null)
                            {
                                var percentage = insurencedata.TaxPercentage;
                                var price = insurencedata.Price;
                                var value = price * (percentage / 100);
                                careprice = price + value;
                            }
                        }
                        var driversubscriptiondata = await _context.Driversubscriptions
                               .Where(c => c.DriverId == id)
                               .OrderByDescending(c => c.DriversubscriptionId) // Sort by the latest date first
                               .FirstOrDefaultAsync(); // Get the latest entry
                        if (driversubscriptiondata != null)
                        {
                            trip.TotalTripValue = Math.Abs(Convert.ToDecimal(trip.EstimatedPrice));
                            decimal totalValue = Convert.ToDecimal(trip.EstimatedPrice);
                            var taxvalue = Convert.ToInt32(totalValue) * (taxpercentages / 100);
                            int? anonymuscharges = 0;
                            var anonymusdata = _context.Anonymoustripcharges.Where(c => c.TriptypeId == trip.TripTypeId).FirstOrDefault();
                            if (anonymusdata != null)
                            {
                                anonymuscharges = anonymusdata.Amount;
                            }

                            // Calculate drivervalue using decimal arithmetic to preserve precision
                            decimal drivervalue = (totalValue - Convert.ToDecimal(taxvalue) - Convert.ToDecimal(careprice) - Convert.ToInt32(anonymuscharges));
                            trip.DriversPrice = Math.Round(Convert.ToDecimal(drivervalue), 0);
                        }
                        else
                        {
                            trip.TotalTripValue = Math.Abs(Convert.ToDecimal(trip.EstimatedPrice));
                            decimal totalValue = Convert.ToDecimal(trip.EstimatedPrice);
                            var taxvalue = Convert.ToInt32(totalValue) * (taxpercentages / 100);


                            // Calculate drivervalue using decimal arithmetic to preserve precision
                            decimal drivervalue = (totalValue - Convert.ToDecimal(taxvalue) - Convert.ToDecimal(careprice)) * 80 / 100;
                            trip.DriversPrice = Math.Round(Convert.ToDecimal(drivervalue), 0);
                        }
                        tripVM.DriverPrice = trip.DriversPrice.ToString();
                        tripVM.IsSecuredTrip = trip.IsSecuredTrip;

                        if (tripVM.IsTripOneway == true)
                        {

                            var expectedextradiatancethanbaseLimit = Convert.ToDecimal(tripVM.ExpectedDistance) - Convert.ToDecimal(tripVM.KilometerLimit);

                            var expectedtotalpriceforextradistance = expectedextradiatancethanbaseLimit * tripVM.PricePerKilometers;

                            var expectedtotalprice = expectedtotalpriceforextradistance + (tripVM.BasePrice * tripVM.NoOfHoursSelected) + tripVM.Aerialpriceperkilometer;

                            var cuponpercentagevalue = expectedtotalprice * tripVM.CuponPercentage / 100;

                            var expectedtotalvaluewithouttax = expectedtotalprice - cuponpercentagevalue;

                            Decimal? securetaxvalue = 0;

                            if (tripVM.IsSecuredTrip == true)
                            {

                                var insurencedata = _context.InsurenceTaxandPrice.FirstOrDefault();

                                if (insurencedata != null)
                                {

                                    var taxpercentage = insurencedata.TaxPercentage;

                                    var insurencetaxvalue = expectedtotalvaluewithouttax * taxpercentage / 100;

                                    securetaxvalue = insurencetaxvalue;
                                }
                            }

                            Decimal travelledtotaltaxvalue = 0;

                            var taxesdata = _context.Taxes.ToList();

                            if (taxesdata.Count > 0)
                            {
                                foreach (var tax in taxesdata)
                                {

                                    var taxpercentage = Convert.ToInt32(tax.Percentage);

                                    var taxvalue = Convert.ToDecimal(expectedtotalvaluewithouttax) * taxpercentage / 100;

                                    var travelledtaxvalue = Convert.ToDecimal(expectedtotalvaluewithouttax) * taxpercentage / 100;

                                    travelledtotaltaxvalue += travelledtaxvalue;
                                }
                            }

                            tripVM.ExpectedTotalTripvalue = (expectedtotalvaluewithouttax + travelledtotaltaxvalue + securetaxvalue + tripVM.NightCharges);
                            Decimal? totalvaluewithouttax = 0;
                            if (tripVM.IsTripCompByDriver == true)
                            {

                                var extradiatancethanbaseLimit = Convert.ToDecimal(travelledonroaddistance) - Convert.ToDecimal(tripVM.KilometerLimit);

                                var totalpriceforextradistance = extradiatancethanbaseLimit * tripVM.PricePerKilometers;

                                var totalprice = totalpriceforextradistance + (tripVM.BasePrice * tripVM.NoOfHoursActual) + tripVM.Aerialpriceperkilometer;

                                var actualcuponpercentagevalue = totalprice * tripVM.CuponPercentage / 100;

                                totalvaluewithouttax = totalprice - actualcuponpercentagevalue;

                            }
                            tripVM.TotalTripValue = Convert.ToString(totalvaluewithouttax + travelledtotaltaxvalue + securetaxvalue + tripVM.NightCharges);


                        }
                        else
                        {
                            var expectedMinutes = tripVM.TotalMinutes;

                            var expectedPrice = tripVM.ChargesperMinute * Convert.ToDecimal(expectedMinutes);
                            var cuponpercentagevalue = expectedPrice * tripVM.CuponPercentage / 100;
                            var expectedTotalPricewithouttax = expectedPrice + tripVM.NightCharges - cuponpercentagevalue;


                            Decimal? securetaxvalue = 0;

                            if (tripVM.IsSecuredTrip == true)
                            {

                                var insurencedata = _context.InsurenceTaxandPrice.FirstOrDefault();

                                if (insurencedata != null)
                                {

                                    var taxpercentage = insurencedata.TaxPercentage;

                                    var insurencetaxvalue = expectedTotalPricewithouttax * taxpercentage / 100;

                                    securetaxvalue = insurencetaxvalue;
                                }
                            }

                            Decimal travelledtotaltaxvalue = 0;

                            var taxesdata = _context.Taxes.ToList();

                            if (taxesdata.Count > 0)
                            {
                                foreach (var tax in taxesdata)
                                {

                                    var taxpercentage = Convert.ToInt32(tax.Percentage);

                                    var taxvalue = Convert.ToDecimal(expectedTotalPricewithouttax) * taxpercentage / 100;

                                    var travelledtaxvalue = Convert.ToDecimal(expectedTotalPricewithouttax) * taxpercentage / 100;

                                    travelledtotaltaxvalue += travelledtaxvalue;
                                }
                            }
                            tripVM.ExpectedTotalTripvalue = (expectedTotalPricewithouttax + travelledtotaltaxvalue + securetaxvalue + tripVM.NightCharges);
                            Decimal? totalvaluewithouttax = 0;
                            if (tripVM.IsTripCompByDriver == true)
                            {

                                var totalMinutes = tripVM.TotalMinutes;

                                var price = tripVM.ChargesperMinute * Convert.ToDecimal(expectedMinutes);
                                var totalPricewithouttax = price + tripVM.NightCharges - cuponpercentagevalue;

                            }
                            tripVM.TotalTripValue = Convert.ToString(totalvaluewithouttax + travelledtotaltaxvalue + securetaxvalue + tripVM.NightCharges);


                        }

                        tripVMList.Add(tripVM);

                    }

                    var flexiModelList = _context.Flexis.Where(c => c.DriverId == id && c.CloseTrip != true).ToList();
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

                            flexivm.UserId = flexiModel.UserId;
                            var userdata = _context.Users.Find(flexivm.UserId);
                            if (userdata != null)
                            {
                                flexivm.Name = userdata.Name;
                                flexivm.PhoneNumber = userdata.PhoneNumber;


                            }

                            flexivm.IsDriverAssigned = flexiModel.IsDriverAssigned;
                            flexivm.EstimatedPrice = flexiModel.EstimatedPrice;
                            flexivm.EstimatedTaxValue = flexiModel.EstimatedTaxValue;
                            flexivm.EstimatedCuponPrice = flexiModel.EstimatedCuponPrice;
                            flexivm.DriverId = flexiModel.DriverId;
                            flexivm.IsAdvancePaid = flexiModel.IsAdvancePaid;
                            flexivm.AdvancePaid = flexiModel.AdvancePaid;
                            flexivm.NoofDays = flexiModel.NoofDays;
                            flexivm.FlexiSelectedDateTime = flexiModel.FlexiSelectedDateTime;
                            var datelistvm = new List<FlexiDateListVM>();
                            var datelists = await _context.FlexiDatesLists
                                .Where(c => c.FlexiId == flexiModel.FlexiId)
                                .ToListAsync();

                            if (datelists.Count > 0)
                            {
                                foreach (var datelist in datelists)
                                {
                                    var datelistVM = new FlexiDateListVM
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
                                        IsTripStarted = datelist.IsTripStarted
                                    };
                                    datelistvm.Add(datelistVM);
                                }
                            }

                            flexiVmList.Add(flexivm); ;
                        }
                    }

                    var monthlydatalist = await _context.Monthlies.Where(c => c.DriverId == id && c.CloseTrip != true).ToListAsync();
                    var monthlyVMList = new List<MonthlyVM>();
                    if (monthlydatalist.Count > 0)
                    {
                        foreach (var monthly in monthlydatalist)
                        {
                            var monthlyVM = new MonthlyVM();
                            monthlyVM.MonthlyId = monthly.MonthlyId;
                            monthlyVM.UniqueMonthlyId = monthly.UniqueMonthlyId;
                            monthlyVM.TripVarientId = monthly.TripVarientId;

                            var tripvariantdata = await _context.TripVariants.FindAsync(monthlyVM.TripVarientId);
                            if (tripvariantdata != null)
                            {
                                monthlyVM.TripVarientName = tripvariantdata.TripVariantName;
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

                    }
                    return Ok(new { Trip = tripVMList, Flexi = flexiVmList, Monthly = monthlyVMList });
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
        private async Task<int?> GetGoogleDistance(string fromLatitude, string fromLongitude, string toLatitude, string toLongitude)
        {
            // Combine the coordinates into a single string to use as a cache key
            var cacheKey = $"{fromLatitude},{fromLongitude},{toLatitude},{toLongitude}";

            // Check if the result is in the cache
            if (googleDistanceCache.TryGetValue(cacheKey, out int cachedDistance))
            {
                // Return the cached result if available
                return cachedDistance;
            }

            // If not in cache, proceed to make the API call
            string apiKey = "AIzaSyCKkBWbhsJgwsPBxSC2IHOnnAVdmymFvPs";
            string origin = $"{fromLatitude},{fromLongitude}";
            string destination = $"{toLatitude},{toLongitude}";
            string apiUrl = $"https://maps.googleapis.com/maps/api/distancematrix/json?origins={origin}&destinations={destination}&key={apiKey}&traffic_model=best_guess&departure_time=now";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(responseBody);
                    int distance = (int)json["rows"][0]["elements"][0]["distance"]["value"];

                    // Store the result in the cache before returning
                    googleDistanceCache[cacheKey] = distance;
                    return distance;
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    return null; // Or handle the error appropriately
                }
            }
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

        private async Task<int?> GetCachedGoogleDistance(string fromLatitude, string fromLongitude, string toLatitude, string toLongitude)
        {
            try
            {
                var cacheKey = $"{fromLatitude},{fromLongitude},{toLatitude},{toLongitude}";
                // Simulate API call and response handling
                var apiResponse = await GetGoogleDistance(fromLatitude, fromLongitude, toLatitude, toLongitude);
                Console.WriteLine($"API Response: {apiResponse}");

                // Assuming apiResponse contains the distance which needs to be stored
                var distance = apiResponse; // Implement this based on your API response structure

                // Now store the result in the cache
                googleDistanceCache[cacheKey] = Convert.ToInt32(distance);

                return Convert.ToInt32(distance);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calling Google Distance Matrix API: {ex.Message}");
                return 0;
            }

        }

    }




}
