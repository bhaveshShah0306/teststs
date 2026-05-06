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
    using Newtonsoft.Json.Linq;
    using Org.BouncyCastle.Asn1.X509;
    using MailKit.Net.Imap;
    using Nest;
    using GoChauffeurWebApi.Interface;

    namespace GoChauffeurWebApi.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class TripsController : ControllerBase
        {
            private readonly DataContext _context;
            private Dictionary<string, int> googleDistanceCache = new Dictionary<string, int>();
            private readonly IFCMService _fcmService;
            public TripsController(DataContext context, IFCMService fcmService)
            {
                _context = context;
                _fcmService = fcmService;
            }


            // GET: api/Trips
            [HttpGet]
            public async Task<ActionResult<IEnumerable<TripVM>>> GetTrips()
            {
                try
                {
                    var tripdata = _context.Trips.ToList();

                    var tripVMList = new List<TripVM>();


                    if (tripdata.Count > 0)
                    {

                        foreach (var trip in tripdata)
                        {
                            var tripVM = new TripVM();
                            tripVM.TripId = trip.TripId;
                            tripVM.FromLocation = trip.FromLocation;
                            tripVM.ToLocation = trip.ToLocation;
                            tripVM.FromLocationName = trip.FromLocationName;
                            tripVM.ToLocationName = trip.ToLocationName;
                            tripVM.NoOfHoursActual = trip.NoOfHoursActual;
                            tripVM.IsProcessing = trip.IsProcessing;
                            tripVM.IsdriverArrived = trip.IsdriverArrived;
                            tripVM.IsAccepted = trip.IsAccepted;
                            tripVM.IsReserved = trip.IsReserved;
                            tripVM.IsAccepted = trip.IsAccepted;
                            tripVM.IsCancelled = trip.IsCancelled;
                            tripVM.TripsUniqueId = trip.TripsUniqueId;
                            tripVM.UserId = trip.UserId;
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
                            tripVM.TripStatus = trip.TripStatus;
                            var tripstatusdata = _context.TripStatuses.Find(tripVM.TripStatus);
                            if (tripstatusdata != null)
                            {
                                tripVM.TripStatusName = tripstatusdata.TripStatusName;
                            }

                            tripVM.PickUPMapURL = trip.PickUPMapURL;
                            tripVM.DropUPMapURL = trip.DropUPMapURL;
                            tripVM.IsTimeScheduled = trip.IsTimeScheduled;
                            tripVM.StartDate = trip.StartDateTime;

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




                            var entityDistance = distance(Convert.ToDouble(tripVM.FromLatitude), Convert.ToDouble(tripVM.FromLongitude), Convert.ToDouble(tripVM.ToLatitude), Convert.ToDouble(tripVM.ToLongitude), 'K');


                            tripVM.AerialDistance = Convert.ToDecimal(entityDistance);

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
                                tripVM.TotalTripValue = (totalvaluewithouttax + travelledtotaltaxvalue + securetaxvalue + tripVM.NightCharges).ToString();


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
                                tripVM.TotalTripValue = (totalvaluewithouttax + travelledtotaltaxvalue + securetaxvalue + tripVM.NightCharges).ToString();


                            }
                            if (tripVM.IsCancelled == true)
                            {
                                tripVM.TripStatus = 4;
                            }
                            if (trip.IsAccepted == true && trip.IsCancelled != true && trip.IsTripCompByDriver != true)
                            {
                                tripVM.TripStatus = 2;
                            }
                            if (trip.IsTripCompByDriver == true)
                            {
                                tripVM.TripStatus = 3;
                            }
                            if (trip.IsTimeScheduled == true && trip.StartDateTime == DateTime.Now)
                            {
                                tripVM.TripStatus = 1;


                            }
                            tripVMList.Add(tripVM);
                        }
                        var flexiModelList = _context.Flexis.ToList();
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
                                if (flexiModel.IsCancelled == true)
                                {
                                    flexivm.TripStatus = 4;
                                }
                                else if (flexiModel.IsAccepted == true && flexiModel.IsCancelled != true && flexiModel.IsTripCompByDriver != true)
                                {
                                    flexivm.TripStatus = 2;
                                }
                                else if (flexiModel.IsTripCompByDriver == true)
                                {
                                    flexivm.TripStatus = 3;
                                }
                                else
                                {
                                    flexivm.TripStatus = 1;
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
                                if (monthly.IsCancelled == true)
                                {
                                    monthlyVM.TripStatus = 4;
                                }
                                else if (monthly.IsAccepted == true && monthly.IsCancelled != true && monthly.IsTripCompByDriver != true)
                                {
                                    monthlyVM.TripStatus = 2;
                                }
                                else if (monthly.IsTripCompByDriver == true)
                                {
                                    monthlyVM.TripStatus = 3;
                                }
                                else
                                {
                                    monthlyVM.TripStatus = 1;
                                }
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

            // GET: api/Trips
            [HttpGet("Users/{id}/{tripTypeId}")]
            public async Task<ActionResult<IEnumerable<TripVM>>> GetTrips(int id, int tripTypeId)
            {
                try
                {
                    var tripdata = new List<Trip>();
                    if (tripTypeId == 0)
                    {

                        tripdata = _context.Trips.Where(x => x.UserId == id).ToList();
                    }
                    else
                    {
                        tripdata = _context.Trips.Where(x => x.UserId == id && x.TripTypeId == tripTypeId).ToList();
                    }

                    var tripVMList = new List<TripVM>();

                    if (tripdata.Count > 0)
                    {

                        foreach (var trip in tripdata)
                        {
                            var tripVM = new TripVM();

                            tripVM.TripId = trip.TripId;

                            tripVM.UserId = trip.UserId;
                            tripVM.TripsUniqueId = trip.TripsUniqueId;
                            var userdata = _context.Users.Find(tripVM.UserId);

                            if (userdata != null)
                            {

                                tripVM.UserName = userdata.Name;

                                tripVM.UserPhoneNumber = userdata.PhoneNumber;

                                tripVM.Image = userdata.UserImage;
                            }
                            tripVM.TripStatus = trip.TripStatus;
                            var tripstatusdata = _context.TripStatuses.Find(tripVM.TripStatus);
                            {
                                tripVM.TripStatusName = tripstatusdata.TripStatusName;
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
                                TimeSpan duration = tripVM.StartDate.Value - tripVM.EndDate.Value;

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
                                for (int i = 0; i < drivertracking.Count - 1; i++)
                                {

                                    var currentEntry = drivertracking[i];

                                    var nextEntry = drivertracking[i + 1];


                                    Decimal? currentLatitude = currentEntry.Latitude;

                                    Decimal? currentLongitude = currentEntry.Longitude;

                                    Decimal? nextLatitude = nextEntry.Latitude;

                                    Decimal? nextLongitude = nextEntry.Longitude;

                                    var actualgoogledistanceresult = GetGoogleDistance(currentLatitude.ToString(), currentLatitude.ToString(), nextLatitude.ToString(), nextLongitude.ToString());

                                    int actualgoogledistance = Convert.ToInt32(actualgoogledistanceresult);

                                    travelledonroaddistance += actualgoogledistance;

                                    Console.WriteLine($"Pair {i + 1}: ({currentLatitude}, {currentLongitude}) - ({nextLatitude}, {nextLongitude})");
                                }
                            }

                            var entityDistance = distance(Convert.ToDouble(tripVM.FromLatitude), Convert.ToDouble(tripVM.FromLongitude), Convert.ToDouble(tripVM.ToLatitude), Convert.ToDouble(tripVM.ToLongitude), 'K');


                            tripVM.AerialDistance = Convert.ToDecimal(entityDistance);

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
                                tripVM.TotalTripValue = (totalvaluewithouttax + travelledtotaltaxvalue + securetaxvalue + tripVM.NightCharges).ToString();


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
                                tripVM.TotalTripValue = (totalvaluewithouttax + travelledtotaltaxvalue + securetaxvalue + tripVM.NightCharges).ToString();


                            }

                            tripVMList.Add(tripVM);
                        }
                        return Ok(tripVMList);
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


            [HttpGet("{fromLatitude}/{fromLongitude}/{toLatitude}/{toLongitude}")]
            public async Task<ActionResult<dynamic>> GetGoogleDistance(string fromLatitude, string fromLongitude, string toLatitude, string toLongitude)
            {
                // Replace "YOUR_API_KEY" with your actual Google Maps API key
                string apiKey = "AIzaSyCKkBWbhsJgwsPBxSC2IHOnnAVdmymFvPs";

                // Replace the coordinates with your actual latitude and longitude values
                string origin = fromLatitude + ',' + fromLongitude; // San Francisco, CA
                string destination = toLatitude + ',' + toLongitude; // Los Angeles, CA

                // Construct the request URL
                string apiUrl = $"https://maps.googleapis.com/maps/api/distancematrix/json?origins={origin}&destinations={destination}&key={apiKey}&traffic_model=best_guess&departure_time=now";

                // Make the request to the Google Maps API
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();

                        // Use Newtonsoft.Json to parse the JSON response
                        JObject json = JObject.Parse(responseBody);

                        // Extract the distance from the JSON
                        int distance = (int)json["rows"][0]["elements"][0]["distance"]["value"];
                        int durationInTraffic = (int)json["rows"][0]["elements"][0]["duration_in_traffic"]["value"];
                        return Ok(distance);
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                        return BadRequest($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
                return NoContent();
            }
            // GET: api/Trips/5
            [HttpGet("{id:int}")]
            public async Task<ActionResult<TripVM>> GetTrip(int id)
            {
                try
                {
                    var trip = _context.Trips.Find(id);
                    var tripVM = new TripVM();
                    if (trip != null)
                    {
                        tripVM.TripId = trip.TripId;

                        tripVM.UserId = trip.UserId;
                        //tripVM.IsATDrop = trip.;
                        var userdata = _context.Users.Find(tripVM.UserId);

                        if (userdata != null)
                        {

                            tripVM.UserName = userdata.Name;

                            tripVM.UserPhoneNumber = userdata.PhoneNumber;

                            tripVM.Image = userdata.UserImage;
                        }
                        tripVM.TripStatus = trip.TripStatus;
                        var tripstatusdata = _context.TripStatuses.Find(tripVM.TripStatus);
                        {
                            tripVM.TripStatusName = tripstatusdata.TripStatusName;
                        }
                        tripVM.DriverId = trip.DriverId;
                        tripVM.ImageUrlsList = trip.ImageUrlsList;
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
                        tripVM.VehicleTypeId = trip.VehicleTypeId;
                        tripVM.IsProcessing = trip.IsProcessing;
                        tripVM.IsdriverArrived = trip.IsdriverArrived;
                        tripVM.IsAccepted = trip.IsAccepted;
                        tripVM.IsReserved = trip.IsReserved;
                        tripVM.IsAccepted = trip.IsAccepted;
                        tripVM.TripsUniqueId = trip.TripsUniqueId;
                        tripVM.IsCancelled = trip.IsCancelled;
                        tripVM.Isonroute = trip.Isonroute;
                        tripVM.IsSelfBook = trip.IsSelfBook;
                        tripVM.IsTripStarted = trip.IsTripStarted;
                        if (trip.IsSecuredTrip == true)
                        {
                            if (trip.TripTypeId != 3)
                            {

                                var insurendedata = _context.InsurenceTaxandPrice.FirstOrDefault();
                                if (insurendedata != null)
                                {
                                    var price = insurendedata.Price;
                                    var percentage = insurendedata.TaxPercentage;
                                    var taxvalue = price * percentage / 100;
                                    tripVM.Insurenceprice = price + taxvalue;
                                }
                            }
                            else
                            {
                                if (trip.TripTypeId == 3 && trip.TripvarientId == 3)
                                {
                                    var insurendedata = await _context.Outstationinsurence.FirstOrDefaultAsync();
                                    if (insurendedata != null)
                                    {
                                        var price = insurendedata.onewayPrice;
                                        var percentage = insurendedata.Onewaypercentage;
                                        var taxvalue = price * percentage / 100;
                                        tripVM.Insurenceprice = price + taxvalue;
                                    }
                                }
                                if (trip.TripTypeId == 3 && trip.TripvarientId == 4)
                                {
                                    var insurendedata = await _context.Outstationinsurence.FirstOrDefaultAsync();
                                    if (insurendedata != null)
                                    {
                                        var price = insurendedata.RoundtripPrice;
                                        var percentage = insurendedata.Roundtrippercentage;
                                        var taxvalue = price * percentage / 100;
                                        tripVM.Insurenceprice = price + taxvalue;
                                    }
                                }

                            }
                        }
                        tripVM.UserId = trip.UserId;
                        if (tripVM.VehicleTypeId != 0 && tripVM.VehicleTypeId != null)
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
                        tripVM.StartDate = trip.StartDateTime;
                        tripVM.FromLocationName = trip.FromLocationName;
                        tripVM.ToLocationName = trip.ToLocationName;
                        tripVM.NoOfHoursSelected = trip.NoOfHoursSelected;
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
                            TimeSpan duration = tripVM.StartDate.Value - tripVM.ActualEndDate.Value;

                            double minutes = Math.Abs(duration.TotalMinutes);
                            tripVM.ActualTotalMinutes = minutes;
                        }

                        if (tripVM.StartDate.HasValue && tripVM.EndDate.HasValue)
                        {
                            TimeSpan duration = tripVM.StartDate.Value - tripVM.EndDate.Value;

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
                                tripVM.VehicleName = vehicledata.VehicleName;

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

                                if (tripVM.VehicleTypeId != 0 && tripVM.VehicleTypeId != null)
                                {

                                    var vehicletypedata = _context.VehicleTypes.Find(tripVM.VehicleTypeId);

                                    if (vehicletypedata != null)
                                    {

                                        tripVM.VehicleTypeName = vehicletypedata.VehicleTypeName;
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
                            for (int i = 0; i < drivertracking.Count - 1; i++)
                            {

                                var currentEntry = drivertracking[i];

                                var nextEntry = drivertracking[i + 1];


                                string currentLatitudeStr = currentEntry.Latitude.ToString();
                                string currentLongitudeStr = currentEntry.Longitude.ToString();
                                string nextLatitudeStr = nextEntry.Latitude.ToString();
                                string nextLongitudeStr = nextEntry.Longitude.ToString();

                                // Call the GetGoogleDistance method and await its result
                                int? googleDistanceResult = await GetCachedGoogleDistance(currentLatitudeStr, currentLongitudeStr, nextLatitudeStr, nextLongitudeStr);

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
                        }

                        var entityDistance = distance(Convert.ToDouble(tripVM.FromLatitude), Convert.ToDouble(tripVM.FromLongitude), Convert.ToDouble(tripVM.ToLatitude), Convert.ToDouble(tripVM.ToLongitude), 'K');


                        tripVM.AerialDistance = Convert.ToDecimal(entityDistance);

                        var aerialDistancedata = _context.AerialDistancePrices.ToList();

                        if (aerialDistancedata.Count > 0)
                        {

                            tripVM.Aerialpriceperkilometer = Convert.ToDecimal(aerialDistancedata[0].AerialDistancePriceperKilometer);
                        }

                        tripVM.ExpectedDistance = trip.ExpectedDistance;

                        tripVM.CuponId = trip.CuponId;

                        var cupondata = _context.Cupons.Find(tripVM.CuponId);

                        if (cupondata != null)
                        {

                            tripVM.CuponName = cupondata.CuponName;

                            tripVM.CuponCode = cupondata.CuponCode;

                            tripVM.CuponPercentage = cupondata.Percentage;
                            if (tripVM.CuponPercentage == null || tripVM.CuponPercentage == 0)
                            {
                                tripVM.CuponPercentage = 0;
                            }
                        }
                        else
                        {
                            tripVM.CuponPercentage = 0;
                        }
                        tripVM.NoOfHoursActual = trip.NoOfHoursSelected;


                        tripVM.IsSecuredTrip = trip.IsSecuredTrip;


                        if (tripVM.IsTripOneway == true)
                        {

                            var expectedextradiatancethanbaseLimit = Convert.ToDecimal(tripVM.KilometerLimit) - Convert.ToDecimal(tripVM.ExpectedDistance);

                            var expectedtotalpriceforextradistance = expectedextradiatancethanbaseLimit * tripVM.PricePerKilometers;

                            var expectedtotalprice = expectedtotalpriceforextradistance + (tripVM.BasePrice * tripVM.NoOfHoursSelected) + tripVM.Aerialpriceperkilometer;

                            var cuponpercentagevalue = expectedtotalprice * tripVM.CuponPercentage / 100;

                            var expectedtotalvaluewithouttax = expectedtotalprice - cuponpercentagevalue;

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

                                totalvaluewithouttax = totalprice - actualcuponpercentagevalue ?? totalprice;

                            }
                            var anonymuscharges = await _context.Anonymoustripcharges.Where(c => c.TriptypeId == trip.TripTypeId).FirstOrDefaultAsync();
                            if (anonymuscharges != null)
                            {
                                tripVM.Anonymuscharge = anonymuscharges.Amount.ToString();
                            }
                            tripVM.TotalTripValue = (totalvaluewithouttax + travelledtotaltaxvalue + securetaxvalue + tripVM.NightCharges).ToString();
                            if (Convert.ToDecimal(trip.EstimatedPrice) > Convert.ToDecimal(tripVM.TotalTripValue))
                            {
                                tripVM.TotalTripValue = trip.EstimatedPrice.ToString();
                            }


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
                                decimal? careprice = 0;
                                if (trip.TripTypeId != 3)
                                {

                                    var insurencedata = await _context.InsurenceTaxandPrice.FirstOrDefaultAsync();
                                    if (insurencedata != null)
                                    {
                                        var percentage = insurencedata.TaxPercentage;
                                        var prices = insurencedata.Price;
                                        var value = prices * (percentage / 100);

                                        careprice = prices + value;
                                    }
                                }
                                else
                                {
                                    var insurencedata = await _context.InsurenceTaxandPriceoutoffcity.FirstOrDefaultAsync();
                                    if (insurencedata != null)
                                    {
                                        var percentage = insurencedata.TaxPercentage;
                                        var prices = insurencedata.Price;
                                        var value = prices * (percentage / 100);
                                        careprice = prices + value;
                                    }
                                }

                                var totalMinutes = tripVM.TotalMinutes;

                                var price = tripVM.ChargesperMinute * Convert.ToDecimal(expectedMinutes);
                                var totalPricewithouttax = price + tripVM.NightCharges - cuponpercentagevalue - careprice;
                                var taxvalue2 = await _context.TripTaxes.FirstOrDefaultAsync();
                                if (taxvalue2 != null)
                                {
                                    // Assuming tripVM.TotalTripValue is a string that needs to be converted to a double
                                    var totalTripValue = Convert.ToDouble(totalPricewithouttax);
                                    tripVM.triptaxvalue = Convert.ToDecimal(totalTripValue) * taxvalue2.Percentage / 100;
                                }

                            }
                            tripVM.TotalTripValue = (totalvaluewithouttax + travelledtotaltaxvalue + securetaxvalue + tripVM.NightCharges).ToString();
                            if (Convert.ToDecimal(trip.EstimatedPrice) < Convert.ToDecimal(tripVM.TotalTripValue))
                            {
                                tripVM.TotalTripValue = tripVM.ExpectedTotalTripvalue.ToString();
                            }
                        }
                        //var taxvalue1 = await _context.TripTaxes.FirstOrDefaultAsync();
                        //if (taxvalue1 != null)
                        //{
                        //    // Assuming tripVM.TotalTripValue is a string that needs to be converted to a double
                        //    var totalTripValue = Convert.ToDouble(tripVM.TotalTripValue);
                        //    tripVM.triptaxvalue = Convert.ToDecimal(totalTripValue) * taxvalue1.Percentage / 100;
                        //}
                        return Ok(tripVM);
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

		    [HttpGet("Bookings")]
		    public async Task<IActionResult> GetBookingsPaginated([FromQuery(Name = "type")] string type = "All",
    [FromQuery(Name = "status")] string status = "All",
    string search = "",
    int pageNumber = 1,
    int pageSize = 10)
		    {
			    try
			    {
				    var query = _context.Trips.AsQueryable();

				    // Filter by Trip Type
				    if (!string.IsNullOrEmpty(type) && type != "All")
				    {
					    var tripType = await _context.TripTypes
						    .FirstOrDefaultAsync(t => t.TripName == type);
					    if (tripType != null)
						    query = query.Where(t => t.TripTypeId == tripType.TripTypeId);
				    }

				    // Filter by Status
				    if (!string.IsNullOrEmpty(status) && status != "All")
				    {
					    var tripStatus = await _context.TripStatuses
						    .FirstOrDefaultAsync(s => s.TripStatusName == status);
					    if (tripStatus != null)
						    query = query.Where(t => t.TripStatus == tripStatus.TripStatusId);
				    }

				    // Search by location or user name
				    if (!string.IsNullOrEmpty(search))
				    {
					    query = query.Where(t =>
						    t.FromLocationName.Contains(search) ||
						    t.ToLocationName.Contains(search));
				    }

				    // Total count before pagination
				    var totalRecords = await query.CountAsync();

					var trips = await query
						.OrderByDescending(t => t.TripId)
						.Skip((pageNumber - 1) * pageSize)
						.Take(pageSize)
						.Select(t => new
						{
							t.TripId,
							t.DriverId,
							t.StartDateTime,
							t.FromLocationName,
							t.ToLocationName,
							t.ExpectedDistance,
							t.IsCancelled,
							t.IsProcessing,
							t.IsAccepted,
							t.IsTripCompByDriver,
							t.TripTypeId,
							t.VehicleTypeId,
							t.TransmissionTypeId,
							t.PaymentTypeId,
							t.NoOfHoursActual,
							t.NoOfHoursSelected,
							t.ActualEndTime,
							t.EndDateTime,
							t.TotalTripValue,     
							t.EstimatedPrice
						})
						.ToListAsync();



				var tripsVMList = new List<UserTripsVM>();

				    foreach (var trip in trips)
				    {
					    var tripVM = new UserTripsVM
					    {
						    TripId = trip.TripId,
						    DriverId = trip.DriverId,
						    FromTime = trip.StartDateTime,
						    PickupLocation = trip.FromLocationName,
						    DropLocation = trip.ToLocationName,
						    Distance = trip.ExpectedDistance,
						    IsCancelled = trip.IsCancelled,
						    IsProcessing = trip.IsProcessing,
						    IsAccepted = trip.IsAccepted,
						    Istripcompleted = trip.IsTripCompByDriver
					    };

                        if (trip.TripTypeId != null && trip.TripTypeId != 0)
                        {
                            var triptypedata = _context.TripTypes.Find(trip.TripTypeId);
                            if (triptypedata != null)
                                tripVM.TripTypeName = triptypedata.TripName; 
                        }

                        if (trip.VehicleTypeId != null && trip.VehicleTypeId != 0)
					    {
                            var vehicletypedata = _context.VehicleTypes.Find(trip.VehicleTypeId);
                            if (vehicletypedata != null)
                                tripVM.VehicleTypeName = vehicletypedata.VehicleTypeName; 
                        }

                        if (trip.TransmissionTypeId != null && trip.TransmissionTypeId != 0)
                        {
                            var transmissiontypedata = _context.TransmissionTypes.Find(trip.TransmissionTypeId);
                            if (transmissiontypedata != null)
                                tripVM.TransmissionTypeName = transmissiontypedata.TransmissionName; 
                        }

					    if (trip.IsTripCompByDriver == true)
					    {
						    tripVM.TotalTripValue = trip.TotalTripValue;
						    tripVM.Hours = trip.NoOfHoursActual;
						    tripVM.ToTime = trip.ActualEndTime;
					    }
					    else
					    {
						    tripVM.TotalTripValue = trip.EstimatedPrice;
						    tripVM.Hours = trip.NoOfHoursSelected;
						    tripVM.ToTime = trip.EndDateTime;
					    }

                        if (trip.PaymentTypeId != null && trip.PaymentTypeId != 0)
                        {
                            var paymenttypedata = _context.PaymentTypes.Find(trip.PaymentTypeId);
                            if (paymenttypedata != null)
                                tripVM.PaymentType = paymenttypedata.PaymentTypeName; 
                        }

					    tripsVMList.Add(tripVM);
				    }

				    return Ok(new
				    {
					    totalRecords,
					    pageNumber,
					    pageSize,
					    totalPages = (int)Math.Ceiling((double)totalRecords / pageSize),
					    data = tripsVMList
				    });
			    }
			    catch (Exception ex)
			    {
				    return BadRequest(ex.Message);
			    }
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


            // PUT: api/Trips/5
            // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
            [HttpPut("{id:int}")]
            public async Task<IActionResult> PutTrip(int id, Trip trip)
            {
                if (id != trip.TripId)
                {
                    return BadRequest();
                }

                _context.Entry(trip).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TripExists(id))
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

            // POST: api/Trips
            // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
            [HttpPost]
            public async Task<ActionResult<Trip>> PostTrip(Trip trip)
            {
                if (_context.Trips == null)
                {
                    return Problem("Entity set 'DataContext.Trips'  is null.");
                }
                var existingTrip = await _context.Trips
    .Where(t => t.UserId == trip.UserId
                && t.IsAccepted == true          // Trip accepted by a driver
                && t.IsTripCompByDriver != true  // Not completed
                && t.IsCancelled != true)        // Not cancelled
    .FirstOrDefaultAsync();

                if (existingTrip != null)
                {
                    return BadRequest("You already have an active trip that must be completed before booking a new one.");
                }

                trip.IsTripStarted = false;
                trip.IsTripCompByDriver = false;
                trip.IsdriverArrived = false;
                if (trip.StartDateTime.HasValue)
                {
                    TimeZoneInfo istTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                    DateTime istStartDateTime = TimeZoneInfo.ConvertTimeFromUtc(trip.StartDateTime.Value, istTimeZone);

                    trip.StartDateTime = istStartDateTime;
                }
                var numberofhours = trip.NoOfHoursSelected;

                if (numberofhours != null)
                {
                    var endtime = trip.StartDateTime?.AddHours(Convert.ToDouble(numberofhours));
                    trip.EndDateTime = endtime;
                }
                // Generate unique flexi code
                string tripTypeId = trip.TripTypeId.ToString().PadLeft(1, '0');
                string currentMonth = DateTime.Today.Month.ToString().PadLeft(2, '0');
                string currentDay = DateTime.Today.Day.ToString().PadLeft(2, '0');
                var tripCode = $"Go{tripTypeId}{currentDay}{currentMonth}";
                trip.TripsUniqueId = tripCode;

                //--------hours charges getting from DB
                int selectedHours = trip.NoOfHoursSelected ?? 0;
                var hourRate = await _context.Hours.FirstOrDefaultAsync(h => h.HoursName == selectedHours);
                int baseHourCharge = hourRate?.Charges ?? 0;
                trip.hoursCharge = baseHourCharge;

                // Charges from frontend
                //int baseHourCharge = trip.hoursCharge ?? 0;
                int returnCharge = trip.driverReturnCharges ?? 0;
                int nightCharge = trip.nightCharge ?? 0;

                //int couponAmount = 0;
                //if (trip.CuponId != null)
                //{
                //    var coupon = await _context.Cupons.FindAsync(trip.CuponId);
                //    if (coupon != null && coupon.Percentage.HasValue)
                //    {
                //        decimal baseTotal = baseHourCharge + returnCharge + nightCharge;
                //        couponAmount = Convert.ToInt32(baseTotal * coupon.Percentage.Value / 100);
                //    }
                //}
                //trip.couponAmount = -couponAmount;

                //decimal secureFee = 0;
                //if (trip.IsSecuredTrip == true)
                //{
                //    const decimal baseSecureFee = 15;
                //    const decimal secureGstPercent = 18;

                //    secureFee = baseSecureFee + (baseSecureFee * secureGstPercent / 100); // 15 + 2.7 = 17.7
                //}
                //trip.secureFee = Convert.ToInt32(secureFee);
                var driversubdata = _context.Driversubscriptions.Where(c => c.DriverId == trip.DriverId && c.Expirydate <= DateTime.Now).FirstOrDefault();
                if (driversubdata != null)
                {

                    var subscriptiondata = _context.Subscriptions.Find(driversubdata.SubscriptionId);
                    if (subscriptiondata != null)
                    {

                        // Taxes & Fees: ₹25 fixed + 5% GST on (hour + night + return)
                        decimal taxBase = baseHourCharge + returnCharge + nightCharge;
                        decimal taxGst = taxBase * 18 / 100;
                        decimal taxAndFee = 25 + taxGst;
                        trip.taxandfee = Convert.ToInt32(taxAndFee);
                    }
                }
                else
                {
                    decimal taxBase = baseHourCharge + returnCharge + nightCharge;
                    decimal taxGst = taxBase * 5 / 100;
                    decimal taxAndFee = 25 + taxGst;
                    trip.taxandfee = Convert.ToInt32(taxAndFee);
                }
                // Driver fee 
                //decimal driverFee = taxBase; // hour + night + return only
                //                             //trip.DriverFee = Math.Round(driverFee, 2);
                //trip.DriverFee = Convert.ToInt32(Math.Round(driverFee, 2));


                // Update Flexi entity with unique code and save changes again
                await _context.SaveChangesAsync();

                _context.Trips.Add(trip);
                await _context.SaveChangesAsync();
                var userdata = _context.Users.Find(trip.UserId);
                var senderid = "";
                var message = "";
                if (userdata != null)
                {
                     senderid = $"Upcoming Trip Reminder";
                    message = $"{userdata.Name}, this is a reminder for your upcoming trip from: \n- Pickup Location: {trip.ToLocationName} \n- Date & Time: {trip.StartDateTime}.";
                }
                else
                {
                    senderid = $"Upcoming Trip Reminder";
                    message = $"This is an important reminder about your upcoming trip from: \n- Pickup Location: {trip.ToLocationName} \n- Date & Time: {trip.StartDateTime}.";
                }

              await SendNotification("token", senderid, message,trip);

                return CreatedAtAction("GetTrip", new { id = trip.TripId }, trip);
            }
            private async Task<IActionResult> SendNotification(string token, string senderName, string text, Trip trip)
            {
                try
                {
                    var drivers = await _context.Drivers
         .Where(c => c.IsDriverActive == true)
         .ToListAsync();
                    var drivers1 = await _context.Drivers
        .Where(c => c.IsDriverActive == true)
        .Select(c => c.DriverId) // Assuming DriverId is of type int or int?
        .ToListAsync();
                    // Get ignored trips where DriverId is in the active drivers list
                    var ignoredTrips = await _context.IgnoredTrips
        .Where(c => c.DriverId != null && drivers1.Contains(c.DriverId.Value)) // Check if DriverId is in the active drivers
        .ToListAsync();
                    var distancetogettripsdata = _context.Driversurroundingtrips.FirstOrDefault();
                    int? distancetocover = 0;
                    if (distancetogettripsdata != null)
                    {
                        distancetocover = distancetogettripsdata.Distance;
                    }
                    else
                    {
                        distancetocover = 30;
                    }

                    if (drivers.Count > 0)
                    {
                        foreach (var driver in drivers)
                        {
                            string transmissionTypesString = driver.TransmissionTypeId;
                            string[] transmissionTypeStrings = new string[] { };
                            if (driver.TransmissionTypeId != string.Empty && driver.TransmissionTypeId != null)
                            {

                                transmissionTypeStrings = transmissionTypesString.Split(',');


                            }
                            List<int> transmissionTypes = transmissionTypeStrings.Select(int.Parse).ToList();
                            string vehicletypestring = driver.VehicleTypeIds;
                            string[] vehicletypeStrings = new string[] { };
                            if (vehicletypestring != string.Empty && driver.TransmissionTypeId != null)
                            {

                                vehicletypeStrings = vehicletypestring.Split(',');
                            }

                            List<int> vehicletypes = vehicletypeStrings.Select(int.Parse).ToList();
                            var tripdata = new List<Trip>();
                            if (transmissionTypesString != null && transmissionTypesString != string.Empty && vehicletypestring != string.Empty && driver.TransmissionTypeId != null)
                            {
                                tripdata = _context.Trips
                            .Where(c => (c.DriverId == 0 || c.DriverId == null || c.DriverId.HasValue) && c.IsCancelled != true && (c.IsTimeScheduled == false || (c.IsTimeScheduled == true && c.IsReserved != true && c.IsAccepted != true || (c.IsTimeScheduled == true && c.IsReserved == true && c.IsAccepted != true)))
                                          && (transmissionTypes.Contains(c.TransmissionTypeId.Value) || !c.TransmissionTypeId.HasValue)
                                          && (vehicletypes.Contains(c.VehicleTypeId.Value) || !c.VehicleTypeId.HasValue) && c.IsAccepted != true)
                            .ToList();



                            }
                            var driverwallet = _context.Driverwallets.Where(c => c.DriverId == driver.DriverId).FirstOrDefault();
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
                            if (driverwallet != null && Convert.ToInt32(driverwallet.WalletBalance) > minimumwalletbalance)
                            {
                                if (trip != null)
                                {
                                    var ignoredtripsofdriver = _context.IgnoredTrips.Where(c => c.FlexiId == trip.TripId &&c.DriverId ==driver.DriverId).FirstOrDefault();
                                    if (trip.FromLocation != null && trip.FromLocation != string.Empty)
                                    {
                                        if (ignoredtripsofdriver == null)
                                        {
                                            DateTime? newDateTime = trip.StartDateTime.HasValue ? trip.StartDateTime.Value.AddHours(4) : (DateTime?)null;
                                            if (newDateTime <= DateTime.Now)
                                            {
                                                if (trip.IsTripCompByDriver != true)
                                                {
                                                    trip.IsCancelled = true;
                                                    _context.Entry(trip).State = EntityState.Modified;
                                                    await _context.SaveChangesAsync();
                                                    continue;
                                                }
                                            }
                                            var locatrip = trip.FromLocation.Split(',').ToList();
                                            var driverDistance = distance(Convert.ToDouble(locatrip[0]), Convert.ToDouble(locatrip[1]), Convert.ToDouble(driver.Latitude), Convert.ToDouble(driver.Longitude), 'K');
                                            if (driverDistance < distancetocover)
                                            {
                                                if (driver.Token != null)
                                                {

                                                    var result = await _fcmService.SendNotificationAsync(driver.Token, senderName, text);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }


                    }
                    return Ok(); 
                }
                catch (Exception ex)
                {
                    return BadRequest($"Error sending notification: {ex.Message}");
                }
            }
            // DELETE: api/Trips/5
            [HttpDelete("{id:int}")]
            public async Task<IActionResult> DeleteTrip(int id)
            {
                if (_context.Trips == null)
                {
                    return NotFound();
                }
                var trip = await _context.Trips.FindAsync(id);
                if (trip == null)
                {
                    return NotFound();
                }

                _context.Trips.Remove(trip);
                await _context.SaveChangesAsync();

                return NoContent();
            }

            // invoice
            [HttpGet("{userId:int}/{tripId:int}/invoice")]

            public async Task<ActionResult<IEnumerable<TripVM>>> GetTripsForInvoice(int userId, int tripId)
            {
                try
                {
                    var list = await _context.Trips.Where(e => e.TripId == tripId && e.UserId == userId).FirstOrDefaultAsync();
                    if (list != null && list.IsTripCompByDriver == true)
                    {

                        var vm = new TripVM();
                        vm.UserId = list.UserId;
                        var user = _context.Users.Find(list.UserId);
                        if (user != null)
                        {
                            vm.UserName = user.Name;
                        }
                        vm.TripId = list.TripId;
                        vm.TripTypeId = list.TripTypeId;
                        var trip = _context.TripTypes.Find(list.TripTypeId);
                        if (trip != null)
                        {
                            vm.TripTypeName = trip.TripName;
                        }

                        vm.TaxIds = list.TaxIds;
                        var tax = _context.Taxes.Find(Convert.ToInt32(list.TaxIds));
                        if (tax != null)
                        {
                            vm.TaxName = tax.TaxName;
                        }
                        vm.GSTAmount = Convert.ToDecimal(list.TotalTripValue) * (vm.Percentage / 100);
                        var value = Convert.ToDecimal(list.TotalTripValue) - vm.GSTAmount;
                        vm.TotalTripValue = value.ToString();
                        vm.NoOfHoursActual = list.NoOfHoursActual;
                        return Ok(vm);
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
            //company invoice

            [HttpGet("companyInvoice/flag")]
            public async Task<ActionResult<IEnumerable<TripVM>>> GetTripsForAllInvoice(DateTime fromdate, DateTime todate)
            {
                try
                {
                    var vmlist = new List<TripVM>();
                    var list = await _context.Trips.Where(r => r.CreatedDate >= fromdate && r.CreatedDate <= todate).ToListAsync();

                    if (list.Count > 0)
                    {
                        foreach (var item in list)
                        {
                            var vm = new TripVM();
                            vm.TripId = item.TripId;
                            vm.UserId = item.UserId;
                            var user = _context.Users.Find(item.UserId);
                            if (user != null)
                            {
                                vm.UserName = user.Name;
                            }
                            vm.TripTypeId = item.TripTypeId;
                            var trip = _context.TripTypes.Find(item.TripTypeId);
                            if (trip != null)
                            {
                                vm.TripTypeName = trip.TripName;
                            }

                            vm.TaxIds = item.TaxIds;
                            if (vm.TaxIds != "" && vm.TaxIds != "string")
                            {

                                var tax = _context.Taxes.Find(Convert.ToInt32(item.TaxIds));
                                if (tax != null)
                                {
                                    vm.TaxName = tax.TaxName;
                                }
                            }
                            vm.GSTAmount = Convert.ToDecimal(item.TotalTripValue) * (vm.Percentage / 100);
                            var value = Convert.ToDecimal(item.TotalTripValue) - vm.GSTAmount;
                            vm.TotalTripValue = value.ToString();
                            vm.NoOfHoursActual = item.NoOfHoursActual;
                            vmlist.Add(vm);
                        }
                    }
                    return Ok(vmlist);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            [HttpGet("totaldriverSubscriptions")]
            public async Task<IActionResult> TotalDriverswithGST(DateTime fromdate, DateTime todate)
            {
                try
                {
                    var trips = await _context.Driversubscriptions.Where(r => r.CreatedDate >= fromdate && r.CreatedDate <= todate).ToListAsync();
                    var vmlist = new List<DriversubscriptionTransctionVM>();
                    if (trips.Count > 0)
                    {
                        foreach (var driver in trips)
                        {
                            var vm = new DriversubscriptionTransctionVM();
                            vm.DriverId = driver.DriverId;
                            var d = _context.Drivers.Find(driver.DriverId);
                            if (d != null)
                            {
                                vm.DriverName = d.DriverName;
                                vm.PhoneNumber = d.PhoneNumber;

                            }
                            vm.Expirydate = driver.Expirydate;
                            vm.CreatedDate = driver.CreatedDate;
                            vm.SubscriptionId = driver.SubscriptionId;
                            var sub = _context.Subscriptions.Find(driver.SubscriptionId);
                            if (sub != null)
                            {
                                vm.SubPrice = sub.SubPrice;
                                //var gstval = sub.SubscripationGstId;
                                var gst = _context.SubscripationGst.Find(sub.SubscripationGstId);
                                if (gst != null)
                                {
                                    var gstpercent = Convert.ToDecimal(gst.GstPercentage);
                                    var gstvalue = vm.SubPrice * (gstpercent / 100);
                                    vm.GSTAmount = gstvalue;
                                }
                            }
                            vm.Amount = driver.Amount;
                            vmlist.Add(vm);
                        }
                        return Ok(vmlist);
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




            // totaltripsGST
            [HttpGet("totaltrips")]
            public async Task<IActionResult> TotalTripswithGST(DateTime fromdate, DateTime todate)
            {
                try
                {
                    var trips = await _context.Trips.Where(r => r.StartDateTime >= fromdate && r.EndDateTime <= todate).ToListAsync();
                    if (trips.Any())
                    {
                        var vm = new TotalTripCountsVM();
                        vm.TotalTrips = trips.Count;
                        vm.TotalDrivers = trips.Select(t => t.DriverId).Distinct().Count();
                        vm.TotalCustomers = trips.Select(t => t.UserId).Distinct().Count();
                        vm.TotalCustomerAmount = trips.Sum(t => Convert.ToDecimal(t.TotalTripValue));
                        var taxes = await _context.Taxes.Where(t => trips.Select(t => Convert.ToInt32(t.TaxIds)).Contains(t.TaxId)).ToListAsync();
                        if (taxes.Count > 0)
                        {
                            decimal? totalGSTAmount = 0;
                            decimal? totalGSTPercentage = 0;
                            foreach (var trip in trips)
                            {
                                var tripTax = taxes.FirstOrDefault(t => t.TaxId == Convert.ToInt32(trip.TaxIds));
                                if (tripTax != null)
                                {
                                    totalGSTPercentage += tripTax.Percentage;
                                    var tripGSTAmount = tripTax.Percentage * Convert.ToDecimal(trip.TotalTripValue) / 100;
                                    totalGSTAmount += tripGSTAmount;
                                }
                            }
                            vm.TotalGSTPercentage = totalGSTPercentage;
                            vm.TotalGSTAmount = totalGSTAmount;
                        }
                        return Ok(vm);
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


            private bool TripExists(int id)
            {
                return (_context.Trips?.Any(e => e.TripId == id)).GetValueOrDefault();
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

        class GeoCoordinate
        {
            public double Latitude { get; }
            public double Longitude { get; }

            public GeoCoordinate(double latitude, double longitude)
            {
                Latitude = latitude;
                Longitude = longitude;
            }

        }
    }

#region some code
//var drivertracking = _context.DriverTrackings.Where(c => c.DriverId == tripVM.DriverId && c.TripId == tripVM.TripId).ToList();
//if (drivertracking.Count > 1)
//{
//    for (int i = 0; i < drivertracking.Count - 1; i++)
//    {
//        var currentEntry = drivertracking[i];
//        var nextEntry = drivertracking[i + 1];

//        Decimal? currentLatitude = currentEntry.Latitude;
//        Decimal? currentLongitude = currentEntry.Longitude;
//        Decimal? nextLatitude = nextEntry.Latitude;
//        Decimal? nextLongitude = nextEntry.Longitude;

//        var actualgoogledistanceresult = await GetGoogleDistance(currentLatitude.ToString(), currentLatitude.ToString(), nextLatitude.ToString(), nextLongitude.ToString());
//        int actualgoogledistance = Convert.ToInt32(actualgoogledistanceresult);

//        travelledonroaddistance += actualgoogledistance;

//        Console.WriteLine($"Pair {i + 1}: ({currentLatitude}, {currentLongitude}) - ({nextLatitude}, {nextLongitude})");
//    }
//}
#endregion