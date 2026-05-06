using GoChauffeurWebApi.Data;
using GoChauffeurWebApi.Models;
using GoChauffeurWebApi.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Linq;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Tripstodriver : ControllerBase
    {
        private readonly DataContext _context;

        public Tripstodriver(DataContext context)
        {
            _context = context;
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<TripVM>>> GetTrips(int id)
        //{
        //    try
        //    {
        //        var drivers = _context.Drivers.Find(id);
        //        var ignoretrips = _context.IgnoredTrips.Where(c => c.DriverId == id).ToList();
        //        var distancetogettripsdata = _context.Driversurroundingtrips.FirstOrDefault();
        //        int? distancetocover = 0;
        //        if (distancetogettripsdata != null)
        //        {
        //            distancetocover = distancetogettripsdata.Distance;
        //        }
        //        else
        //        {
        //            distancetocover = 30;
        //        }
        //        if (drivers != null)
        //        {

        //            string transmissionTypesString = drivers.TransmissionTypeId;
        //            string[] transmissionTypeStrings = new string[] { };
        //            if (drivers.TransmissionTypeId != string.Empty && drivers.TransmissionTypeId != null)
        //            {

        //                transmissionTypeStrings = transmissionTypesString.Split(',');


        //            }
        //            List<int> transmissionTypes = transmissionTypeStrings.Select(int.Parse).ToList();
        //            string vehicletypestring = drivers.VehicleTypeIds;
        //            string[] vehicletypeStrings = new string[] { };
        //            if (vehicletypestring != string.Empty && drivers.TransmissionTypeId != null)
        //            {

        //                vehicletypeStrings = vehicletypestring.Split(',');
        //            }

        //            List<int> vehicletypes = vehicletypeStrings.Select(int.Parse).ToList();
        //            var tripdata = new List<Trip>();
        //            if (transmissionTypesString != null && transmissionTypesString != string.Empty && vehicletypestring != string.Empty && drivers.TransmissionTypeId != null)
        //            {
        //                var driverId = id; // Specify the DriverId to check
        //                tripdata = _context.Trips
        //            .Where(c => (c.DriverId == 0 || c.DriverId == null || c.DriverId.HasValue)
        //                          && (transmissionTypes.Contains(c.TransmissionTypeId.Value) || !c.TransmissionTypeId.HasValue)
        //                          && (vehicletypes.Contains(c.VehicleTypeId.Value) || !c.VehicleTypeId.HasValue ))
        //            .ToList();



        //            }
        //            else
        //            {
        //                return BadRequest("Please contact admin to update vehicletypes ");
        //            }

        //            var tripVMList = new List<TripVM>();

        //            if (tripdata.Count > 0)
        //            {

        //                foreach (var trip in tripdata)
        //                {
        //                    var ignoredtripsofdriver = ignoretrips.Where(c => c.TripId == trip.TripId).FirstOrDefault();
        //                    if (trip.FromLocation != null && trip.FromLocation != string.Empty)
        //                    {

        //                        if (ignoredtripsofdriver == null)
        //                        {
        //                            var locatrip = trip.FromLocation.Split(',').ToList();
        //                            var driverDistance = distance(Convert.ToDouble(locatrip[0]), Convert.ToDouble(locatrip[1]), Convert.ToDouble(drivers.Latitude), Convert.ToDouble(drivers.Longitude), 'K');
        //                            var tripVM = new TripVM();
        //                            tripVM.IsTimeScheduled = trip.IsTimeScheduled;
        //                            if (driverDistance < distancetocover)
        //                            {
        //                                if (trip.IsTimeScheduled == true)
        //                                {
        //                                    var dataconfiguretime = _context.Schuduletriptimechagemodel.Where(c => c.TripTypeId == trip.TripTypeId).FirstOrDefault();
        //                                    if (dataconfiguretime != null)
        //                                    {
        //                                        TimeZoneInfo istTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        //                                        DateTime istStartDateTime = TimeZoneInfo.ConvertTimeFromUtc(trip.StartDateTime.Value, istTimeZone);
        //                                        DateTime currentIstTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, istTimeZone);

        //                                        DateTime currentIstTimePlus45Minutes = currentIstTime.AddMinutes(Convert.ToInt32(dataconfiguretime.SchuduletripDuration));
        //                                        if (trip.IsReserved == true)
        //                                        {

        //                                            DateTime? startTimes = trip.StartDateTime;

        //                                            var starttimes = String.Format("{0:h:mm tt}", startTimes);

        //                                            currentIstTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, istTimeZone);
        //                                            // Assuming tripVM.StartTime is a string representing a date and time
        //                                            tripVM.StartTime = starttimes;

        //                                            // Convert StartTime string to DateTime
        //                                            DateTime startTimeDateTime = DateTime.Parse(tripVM.StartTime);

        //                                            // Convert the duration to an integer
        //                                            int duration = Convert.ToInt32(dataconfiguretime.SchuduletripDuration);

        //                                            // Calculate the time before the duration
        //                                            DateTime beforeDurationTime = startTimeDateTime.AddMinutes(-duration);

        //                                            // Define AcceptTimeTo as beforeDurationTime
        //                                            DateTime AcceptTimeTo = beforeDurationTime;



        //                                            // Define AcceptTimeFrom as the beginning of the hour of beforeDurationTime
        //                                            DateTime AcceptTimeFrom = new DateTime(beforeDurationTime.Year, beforeDurationTime.Month, beforeDurationTime.Day, beforeDurationTime.Hour, 0, 0);


        //                                            // Output the results (if needed for debugging)
        //                                            Console.WriteLine($"AcceptTimeFrom: {AcceptTimeFrom}");
        //                                            Console.WriteLine($"AcceptTimeTo: {AcceptTimeTo}");

        //                                            // You can assign these values to the appropriate properties in your view model if needed
        //                                            tripVM.Accepttimefrom = AcceptTimeFrom;
        //                                            tripVM.AccepttimeTo = AcceptTimeTo;




        //                                        }

        //                                        if (istStartDateTime < currentIstTimePlus45Minutes)
        //                                        {
        //                                            await Udatetriptoinstant(trip.TripId);
        //                                            tripVM.IsTimeScheduled = false;
        //                                        }
        //                                    }
        //                                }
        //                                tripVM.TripId = trip.TripId;
        //                                tripVM.IsAccepted = trip.IsAccepted;
        //                                tripVM.TripsUniqueId = trip.TripsUniqueId;
        //                                tripVM.ExpectedDistance = Math.Round(Convert.ToDecimal(driverDistance), 2);

        //                                tripVM.UserId = trip.UserId;
        //                                tripVM.NoOfHoursSelected = trip.NoOfHoursSelected;
        //                                var userdata = _context.Users.Find(tripVM.UserId);

        //                                if (userdata != null)
        //                                {
        //                                    tripVM.UserName = userdata.Name;
        //                                    tripVM.UserPhoneNumber = userdata.PhoneNumber;
        //                                    tripVM.Image = userdata.UserImage;
        //                                }

        //                                tripVM.DriverId = id;
        //                                tripVM.IsReserved = trip.IsReserved;
        //                                var driverdata = _context.Drivers.Find(tripVM.DriverId);

        //                                if (driverdata != null)
        //                                {

        //                                    tripVM.DriverName = driverdata.DriverName;

        //                                    tripVM.DriverImage = driverdata.Image;

        //                                    tripVM.Experience = driverdata.Experiance;

        //                                }

        //                                tripVM.PickUPMapURL = trip.PickUPMapURL;
        //                                tripVM.DropUPMapURL = trip.DropUPMapURL;
        //                                tripVM.IsTimeScheduled = trip.IsTimeScheduled;
        //                                tripVM.StartDate = trip.StartDateTime;
        //                                tripVM.FromLocationName = trip.FromLocationName;
        //                                tripVM.ToLocationName = trip.ToLocationName;
        //                                tripVM.EstimatedPrice = trip.EstimatedPrice;

        //                                DateTime? startTime = tripVM.StartDate;

        //                                var starttime = String.Format("{0:h:mm tt}", startTime);

        //                                tripVM.StartTime = starttime;

        //                                tripVM.EndDate = trip.EndDateTime;

        //                                DateTime? endTime = tripVM.EndDate;

        //                                var endtime = String.Format("{0:h:mm tt}", endTime);

        //                                tripVM.EndTime = endtime;

        //                                tripVM.ActualEndDate = trip.ActualEndTime;

        //                                DateTime? actualendTime = tripVM.ActualEndDate;

        //                                var actualendtime = String.Format("{0:h:mm tt}", actualendTime);

        //                                tripVM.ActualEndTime = actualendtime;

        //                                if (tripVM.StartDate.HasValue && tripVM.ActualEndDate.HasValue)
        //                                {
        //                                    TimeSpan duration = tripVM.ActualEndDate.Value - tripVM.StartDate.Value;

        //                                    double minutes = duration.TotalMinutes;
        //                                    tripVM.ActualTotalMinutes = minutes;
        //                                }

        //                                if (tripVM.StartDate.HasValue && tripVM.EndDate.HasValue)
        //                                {
        //                                    TimeSpan duration = tripVM.EndDate.Value - tripVM.StartDate.Value;

        //                                    double minutes = duration.TotalMinutes;

        //                                    tripVM.TotalMinutes = minutes;
        //                                }

        //                                tripVM.RequestedDate = trip.RequstedDateTime;

        //                                DateTime? requestedTime = tripVM.RequestedDate;

        //                                var requestedtime = String.Format("{0:h:mm tt}", requestedTime);

        //                                tripVM.RequestedTime = requestedtime;

        //                                tripVM.TripTypeId = trip.TripTypeId;

        //                                tripVM.IsTripCompByDriver = trip.IsTripCompByDriver;

        //                                var tripTypeData = _context.TripTypes.Find(trip.TripTypeId);

        //                                if (tripTypeData != null)
        //                                {

        //                                    tripVM.TripTypeName = tripTypeData.TripName;

        //                                }

        //                                tripVM.TripvarientId = trip.TripvarientId;

        //                                var triptypevariantdata = _context.TripVariants.Find(trip.TripvarientId);

        //                                if (triptypevariantdata != null)
        //                                {

        //                                    tripVM.BasePrice = triptypevariantdata.BasePrice;

        //                                    tripVM.KilometerLimit = triptypevariantdata.KilometerLimit;

        //                                    tripVM.NightCharges = triptypevariantdata.NightCharges;

        //                                    tripVM.ChargesperMinute = triptypevariantdata.ChargesperMinute;

        //                                    tripVM.PricePerKilometers = triptypevariantdata.PricePerKilometers;

        //                                    tripVM.IsTripOneway = triptypevariantdata.IsTripOneway;

        //                                }

        //                                tripVM.VehicleId = trip.VehicleId;

        //                                if (trip.VehicleId != 0)
        //                                {

        //                                    var vehicledata = _context.Vehicles.Find(trip.VehicleId);

        //                                    if (vehicledata != null)
        //                                    {

        //                                        tripVM.VehicleNo = vehicledata.VehicleNo;

        //                                        if (!string.IsNullOrEmpty(vehicledata.Images))
        //                                        {

        //                                            var images = vehicledata.Images.Split(',').ToList();

        //                                            var vehicleimagelist = new List<VehicleImages>();

        //                                            if (images.Count > 0)
        //                                            {

        //                                                foreach (var image in images)
        //                                                {

        //                                                    var imagevm = new VehicleImages();

        //                                                    imagevm.VechileImage = image;

        //                                                    vehicleimagelist.Add(imagevm);
        //                                                }
        //                                            }

        //                                            tripVM.VehicleImages = vehicleimagelist;
        //                                        }

        //                                        tripVM.VehicleTypeId = vehicledata.VehicleTypeId;

        //                                        if (tripVM.VehicleTypeId == 0)
        //                                        {

        //                                            var vehicletypedata = _context.VehicleTypes.Find(tripVM.VehicleTypeId);

        //                                            if (vehicletypedata != null)
        //                                            {

        //                                                tripVM.VehicleTypeName = vehicletypedata.VehicleTypeName;
        //                                            }
        //                                        }


        //                                    }

        //                                }
        //                                if (!string.IsNullOrEmpty(trip.FromLocation))
        //                                {

        //                                    var fromLocations = trip.FromLocation.Split(',').ToList();

        //                                    if (fromLocations.Count > 0)
        //                                    {

        //                                        tripVM.FromLatitude = fromLocations[0];

        //                                        tripVM.FromLongitude = fromLocations[1];

        //                                    }
        //                                }
        //                                if (!string.IsNullOrEmpty(trip.ToLocation))
        //                                {

        //                                    var toLocations = trip.ToLocation.Split(',').ToList();

        //                                    if (toLocations.Count > 0)
        //                                    {

        //                                        tripVM.ToLatitude = toLocations[0];

        //                                        tripVM.ToLongitude = toLocations[1];
        //                                    }
        //                                }
        //                                tripVM.ImageUrlsList = trip.ImageUrlsList;
        //                                if (tripVM.ImageUrlsList != null && tripVM.ImageUrlsList != "")
        //                                {
        //                                    var imageurls = trip.ImageUrlsList.Split(',').ToList();
        //                                    var imageurlslist = new List<Imageurls>();
        //                                    if (imageurls.Count > 0)
        //                                    {
        //                                        foreach (var imageUrl in imageurls)
        //                                        {
        //                                            var imagevm = new Imageurls();
        //                                            imagevm.Images = imageUrl;
        //                                            imageurlslist.Add(imagevm);
        //                                        }
        //                                    }
        //                                    tripVM.imageurls = imageurlslist;
        //                                }
        //                                int travelledonroaddistance = 0;

        //                                var drivertracking = _context.DriverTrackings.Where(c => c.DriverId == tripVM.DriverId && c.TripId == tripVM.TripId).ToList();

        //                                if (drivertracking.Count > 1)
        //                                {

        //                                    var currentEntry = drivertracking.FirstOrDefault();

        //                                    var nextEntry = drivertracking.Last();


        //                                    Decimal? currentLatitude = currentEntry.Latitude;

        //                                    Decimal? currentLongitude = currentEntry.Longitude;

        //                                    Decimal? nextLatitude = nextEntry.Latitude;

        //                                    Decimal? nextLongitude = nextEntry.Longitude;

        //                                    var actualgoogledistanceresult = await GetGoogleDistance(currentLatitude.ToString(), currentLatitude.ToString(), nextLatitude.ToString(), nextLongitude.ToString());
        //                                    int actualgoogledistance = 0;
        //                                    if (actualgoogledistanceresult.Result == null)
        //                                    {

        //                                        actualgoogledistance = Convert.ToInt32(actualgoogledistanceresult.Value);
        //                                    }
        //                                    else
        //                                    {

        //                                        actualgoogledistance = Convert.ToInt32(actualgoogledistanceresult);
        //                                    }

        //                                    travelledonroaddistance += actualgoogledistance;

        //                                }

        //                                var entityDistance = distance(Convert.ToDouble(tripVM.FromLatitude), Convert.ToDouble(tripVM.FromLongitude), Convert.ToDouble(tripVM.ToLatitude), Convert.ToDouble(tripVM.ToLongitude), 'K');


        //                                tripVM.AerialDistance = Math.Round(Convert.ToDecimal(entityDistance), 2);

        //                                var aerialDistancedata = _context.AerialDistancePrices.ToList();

        //                                if (aerialDistancedata.Count > 0)
        //                                {

        //                                    tripVM.Aerialpriceperkilometer = Convert.ToDecimal(aerialDistancedata[0].AerialDistancePriceperKilometer);
        //                                }

        //                                tripVM.ExpectedDistance = trip.ExpectedDistance;

        //                                tripVM.CuponId = tripVM.CuponId;

        //                                var cupondata = _context.Cupons.Find(tripVM.CuponId);

        //                                if (cupondata != null)
        //                                {

        //                                    tripVM.CuponName = cupondata.CuponName;

        //                                    tripVM.CuponCode = cupondata.CuponCode;

        //                                    tripVM.CuponPercentage = cupondata.Percentage;
        //                                }
        //                                else if (cupondata == null)
        //                                {

        //                                    tripVM.CuponPercentage = 0;
        //                                }




        //                                tripVM.IsSecuredTrip = trip.IsSecuredTrip;

        //                                if (tripVM.IsTripOneway == true)    
        //                                {

        //                                    var expectedextradiatancethanbaseLimit = Math.Abs(Convert.ToDecimal(tripVM.ExpectedDistance) - Convert.ToDecimal(tripVM.KilometerLimit));


        //                                    var expectedtotalpriceforextradistance = expectedextradiatancethanbaseLimit * tripVM.PricePerKilometers;

        //                                    var expectedtotalprice = expectedtotalpriceforextradistance + (tripVM.BasePrice * tripVM.NoOfHoursSelected) + tripVM.Aerialpriceperkilometer;

        //                                    var cuponpercentagevalue = expectedtotalprice * tripVM.CuponPercentage / 100;

        //                                    var expectedtotalvaluewithouttax = expectedtotalprice - cuponpercentagevalue;//240

        //                                    Decimal? securetaxvalue = 0;

        //                                    if (tripVM.IsSecuredTrip == true)
        //                                    {


        //                                        var insurencedata = _context.InsurenceTaxandPrice.FirstOrDefault();

        //                                        if (insurencedata != null)
        //                                        {

        //                                            var taxpercentage = insurencedata.TaxPercentage;

        //                                            var insurencetaxvalue = expectedtotalvaluewithouttax * taxpercentage / 100;

        //                                            securetaxvalue = insurencetaxvalue;//28.80
        //                                        }
        //                                    }

        //                                    Decimal travelledtotaltaxvalue = 0;

        //                                    var taxesdata = _context.Taxes.ToList();

        //                                    if (taxesdata.Count > 0)
        //                                    {
        //                                        foreach (var tax in taxesdata)
        //                                        {

        //                                            var taxpercentage = Convert.ToInt32(tax.Percentage);

        //                                            var taxvalue = Convert.ToDecimal(expectedtotalvaluewithouttax) * taxpercentage / 100;

        //                                            var travelledtaxvalue = Convert.ToDecimal(expectedtotalvaluewithouttax) * taxpercentage / 100;

        //                                            travelledtotaltaxvalue += travelledtaxvalue;//19.20
        //                                        }
        //                                    }

        //                                    tripVM.ExpectedTotalTripvalue = (expectedtotalvaluewithouttax + travelledtotaltaxvalue + securetaxvalue + tripVM.NightCharges);//488
        //                                    Decimal? totalvaluewithouttax = 0;
        //                                    if (tripVM.IsTripCompByDriver == true)
        //                                    {

        //                                        var extradiatancethanbaseLimit = Convert.ToDecimal(travelledonroaddistance) - Convert.ToDecimal(tripVM.KilometerLimit);

        //                                        var totalpriceforextradistance = extradiatancethanbaseLimit * tripVM.PricePerKilometers;

        //                                        var totalprice = totalpriceforextradistance + (tripVM.BasePrice * tripVM.NoOfHoursActual) + tripVM.Aerialpriceperkilometer;

        //                                        var actualcuponpercentagevalue = totalprice * tripVM.CuponPercentage / 100;

        //                                        totalvaluewithouttax = totalprice - actualcuponpercentagevalue;

        //                                    }
        //                                    tripVM.TotalTripValue = Convert.ToString(totalvaluewithouttax + travelledtotaltaxvalue + securetaxvalue + tripVM.NightCharges);


        //                                }
        //                                else
        //                                {
        //                                    var expectedMinutes = tripVM.TotalMinutes;

        //                                    var expectedPrice = tripVM.ChargesperMinute * Convert.ToDecimal(expectedMinutes);
        //                                    var cuponpercentagevalue = expectedPrice * tripVM.CuponPercentage / 100;
        //                                    var expectedTotalPricewithouttax = expectedPrice + tripVM.NightCharges - cuponpercentagevalue;


        //                                    Decimal? securetaxvalue = 0;

        //                                    if (tripVM.IsSecuredTrip == true)
        //                                    {

        //                                        var insurencedata = _context.InsurenceTaxandPrice.FirstOrDefault();

        //                                        if (insurencedata != null)
        //                                        {

        //                                            var taxpercentage = insurencedata.TaxPercentage;

        //                                            var insurencetaxvalue = expectedTotalPricewithouttax * taxpercentage / 100;

        //                                            securetaxvalue = insurencetaxvalue;
        //                                        }
        //                                    }

        //                                    Decimal travelledtotaltaxvalue = 0;

        //                                    var taxesdata = _context.Taxes.ToList();

        //                                    if (taxesdata.Count > 0)
        //                                    {
        //                                        foreach (var tax in taxesdata)
        //                                        {

        //                                            var taxpercentage = Convert.ToInt32(tax.Percentage);

        //                                            var taxvalue = Convert.ToDecimal(expectedTotalPricewithouttax) * taxpercentage / 100;

        //                                            var travelledtaxvalue = Convert.ToDecimal(expectedTotalPricewithouttax) * taxpercentage / 100;

        //                                            travelledtotaltaxvalue += travelledtaxvalue;
        //                                        }
        //                                    }
        //                                    tripVM.ExpectedTotalTripvalue =(expectedTotalPricewithouttax + travelledtotaltaxvalue + securetaxvalue + tripVM.NightCharges);
        //                                    Decimal? totalvaluewithouttax = 0;
        //                                    if (tripVM.IsTripCompByDriver == true)
        //                                    {

        //                                        var totalMinutes = tripVM.TotalMinutes;

        //                                        var price = tripVM.ChargesperMinute * Convert.ToDecimal(expectedMinutes);
        //                                        var totalPricewithouttax = price + tripVM.NightCharges - cuponpercentagevalue;

        //                                    }
        //                                    tripVM.TotalTripValue = Convert.ToString(totalvaluewithouttax + travelledtotaltaxvalue + securetaxvalue + tripVM.NightCharges);


        //                                }

        //                                tripVMList.Add(tripVM);
        //                            }
        //                        }
        //                    }
        //                }
        //                return Ok(tripVMList);
        //            }
        //            else
        //            {
        //                return NoContent();
        //            }
        //        }
        //        else
        //        {
        //            return NoContent();
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        /// Sree upadted code 


        [HttpGet]
        public async Task<ActionResult<IEnumerable<TripVM>>> GetTrips(int id)
        {
            try
            {
                var drivers = _context.Drivers.Where(c=>c.DriverId ==id && c.IsDriverActive==true).FirstOrDefault();

                var ignoretrips = _context.IgnoredTrips.Where(c => c.DriverId == id).ToList();
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
                if (drivers != null )
                {

                    string transmissionTypesString = drivers.TransmissionTypeId;
                    string[] transmissionTypeStrings = new string[] { };
                    if (drivers.TransmissionTypeId != string.Empty && drivers.TransmissionTypeId != null)
                    {

                        transmissionTypeStrings = transmissionTypesString.Split(',');


                    }
                    List<int> transmissionTypes = transmissionTypeStrings.Select(int.Parse).ToList();
                    string vehicletypestring = drivers.VehicleTypeIds;
                    string[] vehicletypeStrings = new string[] { };
                    if (vehicletypestring != string.Empty && drivers.TransmissionTypeId != null)
                    {

                        vehicletypeStrings = vehicletypestring.Split(',');
                    }

                    List<int> vehicletypes = vehicletypeStrings.Select(int.Parse).ToList();
                    var tripdata = new List<Trip>();
                    if (transmissionTypesString != null && transmissionTypesString != string.Empty && vehicletypestring != string.Empty && drivers.TransmissionTypeId != null)
                    {
                        var driverId = id; // Specify the DriverId to check
                        tripdata = _context.Trips
                    .Where(c => (c.DriverId == 0 || c.DriverId == null || c.DriverId.HasValue)&&c.IsCancelled != true && (c.IsTimeScheduled==false || (c.IsTimeScheduled==true&&c.IsReserved!=true&& c.IsAccepted!=true||(c.IsTimeScheduled == true&&c.IsReserved==true && c.IsAccepted != true && c.DriverId == id)))
                                  && (transmissionTypes.Contains(c.TransmissionTypeId.Value) || !c.TransmissionTypeId.HasValue)
                                  && (vehicletypes.Contains(c.VehicleTypeId.Value) || !c.VehicleTypeId.HasValue)&&c.IsAccepted!=true)
                    .ToList();



                    }
                    else
                    {
                        return BadRequest("Please contact admin to update vehicletypes ");
                    }
                    var driverwallet = _context.Driverwallets.Where(c => c.DriverId == drivers.DriverId).FirstOrDefault();
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

                        var tripVMList = new List<TripVM>();

                        if (tripdata.Count > 0)
                        {

                            foreach (var trip in tripdata)
                            {
                                var ignoredtripsofdriver = ignoretrips.Where(c => c.FlexiId == trip.TripId).FirstOrDefault();
                                if (trip.FromLocation != null && trip.FromLocation != string.Empty)
                                {

                                    if (ignoredtripsofdriver == null)
                                    {
                                        DateTime? newDateTime = trip.StartDateTime.HasValue? trip.StartDateTime.Value.AddHours(4): (DateTime?)null;
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
                                        var driverDistance = distance(Convert.ToDouble(locatrip[0]), Convert.ToDouble(locatrip[1]), Convert.ToDouble(drivers.Latitude), Convert.ToDouble(drivers.Longitude), 'K');
                                        var tripVM = new TripVM();
                                        //tripVM.IsTimeScheduled = trip.IsTimeScheduled;
                                        if (driverDistance < distancetocover)
                                        {
                                            if (trip.IsTimeScheduled == true)
                                            {
                                                var dataconfiguretime = _context.Schuduletriptimechagemodel.Where(c => c.TripTypeId == trip.TripTypeId).FirstOrDefault();
                                                if (dataconfiguretime != null)
                                                {
                                                    TimeZoneInfo istTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                                                    DateTime istStartDateTime = TimeZoneInfo.ConvertTimeFromUtc(trip.StartDateTime.Value, istTimeZone);
                                                    DateTime currentIstTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, istTimeZone);

                                                    DateTime currentIstTimePlus45Minutes = currentIstTime.AddMinutes(Convert.ToInt32(dataconfiguretime.SchuduletripDuration));
                                                    if (trip.IsReserved == true)
                                                    {

                                                        DateTime? startTimes = trip.StartDateTime;

                                                        var starttimes = String.Format("{0:h:mm tt}", startTimes);

                                                        currentIstTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, istTimeZone);
                                                        // Assuming tripVM.StartTime is a string representing a date and time
                                                        tripVM.StartTime = starttimes;

                                                        // Convert StartTime string to DateTime
                                                        DateTime startTimeDateTime = DateTime.Parse(tripVM.StartTime);

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
                                                        tripVM.Accepttimefrom = AcceptTimeFrom;
                                                        tripVM.AccepttimeTo = AcceptTimeFrom.AddMinutes(15);

                                                    }

                                                    if (istStartDateTime < currentIstTimePlus45Minutes)
                                                    {
                                                        await Udatetriptoinstant(trip.TripId);
                                                        tripVM.IsTimeScheduled = false;
                                                    }
                                                }
                                            }
                                            tripVM.TripId = trip.TripId;
                                            tripVM.IsAccepted = trip.IsAccepted;
                                            tripVM.TripsUniqueId = trip.TripsUniqueId;
                                            tripVM.ExpectedDistance = Math.Round(Convert.ToDecimal(driverDistance), 2);
                                            tripVM.IsTimeScheduled = trip.IsTimeScheduled;
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
                                            tripVM.IsReserved = trip.IsReserved;
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
                                            tripVM.Istakenpics = trip.Istakenpics;
                                            tripVM.IsEndPicsTaken= trip.IsEndPicsTaken;

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
                                                tripVM.TripTypeStarttime = tripTypeData.Starttime;
                                                tripVM.TripTypeEndtime = tripTypeData.Endtime;


                                            }

                                            tripVM.TripvarientId = trip.TripvarientId;

                                            var triptypevariantdata = _context.TripVariants.Find(trip.TripvarientId);

                                            if (triptypevariantdata != null)
                                            {
                                                tripVM.TripvarientName = triptypevariantdata.TripVariantName;

                                                tripVM.BasePrice = triptypevariantdata.BasePrice;

                                                tripVM.KilometerLimit = triptypevariantdata.KilometerLimit;
                                                if (tripVM.StartDate.HasValue && tripVM.EndDate.HasValue)
                                                {
                                                    // Assuming the time part of StartDate and EndDate is to be ignored and you are only working with the date part
                                                    DateTime tripnightchargestartTime = DateTime.ParseExact("10:00 PM", "hh:mm tt", CultureInfo.InvariantCulture);
                                                    DateTime tripnightchargesendTime = DateTime.ParseExact("06:00 AM", "hh:mm tt", CultureInfo.InvariantCulture);

                                                    // Assuming the logic checks if StartDate is within the night charge period
                                                    DateTime startDate = tripVM.StartDate.Value;
                                                    DateTime endDate = tripVM.EndDate.Value;

                                                    if (startDate.TimeOfDay >= tripnightchargestartTime.TimeOfDay || endDate.TimeOfDay <= tripnightchargesendTime.TimeOfDay)
                                                    {
                                                        tripVM.NightCharges = triptypevariantdata.NightCharges;
                                                    }
                                                    else
                                                    {
                                                        tripVM.NightCharges = 0;
                                                    }
                                                }
                                                else
                                                {
                                                    // Handle the case where StartDate or EndDate is null, if necessary
                                                    tripVM.NightCharges = 0;
                                                }

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
                                            tripVM.ImageUrlsList = trip.ImageUrlsList;
                                            if (tripVM.ImageUrlsList != null && tripVM.ImageUrlsList != "")
                                            {
                                                var imageurls = trip.ImageUrlsList.Split(',').ToList();
                                                var imageurlslist = new List<Imageurls>();
                                                if (imageurls.Count > 0)
                                                {
                                                    foreach (var imageUrl in imageurls)
                                                    {
                                                        var imagevm = new Imageurls();
                                                        imagevm.Images = imageUrl;
                                                        imageurlslist.Add(imagevm);
                                                    }
                                                }
                                                tripVM.imageurls = imageurlslist;
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

                                                var actualgoogledistanceresult = await GetGoogleDistance(currentLatitude.ToString(), currentLatitude.ToString(), nextLatitude.ToString(), nextLongitude.ToString());
                                                int actualgoogledistance = 0;
                                                if (actualgoogledistanceresult.Result == null)
                                                {

                                                    actualgoogledistance = Convert.ToInt32(actualgoogledistanceresult.Value);
                                                }
                                                else
                                                {

                                                    actualgoogledistance = Convert.ToInt32(actualgoogledistanceresult);
                                                }

                                                travelledonroaddistance += actualgoogledistance;

                                            }

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
                                            else if (cupondata == null)
                                            {

                                                tripVM.CuponPercentage = 0;
                                            }

                                            Decimal? taxpercentages = 0;
                                            var taxdata = await _context.Taxes.FirstOrDefaultAsync();
                                            if (taxdata != null)
                                            {
                                                taxpercentages = taxdata.Percentage;

                                            }

                                            tripVM.IsSecuredTrip = trip.IsSecuredTrip;

                                            if (tripVM.IsTripOneway == true)
                                            {
                                                var expectedMinutes = Convert.ToDouble(tripVM.TotalMinutes);
                                                TimeSpan timeSpan = TimeSpan.FromMinutes(expectedMinutes);
                                                int? baseprice = 0;
                                                // Extract hours and minutes
                                                int hours = timeSpan.Hours;
                                                var hoursdata = await _context.Hours.Where(c => c.TripVarientId == tripVM.TripvarientId && c.HoursName == hours).FirstOrDefaultAsync();
                                                if (hoursdata != null)
                                                {
                                                    string startTimeString = tripVM.TripTypeStarttime;
                                                    string endTimeString = tripVM.TripTypeEndtime;
                                                    if (tripVM.StartDate.HasValue && tripVM.EndDate.HasValue)
                                                    {
                                                        DateTime startDateTime = tripVM.StartDate.Value.Add(DateTime.ParseExact(tripVM.TripTypeStarttime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay);
                                                        DateTime endDateTime = tripVM.EndDate.Value.Add(DateTime.ParseExact(tripVM.TripTypeEndtime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay);
                                                        if (tripVM.StartDate.Value <= startDateTime || tripVM.EndDate.Value >= endDateTime)
                                                        {
                                                            baseprice = hoursdata.NightCharges;
                                                        }
                                                        else
                                                        {
                                                            baseprice = hoursdata.Charges;
                                                        }

                                                    }
                                                }
                                                tripVM.ExpectedDistance = trip.ExpectedDistance;

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
                                           .FirstOrDefaultAsync();
                                                tripVM.ActualEndTime = trip.ActualEndTime.ToString();
                                                tripVM.StartTime= trip.StartDateTime.ToString();
                                                tripVM.DriverPrice = trip.DriversPrice.ToString();
                                                TimeSpan? timeDifference = trip.ActualEndTime - trip.StartDateTime;
                                                if (timeDifference.HasValue)
                                                {
                                                    double hoursDifference = timeDifference.Value.TotalHours;
                                                    // Assign the calculated hours to ActualEndTime or any other property as needed
                                                    trip.ActualEndTime = DateTime.Now;
                                                    trip.NoOfHoursActual = Convert.ToInt32(hoursDifference);

                                                    var difference = (Convert.ToInt32(trip.NoOfHoursActual) - Convert.ToInt32(trip.NoOfHoursSelected));
                                                    var hoursdatas = _context.Hours
                                                      .Where(c => c.TripTypeId == trip.TripTypeId && c.HoursName <= difference)
                                                      .OrderByDescending(c => c.HoursName) // Order by HoursName in descending order
                                                      .FirstOrDefault();


                                                    if (hoursdatas != null)
                                                    {
                                                        if (difference <= 0)
                                                        {
                                                            trip.TotalTripValue = trip.EstimatedPrice;
                                                            var driversubdata = await _context.Driversubscriptions
                                                              .Where(c => c.DriverId == trip.DriverId && c.Expirydate <= DateTime.Now)
                                                              .OrderByDescending(c => c.DriversubscriptionId) // Sort by the latest date first
                                                              .FirstOrDefaultAsync();
                                                            if (driversubdata != null)
                                                            {
                                                                var subscriptiondata = _context.Subscriptions.Find(driversubdata.SubscriptionId);
                                                                if (subscriptiondata != null)
                                                                {
                                                                    var percentage = subscriptiondata.Percentage;
                                                                    if (percentage > 0)
                                                                    {
                                                                        int? anonymuscharges = 0;
                                                                        var anonymusdata = _context.Anonymoustripcharges.Where(c => c.TriptypeId == trip.TripTypeId).FirstOrDefault();
                                                                        if (anonymusdata != null)
                                                                        {
                                                                            anonymuscharges = anonymusdata.Amount;
                                                                        }
                                                                        careprice = anonymuscharges;
                                                                        var tripprice = trip.TotalTripValue;
                                                                        var taxvalue = Convert.ToInt32(tripprice) * (taxpercentages / 100);
                                                                        var drivervalue = (Convert.ToInt32(tripprice) - taxvalue - careprice) * (percentage / 100);
                                                                        trip.DriversPrice = Math.Round(drivervalue ?? 0, 0);

                                                                        
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            int minutes = difference * 60;
                                                            decimal totalprice = 0;
                                                            var tripvarientsdata = _context.TripVariants.Where(c => c.TriptypeId == trip.TripTypeId).FirstOrDefault();
                                                            if (tripvarientsdata != null)
                                                            {
                                                                totalprice = minutes * Convert.ToInt32(tripvarientsdata.ChargesperMinute);
                                                            }
                                                            var driversubdata = _context.Driversubscriptions.Where(c => c.DriverId == trip.DriverId && c.Expirydate <= DateTime.Now).FirstOrDefault();
                                                            if (driversubdata != null)
                                                            {
                                                                var subscriptiondata = _context.Subscriptions.Find(driversubdata.SubscriptionId);
                                                                if (subscriptiondata != null)
                                                                {
                                                                    var percentage = Convert.ToInt32(subscriptiondata.Percentage);
                                                                    if (percentage > 0)
                                                                    {
                                                                        trip.TotalTripValue = Math.Abs(Convert.ToDecimal(trip.EstimatedPrice) + totalprice);
                                                                        decimal totalValue = Convert.ToDecimal(trip.EstimatedPrice) + Convert.ToDecimal(totalprice);
                                                                        var taxvalue = Convert.ToInt32(totalValue) * (taxpercentages / 100);
                                                                        int? anonymuscharges = 0;
                                                                        var anonymusdata = _context.Anonymoustripcharges.Where(c => c.TriptypeId == trip.TripTypeId).FirstOrDefault();
                                                                        if (anonymusdata != null)
                                                                        {
                                                                            anonymuscharges = anonymusdata.Amount;
                                                                        }

                                                                        // Calculate drivervalue using decimal arithmetic to preserve precision
                                                                        decimal drivervalue = (totalValue - Convert.ToDecimal(taxvalue) - Convert.ToDecimal(careprice) - Convert.ToInt32(anonymuscharges)) * percentage / 100;
                                                                        trip.DriversPrice = Math.Round(drivervalue, 0);
                                                                        // Convert drivervalue to string for assignment
                                                                        trip.DriversPrice = drivervalue;
                                                                     
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                var percentage = 80;
                                                                if (percentage > 0)
                                                                {
                                                                    trip.TotalTripValue = Math.Abs(Math.Round(Convert.ToDecimal(trip.EstimatedPrice), 1) + totalprice);
                                                                    // Convert EstimatedPrice and totalprice to decimal and add them
                                                                    decimal totalValue = Convert.ToDecimal(trip.EstimatedPrice) + Convert.ToDecimal(totalprice);

                                                                    // Calculate drivervalue using decimal arithmetic to preserve precision
                                                                    var taxvalue = Convert.ToInt32(totalValue) * (taxpercentages / 100);

                                                                    // Calculate drivervalue using decimal arithmetic to preserve precision
                                                                    decimal drivervalue = (totalValue - Convert.ToDecimal(taxvalue) - Convert.ToDecimal(careprice)) * percentage / 100;

                                                                    trip.DriversPrice = Math.Round(drivervalue, 0);
                                                                    
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        trip.TotalTripValue = trip.EstimatedPrice;
                                                        var tripprice = trip.TotalTripValue;
                                                        var driversubdata = _context.Driversubscriptions.Where(c => c.DriverId == trip.DriverId && c.Expirydate <= DateTime.Now).FirstOrDefault();
                                                        if (driversubdata != null)
                                                        {

                                                            var subscriptiondata = _context.Subscriptions.Find(driversubdata.SubscriptionId);
                                                            if (subscriptiondata != null)
                                                            {
                                                                var percentage = subscriptiondata.Percentage;
                                                                if (percentage > 0)
                                                                {
                                                                    // Calculate drivervalue using decimal arithmetic to preserve precision
                                                                    var taxvalue = Convert.ToInt32(tripprice) * (taxpercentages / 100);
                                                                    int? anonymuscharges = 0;
                                                                    var anonymusdata = _context.Anonymoustripcharges.Where(c => c.TriptypeId == trip.TripTypeId).FirstOrDefault();
                                                                    if (anonymusdata != null)
                                                                    {
                                                                        anonymuscharges = anonymusdata.Amount;
                                                                    }

                                                                    // Calculate drivervalue using decimal arithmetic to preserve precision
                                                                    var drivervalue = (Convert.ToInt32(trip.TotalTripValue) - Convert.ToDecimal(taxvalue) - Convert.ToDecimal(careprice) - anonymuscharges) * percentage / 100;

                                                                    trip.DriversPrice = drivervalue;

                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            var percentage = 80;
                                                            if (percentage > 0)
                                                            {
                                                                trip.TotalTripValue = Math.Abs(Convert.ToDecimal(trip.EstimatedPrice));
                                                                // Convert EstimatedPrice and totalprice to decimal and add them
                                                                decimal totalValue = Convert.ToDecimal(trip.EstimatedPrice);

                                                                var taxvalue = Convert.ToInt32(totalValue) * (taxpercentages / 100);
                                                                int? anonymuscharges = 0;
                                                                var anonymusdata = _context.Anonymoustripcharges.Where(c => c.TriptypeId == trip.TripTypeId).FirstOrDefault();
                                                                if (anonymusdata != null)
                                                                {
                                                                    anonymuscharges = anonymusdata.Amount;
                                                                }
                                                                // Calculate drivervalue using decimal arithmetic to preserve precision
                                                                decimal drivervalue = (totalValue - Convert.ToDecimal(taxvalue) - Convert.ToDecimal(careprice)) * percentage / 100;

                                                                // Convert drivervalue to string for assignment
                                                                trip.DriversPrice = drivervalue;
                                                                
                                                            }
                                                        }
                                                    }


                                                    // Optionally, update StartDateTime if necessary
                                                    // tripdata.StartDateTime = tripdata.StartDateTime; // This line seems redundant unless you have a specific reason to reassign StartDateTime to itself


                                                }
                                                else
                                                {
                                                    trip.TotalTripValue = trip.EstimatedPrice;
                                                    var tripprice = trip.TotalTripValue;
                                                    var driversubdata = _context.Driversubscriptions.Where(c => c.DriverId == trip.DriverId && c.Expirydate <= DateTime.Now).FirstOrDefault();
                                                    if (driversubdata != null)
                                                    {
                                                        var subscriptiondata = _context.Subscriptions.Find(driversubdata.SubscriptionId);
                                                        if (subscriptiondata != null)
                                                        {
                                                            var percentage = subscriptiondata.Percentage;
                                                            if (percentage > 0)
                                                            {
                                                                int? anonymuscharges = 0;
                                                                var anonymusdata = _context.Anonymoustripcharges.Where(c => c.TriptypeId == trip.TripTypeId).FirstOrDefault();
                                                                if (anonymusdata != null)
                                                                {
                                                                    anonymuscharges = anonymusdata.Amount;
                                                                }
                                                                var taxvalue = Convert.ToInt32(tripprice) * (taxpercentages / 100);

                                                                // Calculate drivervalue using decimal arithmetic to preserve precision
                                                                var drivervalue = (Convert.ToInt32(tripprice) - taxvalue - Convert.ToDecimal(careprice) - anonymuscharges) * (percentage / 100);
                                                                trip.DriversPrice = drivervalue;
                                                                
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        var percentage = 80;
                                                        if (percentage > 0)
                                                        {
                                                            trip.TotalTripValue = Math.Abs(Convert.ToDecimal(trip.EstimatedPrice));
                                                            tripprice = trip.TotalTripValue;
                                                            // Convert EstimatedPrice and totalprice to decimal and add them
                                                            decimal totalValue = Convert.ToDecimal(trip.EstimatedPrice);
                                                            var taxvalue = Convert.ToInt32(totalValue) * (taxpercentages / 100);
                                                            // Calculate drivervalue using decimal arithmetic to preserve precision
                                                            decimal drivervalue = (totalValue - Convert.ToDecimal(taxvalue) - Convert.ToDecimal(careprice)) * percentage / 100;

                                                            // Convert drivervalue to string for assignment
                                                            trip.DriversPrice = Math.Round(drivervalue,0);
                                                            
                                                        }
                                                    }
                                                }      
                                                tripVM.DriverPrice = trip.DriversPrice.ToString();
                                                var expectedextradiatancethanbaseLimit = (Convert.ToDecimal(tripVM.ExpectedDistance) - Convert.ToDecimal(tripVM.KilometerLimit));
                                                decimal? expectedtotalpriceforextradistance = 0;
                                                if (expectedextradiatancethanbaseLimit > 0)
                                                {
                                                    expectedtotalpriceforextradistance = expectedextradiatancethanbaseLimit * tripVM.PricePerKilometers;

                                                }

                                                var expectedtotalprice = expectedtotalpriceforextradistance + baseprice + tripVM.Aerialpriceperkilometer + tripVM.NightCharges;

                                                var cuponpercentagevalue = expectedtotalprice * tripVM.CuponPercentage / 100;

                                                var expectedtotalvaluewithouttax = expectedtotalprice - cuponpercentagevalue;//240

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

                                                var tax = await _context.Taxes.FirstOrDefaultAsync();
                                                
                                                if (tax!=null)
                                                {
                                                   
                                                        var taxpercentage = Convert.ToInt32(tax.Percentage);

                                                        var taxvalue = Convert.ToDecimal(expectedtotalvaluewithouttax) * taxpercentage / 100;
                                                    tripVM.triptaxvalue = taxvalue;
                                                        var travelledtaxvalue = Convert.ToDecimal(expectedtotalvaluewithouttax) * taxpercentage / 100;

                                                        travelledtotaltaxvalue += travelledtaxvalue;//19.20
                                                }

                                                tripVM.ExpectedTotalTripvalue = (expectedtotalvaluewithouttax + travelledtotaltaxvalue + securetaxvalue);//488
                                                Decimal? totalvaluewithouttax = 0;
                                                if (tripVM.IsTripCompByDriver == true)
                                                {

                                                    var extradiatancethanbaseLimit = Convert.ToDecimal(travelledonroaddistance) - Convert.ToDecimal(tripVM.KilometerLimit);

                                                    decimal? totalpriceforextradistance = 0;

                                                    if (extradiatancethanbaseLimit > 0)
                                                    {

                                                        totalpriceforextradistance = extradiatancethanbaseLimit * tripVM.PricePerKilometers;
                                                    }


                                                    var totalprice = totalpriceforextradistance + baseprice + tripVM.Aerialpriceperkilometer;

                                                    var actualcuponpercentagevalue = totalprice * tripVM.CuponPercentage / 100;

                                                    totalvaluewithouttax = totalprice - actualcuponpercentagevalue;

                                                }
                                                tripVM.TotalTripValue = Convert.ToString(totalvaluewithouttax + travelledtotaltaxvalue + securetaxvalue + tripVM.NightCharges);


                                            }
                                            else
                                            {
                                                var expectedMinutes = Convert.ToDouble(tripVM.TotalMinutes);
                                                TimeSpan timeSpan = TimeSpan.FromMinutes(expectedMinutes);
                                                int? baseprice = 0;
                                                // Extract hours and minutes
                                                int hours = timeSpan.Hours;
                                                var hoursdata = await _context.Hours.Where(c => c.TripVarientId == tripVM.TripvarientId && c.HoursName == hours).FirstOrDefaultAsync();
                                                if (hoursdata != null)
                                                {
                                                    string startTimeString = tripVM.TripTypeStarttime;
                                                    string endTimeString = tripVM.TripTypeEndtime;
                                                    if (tripVM.StartDate.HasValue && tripVM.EndDate.HasValue)
                                                    {
                                                        DateTime startDateTime = tripVM.StartDate.Value.Add(DateTime.ParseExact(tripVM.TripTypeStarttime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay);
                                                        DateTime endDateTime = tripVM.EndDate.Value.Add(DateTime.ParseExact(tripVM.TripTypeEndtime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay);
                                                        if (tripVM.StartDate.Value <= startDateTime || tripVM.EndDate.Value >= endDateTime)
                                                        {
                                                            baseprice = hoursdata.NightCharges;
                                                        }
                                                        else
                                                        {
                                                            baseprice = hoursdata.Charges;
                                                        }

                                                    }
                                                }
                                                var expectedPrice = baseprice;
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
                                                tripVM.ExpectedTotalTripvalue = (expectedTotalPricewithouttax + travelledtotaltaxvalue + securetaxvalue);
                                                Decimal? totalvaluewithouttax = 0;
                                                if (tripVM.IsTripCompByDriver == true)
                                                {

                                                    var totalMinutes = Convert.ToDouble(tripVM.TotalMinutes);
                                                    TimeSpan actualtimeSpan = TimeSpan.FromMinutes(totalMinutes);
                                                    int? actualbaseprice = 0;
                                                    // Extract hours and minutes
                                                    int actualhours = timeSpan.Hours;
                                                    var actualhoursdata = await _context.Hours.Where(c => c.TripVarientId == tripVM.TripvarientId && c.HoursName == actualhours).FirstOrDefaultAsync();
                                                    if (actualhoursdata != null)
                                                    {
                                                        string startTimeString = tripVM.TripTypeStarttime;
                                                        string endTimeString = tripVM.TripTypeEndtime;
                                                        if (tripVM.StartDate.HasValue && tripVM.EndDate.HasValue)
                                                        {
                                                            DateTime startDateTime = tripVM.StartDate.Value.Add(DateTime.ParseExact(tripVM.TripTypeStarttime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay);
                                                            DateTime endDateTime = tripVM.EndDate.Value.Add(DateTime.ParseExact(tripVM.TripTypeEndtime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay);
                                                            if (tripVM.StartDate.Value <= startDateTime || tripVM.EndDate.Value >= endDateTime)
                                                            {
                                                                actualbaseprice = hoursdata.NightCharges;
                                                            }
                                                            else
                                                            {
                                                                actualbaseprice = hoursdata.Charges;
                                                            }

                                                        }
                                                    }
                                                    var actualtotalMinutes = tripVM.ActualTotalMinutes;
                                                    var totalextraminutes = 0;
                                                    if (actualtotalMinutes > totalMinutes)
                                                    {
                                                        totalextraminutes = Convert.ToInt32(actualtotalMinutes - totalMinutes);
                                                    }
                                                    else
                                                    {
                                                        totalextraminutes = 0;
                                                    }
                                                    var price = tripVM.ChargesperMinute * Convert.ToDecimal(totalextraminutes);

                                                    totalvaluewithouttax = actualbaseprice + price + tripVM.NightCharges - cuponpercentagevalue;

                                                }
                                                tripVM.TotalTripValue = Convert.ToString(totalvaluewithouttax + travelledtotaltaxvalue + securetaxvalue + tripVM.NightCharges);

                                            }

                                            tripVMList.Add(tripVM);
                                        }
                                    }
                                }
                            }
                            return Ok(tripVMList);
                        }
                        else
                        {
                            return NoContent();
                        }
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




        private async Task<ActionResult<Trip>> Udatetriptoinstant(int tripid)
        {
            var tripdata = await _context.Trips.FindAsync(tripid);
            if (tripdata != null)
            {
                tripdata.DriverId = null;
                tripdata.IsTimeScheduled = false;
            }
            _context.Entry(tripdata).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return tripdata;
        }
       
        
        
        [HttpGet("FLAG")]
        public async Task<ActionResult<IEnumerable<TripVM>>> GetofdriverTrips(int id)
        {
            try
            {
                var drivers = _context.Drivers.Where(c => c.DriverId == id && c.IsDriverActive ==true).FirstOrDefault() ;
                var ignoretrips = _context.IgnoredTrips.Where(c => c.DriverId == id).ToList();
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
                if (drivers != null)
                {
                    var tripdata = _context.Trips.Where(c => c.DriverId == id && c.IsTripCompByDriver == true).ToList();

                    var tripVMList = new List<TripVM>();

                    if (tripdata.Count > 0)
                    {

                        foreach (var trip in tripdata)
                        {
                            var ignoredtripsofdriver = ignoretrips.Where(c => c.FlexiId == trip.TripId).FirstOrDefault();
                            if (ignoredtripsofdriver == null)
                            {
                                if (trip.StartDateTime <= DateTime.Now)
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
                                var driverDistance = distance(Convert.ToDouble(locatrip[0]), Convert.ToDouble(locatrip[1]), Convert.ToDouble(drivers.Latitude), Convert.ToDouble(drivers.Longitude), 'K');
                               if (driverDistance < distancetocover)
                                {
                                    var tripVM = new TripVM();
                                    tripVM.TripId = trip.TripId;
                                    tripVM.IsAccepted = trip.IsAccepted;
                                    tripVM.TripsUniqueId = trip.TripsUniqueId;
                                    tripVM.ExpectedDistance = Math.Round(Convert.ToDecimal(driverDistance), 2);
                                    tripVM.DriverPrice = trip.DriversPrice.ToString();
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

                                    tripVM.IsReserved = trip.IsReserved;
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
                                    tripVM.ImageUrlsList = trip.ImageUrlsList;
                                    if (tripVM.ImageUrlsList != null && tripVM.ImageUrlsList != "")
                                    {
                                        var imageurls = trip.ImageUrlsList.Split(',').ToList();
                                        var imageurlslist = new List<Imageurls>();
                                        if (imageurls.Count > 0)
                                        {
                                            foreach (var imageUrl in imageurls)
                                            {
                                                var imagevm = new Imageurls();
                                                imagevm.Images = imageUrl;
                                                imageurlslist.Add(imagevm);
                                            }
                                        }
                                        tripVM.imageurls = imageurlslist;
                                    }

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
                                    Decimal? taxpercentages = 0;
                                    var taxdata = await _context.Taxes.FirstOrDefaultAsync();
                                    if (taxdata != null)
                                    {
                                        taxpercentages = taxdata.Percentage;

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
                                    }
                                    else
                                    {
                                        trip.TotalTripValue = Math.Abs(Convert.ToDecimal(trip.EstimatedPrice));
                                        decimal totalValue = Convert.ToDecimal(trip.EstimatedPrice);
                                        var taxvalue = Convert.ToInt32(totalValue) * (taxpercentages / 100);


                                        // Calculate drivervalue using decimal arithmetic to preserve precision
                                        decimal drivervalue = (totalValue - Convert.ToDecimal(taxvalue) - Convert.ToDecimal(careprice)) * 80 / 100;
                                    }
                                    tripVM.DriverPrice = trip.DriversPrice.ToString();
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

                                        var actualgoogledistanceresult = await GetGoogleDistance(currentLatitude.ToString(), currentLatitude.ToString(), nextLatitude.ToString(), nextLongitude.ToString());
                                        var drivertravelledDistance = distance(Convert.ToDouble(currentLatitude), Convert.ToDouble(currentLatitude), Convert.ToDouble(nextLatitude), Convert.ToDouble(nextLongitude), 'K');
                                        int actualgoogledistance = 0;
                                        if (actualgoogledistanceresult.Result == null)
                                        {

                                            actualgoogledistance = Convert.ToInt32(actualgoogledistanceresult.Value);
                                        }
                                        else
                                        {

                                            actualgoogledistance = Convert.ToInt32(actualgoogledistanceresult);
                                        }

                                        travelledonroaddistance += Convert.ToInt32(drivertravelledDistance);

                                    }

                                    var entityDistance = distance(Convert.ToDouble(tripVM.FromLatitude), Convert.ToDouble(tripVM.FromLongitude), Convert.ToDouble(tripVM.ToLatitude), Convert.ToDouble(tripVM.ToLongitude), 'K');


                                    tripVM.AerialDistance = Math.Round(Convert.ToDecimal(entityDistance), 2);

                                    var aerialDistancedata = _context.AerialDistancePrices.ToList();

                                    if (aerialDistancedata.Count > 0)
                                    {

                                        tripVM.Aerialpriceperkilometer = Convert.ToDecimal(aerialDistancedata[0].AerialDistancePriceperKilometer);
                                    }

                                    tripVM.ExpectedDistance = trip.ExpectedDistance;
                                    tripVM.DistanceTravelled = travelledonroaddistance;

                                    tripVM.CuponId = tripVM.CuponId;

                                    var cupondata = _context.Cupons.Find(tripVM.CuponId);

                                    if (cupondata != null)
                                    {

                                        tripVM.CuponName = cupondata.CuponName;

                                        tripVM.CuponCode = cupondata.CuponCode;

                                        tripVM.CuponPercentage = cupondata.Percentage;
                                    }else if(cupondata == null && tripVM.CuponPercentage == 0 && tripVM.CuponPercentage == null)
                                    {

                                        tripVM.CuponPercentage = 0;
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

                            }

                        }
                        return Ok(tripVMList);
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


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }





    
        [HttpGet("GetTripswithacepted")]
        public async Task<ActionResult<IEnumerable<TripVM>>> GetTripswithacepted(int id)
        {
            try
            {
                var drivers = _context.Drivers.Where(c => c.DriverId == id && c.IsDriverActive == true).FirstOrDefault(); ;
                var ignoretrips = _context.IgnoredTrips.Where(c => c.DriverId == id).ToList();
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
                if (drivers != null)
                {

                    string transmissionTypesString = drivers.TransmissionTypeId;
                    string[] transmissionTypeStrings = new string[] { };
                    if (drivers.TransmissionTypeId != string.Empty && drivers.TransmissionTypeId != null)
                    {

                        transmissionTypeStrings = transmissionTypesString.Split(',');


                    }
                    List<int> transmissionTypes = transmissionTypeStrings.Select(int.Parse).ToList();
                    string vehicletypestring = drivers.VehicleTypeIds;
                    string[] vehicletypeStrings = new string[] { };
                    if (vehicletypestring != string.Empty && drivers.TransmissionTypeId != null)
                    {

                        vehicletypeStrings = vehicletypestring.Split(',');
                    }

                    List<int> vehicletypes = vehicletypeStrings.Select(int.Parse).ToList();
                    var tripdata = new List<Trip>();
                    if (transmissionTypesString != null && transmissionTypesString != string.Empty && vehicletypestring != string.Empty && drivers.TransmissionTypeId != null)
                    {
                        var driverId = id; // Specify the DriverId to check
                        tripdata = _context.Trips
                       .Where(c => (c.DriverId == 0 || c.DriverId == null )&&c.IsCancelled!=true
                                  && (transmissionTypes.Contains(c.TransmissionTypeId.Value) || !c.TransmissionTypeId.HasValue)
                                  && (vehicletypes.Contains(c.VehicleTypeId.Value) || !c.VehicleTypeId.HasValue))
                       .ToList();

                      //&& c.StartDateTime >= DateTime.Now

                        // Filter based on acceptance status
                        var hasAcceptedTripsForDriver = tripdata.Any(t => t.IsAccepted!=true);

                        if (hasAcceptedTripsForDriver)
                        {
                            // If there are accepted trips for the specified DriverId, filter to show only those trips that are not accepted by any driver
                            tripdata = tripdata.Where(t => t.IsAccepted != true).ToList();
                        }

                    }
                    else
                    {
                        return BadRequest("Please contact admin to update vehicletypes ");
                    }
                    var driverwallet = _context.Driverwallets.Where(c => c.DriverId == drivers.DriverId).FirstOrDefault();
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

                        var tripVMList = new List<TripVM>();

                        if (tripdata.Count > 0)
                        {

                            foreach (var trip in tripdata)
                            {
                                var ignoredtripsofdriver = ignoretrips.Where(c => c.FlexiId == trip.TripId).FirstOrDefault();
                                if (trip.FromLocation != null && trip.IsAccepted!=true&& trip.FromLocation != string.Empty)
                                {

                                    if (ignoredtripsofdriver == null)
                                    {
                                        var locatrip = trip.FromLocation.Split(',').ToList();
                                        var driverDistance = distance(Convert.ToDouble(locatrip[0]), Convert.ToDouble(locatrip[1]), Convert.ToDouble(drivers.Latitude), Convert.ToDouble(drivers.Longitude), 'K');
                                        var tripVM = new TripVM();
                                        //tripVM.IsTimeScheduled = trip.IsTimeScheduled;
                                        if (driverDistance < distancetocover)
                                        {
                                            if (trip.IsTimeScheduled == true)
                                            {
                                                var dataconfiguretime = _context.Schuduletriptimechagemodel.Where(c => c.TripTypeId == trip.TripTypeId).FirstOrDefault();
                                                if (dataconfiguretime != null)
                                                {
                                                    TimeZoneInfo istTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                                                    DateTime istStartDateTime = TimeZoneInfo.ConvertTimeFromUtc(trip.StartDateTime.Value, istTimeZone);
                                                    DateTime currentIstTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, istTimeZone);

                                                    DateTime currentIstTimePlus45Minutes = currentIstTime.AddMinutes(Convert.ToInt32(dataconfiguretime.SchuduletripDuration));
                                                    if (trip.IsReserved == true)
                                                    {

                                                        DateTime? startTimes = trip.StartDateTime;

                                                        var starttimes = String.Format("{0:h:mm tt}", startTimes);

                                                        currentIstTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, istTimeZone);
                                                        // Assuming tripVM.StartTime is a string representing a date and time
                                                        tripVM.StartTime = starttimes;

                                                        // Convert StartTime string to DateTime
                                                        DateTime startTimeDateTime = DateTime.Parse(tripVM.StartTime);

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
                                                        tripVM.Accepttimefrom = AcceptTimeFrom;
                                                        tripVM.AccepttimeTo = AcceptTimeFrom.AddMinutes(15);
                                                    }

                                                    if (istStartDateTime < currentIstTimePlus45Minutes)
                                                    {
                                                        await Udatetriptoinstant(trip.TripId);
                                                        tripVM.IsTimeScheduled = false;
                                                    }
                                                }
                                            }
                                            tripVM.TripId = trip.TripId;
                                            tripVM.IsAccepted = trip.IsAccepted;
                                            tripVM.TripsUniqueId = trip.TripsUniqueId;
                                            tripVM.ExpectedDistance = Math.Round(Convert.ToDecimal(driverDistance), 2);

                                            tripVM.UserId = trip.UserId;
                                            tripVM.NoOfHoursSelected = trip.NoOfHoursSelected;
                                            var userdata = _context.Users.Find(tripVM.UserId);

                                            if (userdata != null)
                                            {
                                                tripVM.UserName = userdata.Name;
                                                tripVM.UserPhoneNumber = userdata.PhoneNumber;
                                                tripVM.Image = userdata.UserImage;
                                            }

                                            tripVM.DriverId = id;
                                            tripVM.IsReserved = trip.IsReserved;
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
                                            tripVM.ImageUrlsList = trip.ImageUrlsList;
                                            if (tripVM.ImageUrlsList != null && tripVM.ImageUrlsList != "")
                                            {
                                                var imageurls = trip.ImageUrlsList.Split(',').ToList();
                                                var imageurlslist = new List<Imageurls>();
                                                if (imageurls.Count > 0)
                                                {
                                                    foreach (var imageUrl in imageurls)
                                                    {
                                                        var imagevm = new Imageurls();
                                                        imagevm.Images = imageUrl;
                                                        imageurlslist.Add(imagevm);
                                                    }
                                                }
                                                tripVM.imageurls = imageurlslist;
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

                                                var actualgoogledistanceresult = await GetGoogleDistance(currentLatitude.ToString(), currentLatitude.ToString(), nextLatitude.ToString(), nextLongitude.ToString());
                                                int actualgoogledistance = 0;
                                                if (actualgoogledistanceresult.Result == null)
                                                {

                                                    actualgoogledistance = Convert.ToInt32(actualgoogledistanceresult.Value);
                                                }
                                                else
                                                {

                                                    actualgoogledistance = Convert.ToInt32(actualgoogledistanceresult);
                                                }

                                                travelledonroaddistance += actualgoogledistance;

                                            }

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
                                            } else if  (tripVM.CuponName == null && tripVM.CuponPercentage == 0 && tripVM.CuponPercentage == null)
                                            {

                                                tripVM.CuponPercentage = 0;
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
                                            Decimal? taxpercentages = 0;
                                            var taxdata = await _context.Taxes.FirstOrDefaultAsync();
                                            if (taxdata != null)
                                            {
                                                taxpercentages = taxdata.Percentage;

                                            }
                                            if (driversubscriptiondata != null)
                                            {
                                                //trip.TotalTripValue = Math.Abs(Convert.ToDecimal(trip.EstimatedPrice)).ToString();
                                                //decimal totalValue = Convert.ToDecimal(trip.EstimatedPrice);
                                                //var taxvalue = Convert.ToInt32(totalValue) * (taxpercentages / 100);
                                                //int? anonymuscharges = 0;
                                                //var anonymusdata = _context.Anonymoustripcharges.Where(c => c.TriptypeId == trip.TripTypeId).FirstOrDefault();
                                                //if (anonymusdata != null)
                                                //{
                                                //    anonymuscharges = anonymusdata.Amount;
                                                //}

                                                //// Calculate drivervalue using decimal arithmetic to preserve precision
                                                //decimal drivervalue = (totalValue - Convert.ToDecimal(taxvalue) - Convert.ToDecimal(careprice) - Convert.ToInt32(anonymuscharges));
                                                //tripVM.DriverPrice = drivervalue.ToString();
                                                int baseHourCharge = trip.hoursCharge ?? 0;
                                                int returnCharge = trip.driverReturnCharges ?? 0;
                                                var nightCharge = tripVM.NightCharges ?? 0;
                                                // Taxes & Fees: ₹25 fixed + subscrption 18% GST on (hour + night + return)
                                                decimal taxBase = baseHourCharge + returnCharge + nightCharge;
                                                decimal taxGst = taxBase * 18 / 100;
                                                decimal taxAndFee = 25 + taxGst;
                                                trip.taxandfee = Convert.ToInt32(taxAndFee);

                                                // Driver fee 
                                                decimal driverFee = taxBase; // hour + night + return only
                                                                             //trip.DriverFee = Math.Round(driverFee, 2);
                                                tripVM.DriverPrice = driverFee.ToString();
                                            }
                                            else
                                            {
                                                //trip.TotalTripValue = Math.Abs(Convert.ToDecimal(trip.EstimatedPrice)).ToString();
                                                //decimal totalValue = Convert.ToDecimal(trip.EstimatedPrice);
                                                //var taxvalue = Convert.ToInt32(totalValue) * (taxpercentages / 100);


                                                //// Calculate drivervalue using decimal arithmetic to preserve precision
                                                //decimal drivervalue = (totalValue - Convert.ToDecimal(taxvalue) - Convert.ToDecimal(careprice)) * 80 / 100;
                                                //tripVM.DriverPrice = drivervalue.ToString();
                                                int baseHourCharge = trip.hoursCharge ?? 0;
                                                int returnCharge = trip.driverReturnCharges ?? 0;
                                                var nightCharge = tripVM.NightCharges ?? 0;
                                                // Taxes & Fees: ₹25 fixed + 5% GST on (hour + night + return)
                                                decimal taxBase = baseHourCharge + returnCharge + nightCharge;
                                                decimal taxGst = taxBase * 5 / 100;
                                                decimal taxAndFee = 25 + taxGst;
                                                trip.taxandfee = Convert.ToInt32(taxAndFee);

                                                // Driver fee 
                                                decimal driverFee = taxBase; // hour + night + return only
                                                                             //trip.DriverFee = Math.Round(driverFee, 2);
                                                tripVM.DriverPrice = driverFee.ToString();
                                            }
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
                                           driversubscriptiondata = await _context.Driversubscriptions
                                       .Where(c => c.DriverId == id)
                                       .OrderByDescending(c => c.DriversubscriptionId) // Sort by the latest date first
                                       .FirstOrDefaultAsync();
                                            tripVM.ActualEndTime = trip.ActualEndTime.ToString();
                                            tripVM.StartTime = trip.StartDateTime.ToString();
                                            tripVM.DriverPrice = trip.DriversPrice.ToString();
                                            TimeSpan? timeDifference = trip.ActualEndTime - trip.StartDateTime;
                                            if (timeDifference.HasValue)
                                            {
                                                double hoursDifference = timeDifference.Value.TotalHours;
                                                // Assign the calculated hours to ActualEndTime or any other property as needed
                                                trip.ActualEndTime = DateTime.Now;
                                                trip.NoOfHoursActual = Convert.ToInt32(hoursDifference);

                                                var difference = (Convert.ToInt32(trip.NoOfHoursActual) - Convert.ToInt32(trip.NoOfHoursSelected));
                                                var hoursdatas = _context.Hours
                                                  .Where(c => c.TripTypeId == trip.TripTypeId && c.HoursName <= difference)
                                                  .OrderByDescending(c => c.HoursName) // Order by HoursName in descending order
                                                  .FirstOrDefault();


                                                if (hoursdatas != null)
                                                {
                                                    if (difference <= 0)
                                                    {
                                                        trip.TotalTripValue = trip.EstimatedPrice;
                                                        var driversubdata = await _context.Driversubscriptions
                                                          .Where(c => c.DriverId == trip.DriverId && c.Expirydate <= DateTime.Now)
                                                          .OrderByDescending(c => c.DriversubscriptionId) // Sort by the latest date first
                                                          .FirstOrDefaultAsync();
                                                        if (driversubdata != null)
                                                        {
                                                            var subscriptiondata = _context.Subscriptions.Find(driversubdata.SubscriptionId);
                                                            if (subscriptiondata != null)
                                                            {
                                                                //var percentage = subscriptiondata.Percentage;
                                                                //if (percentage > 0)
                                                                //{
                                                                //    int? anonymuscharges = 0;
                                                                //    var anonymusdata = _context.Anonymoustripcharges.Where(c => c.TriptypeId == trip.TripTypeId).FirstOrDefault();
                                                                //    if (anonymusdata != null)
                                                                //    {
                                                                //        anonymuscharges = anonymusdata.Amount;
                                                                //    }
                                                                //    careprice = anonymuscharges;
                                                                //    var tripprice = trip.TotalTripValue;
                                                                //    var taxvalue = Convert.ToInt32(tripprice) * (taxpercentages / 100);
                                                                //    var drivervalue = (Convert.ToInt32(tripprice) - taxvalue - careprice) * (percentage / 100);
                                                                //    trip.DriversPrice = Math.Round(drivervalue ?? 0, 0).ToString();


                                                                //}
                                                                int baseHourCharge = trip.hoursCharge ?? 0;
                                                                int returnCharge = trip.driverReturnCharges ?? 0;
                                                                var nightCharge = tripVM.NightCharges ?? 0;
                                                                // Taxes & Fees: ₹25 fixed + 5% GST on (hour + night + return)
                                                                decimal taxBase = baseHourCharge + returnCharge + nightCharge;
                                                                decimal taxGst = taxBase * 18 / 100;
                                                                decimal taxAndFee = 25 + taxGst;
                                                                trip.taxandfee = Convert.ToInt32(taxAndFee);

                                                                // Driver fee 
                                                                decimal driverFee = taxBase; // hour + night + return only
                                                                                             //trip.DriverFee = Math.Round(driverFee, 2);
                                                                tripVM.DriverPrice = driverFee.ToString();
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        int minutes = difference * 60;
                                                        decimal totalprice = 0;
                                                        var tripvarientsdata = _context.TripVariants.Where(c => c.TriptypeId == trip.TripTypeId).FirstOrDefault();
                                                        if (tripvarientsdata != null)
                                                        {
                                                            totalprice = minutes * Convert.ToInt32(tripvarientsdata.ChargesperMinute);
                                                        }
                                                        var driversubdata = _context.Driversubscriptions.Where(c => c.DriverId == trip.DriverId && c.Expirydate <= DateTime.Now).FirstOrDefault();
                                                        if (driversubdata != null)
                                                        {
                                                            var subscriptiondata = _context.Subscriptions.Find(driversubdata.SubscriptionId);
                                                            if (subscriptiondata != null)
                                                            {
                                                                var percentage = Convert.ToInt32(subscriptiondata.Percentage);
                                                                if (percentage > 0)
                                                                {
                                                                    //trip.TotalTripValue = Math.Abs(Convert.ToDecimal(trip.EstimatedPrice) + totalprice).ToString();
                                                                    //decimal totalValue = Convert.ToDecimal(trip.EstimatedPrice) + Convert.ToDecimal(totalprice);
                                                                    //var taxvalue = Convert.ToInt32(totalValue) * (taxpercentages / 100);
                                                                    //int? anonymuscharges = 0;
                                                                    //var anonymusdata = _context.Anonymoustripcharges.Where(c => c.TriptypeId == trip.TripTypeId).FirstOrDefault();
                                                                    //if (anonymusdata != null)
                                                                    //{
                                                                    //    anonymuscharges = anonymusdata.Amount;
                                                                    //}

                                                                    //// Calculate drivervalue using decimal arithmetic to preserve precision
                                                                    //decimal drivervalue = (totalValue - Convert.ToDecimal(taxvalue) - Convert.ToDecimal(careprice) - Convert.ToInt32(anonymuscharges)) * percentage / 100;
                                                                    //trip.DriversPrice = Math.Round(drivervalue, 0).ToString();
                                                                    //// Convert drivervalue to string for assignment
                                                                    //trip.DriversPrice = drivervalue.ToString(CultureInfo.InvariantCulture);


                                                                    int baseHourCharge = trip.hoursCharge ?? 0;
                                                                    int returnCharge = trip.driverReturnCharges ?? 0;
                                                                    var nightCharge = tripVM.NightCharges ?? 0;
                                                                    // Taxes & Fees: ₹25 fixed + 5% GST on (hour + night + return)
                                                                    decimal taxBase = baseHourCharge + returnCharge + nightCharge;
                                                                    decimal taxGst = taxBase * 18 / 100;
                                                                    decimal taxAndFee = 25 + taxGst;
                                                                    trip.taxandfee = Convert.ToInt32(taxAndFee);

                                                                    // Driver fee 
                                                                    decimal driverFee = taxBase; // hour + night + return only
                                                                                                 //trip.DriverFee = Math.Round(driverFee, 2);
                                                                    tripVM.DriverPrice = driverFee.ToString();

                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            var percentage = 80;
                                                            if (percentage > 0)
                                                            {
                                                                //trip.TotalTripValue = Math.Abs(Math.Round(Convert.ToDecimal(trip.EstimatedPrice), 1) + totalprice).ToString();
                                                                //// Convert EstimatedPrice and totalprice to decimal and add them
                                                                //decimal totalValue = Convert.ToDecimal(trip.EstimatedPrice) + Convert.ToDecimal(totalprice);

                                                                //// Calculate drivervalue using decimal arithmetic to preserve precision
                                                                //var taxvalue = Convert.ToInt32(totalValue) * (taxpercentages / 100);

                                                                //// Calculate drivervalue using decimal arithmetic to preserve precision
                                                                //decimal drivervalue = (totalValue - Convert.ToDecimal(taxvalue) - Convert.ToDecimal(careprice)) * percentage / 100;

                                                                //trip.DriversPrice = Math.Round(drivervalue, 0).ToString();

                                                                int baseHourCharge = trip.hoursCharge ?? 0;
                                                                int returnCharge = trip.driverReturnCharges ?? 0;
                                                                var nightCharge = tripVM.NightCharges ?? 0;
                                                                // Taxes & Fees: ₹25 fixed + 5% GST on (hour + night + return)
                                                                decimal taxBase = baseHourCharge + returnCharge + nightCharge;
                                                                decimal taxGst = taxBase * 5 / 100;
                                                                decimal taxAndFee = 25 + taxGst;
                                                                trip.taxandfee = Convert.ToInt32(taxAndFee);

                                                                // Driver fee 
                                                                decimal driverFee = taxBase; // hour + night + return only
                                                                                             //trip.DriverFee = Math.Round(driverFee, 2);
                                                                tripVM.DriverPrice = driverFee.ToString();

                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    trip.TotalTripValue = trip.EstimatedPrice;
                                                    var tripprice = trip.TotalTripValue;
                                                    var driversubdata = _context.Driversubscriptions.Where(c => c.DriverId == trip.DriverId && c.Expirydate <= DateTime.Now).FirstOrDefault();
                                                    if (driversubdata != null)
                                                    {

                                                        var subscriptiondata = _context.Subscriptions.Find(driversubdata.SubscriptionId);
                                                        if (subscriptiondata != null)
                                                        {
                                                            var percentage = subscriptiondata.Percentage;
                                                            if (percentage > 0)
                                                            {
                                                                // Calculate drivervalue using decimal arithmetic to preserve precision
                                                                //var taxvalue = Convert.ToInt32(tripprice) * (taxpercentages / 100);
                                                                //int? anonymuscharges = 0;
                                                                //var anonymusdata = _context.Anonymoustripcharges.Where(c => c.TriptypeId == trip.TripTypeId).FirstOrDefault();
                                                                //if (anonymusdata != null)
                                                                //{
                                                                //    anonymuscharges = anonymusdata.Amount;
                                                                //}

                                                                //// Calculate drivervalue using decimal arithmetic to preserve precision
                                                                //var drivervalue = (Convert.ToInt32(trip.TotalTripValue) - Convert.ToDecimal(taxvalue) - Convert.ToDecimal(careprice) - anonymuscharges) * percentage / 100;

                                                                //trip.DriversPrice = drivervalue.ToString();


                                                                int baseHourCharge = trip.hoursCharge ?? 0;
                                                                int returnCharge = trip.driverReturnCharges ?? 0;
                                                                var nightCharge = tripVM.NightCharges ?? 0;
                                                                // Taxes & Fees: ₹25 fixed + 5% GST on (hour + night + return)
                                                                decimal taxBase = baseHourCharge + returnCharge + nightCharge;
                                                                decimal taxGst = taxBase * 18 / 100;
                                                                decimal taxAndFee = 25 + taxGst;
                                                                trip.taxandfee = Convert.ToInt32(taxAndFee);

                                                                // Driver fee 
                                                                decimal driverFee = taxBase; // hour + night + return only
                                                                                             //trip.DriverFee = Math.Round(driverFee, 2);
                                                                tripVM.DriverPrice = driverFee.ToString();

                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        var percentage = 80;
                                                        if (percentage > 0)
                                                        {
                                                            //trip.TotalTripValue = Math.Abs(Convert.ToDecimal(trip.EstimatedPrice)).ToString();
                                                            //// Convert EstimatedPrice and totalprice to decimal and add them
                                                            //decimal totalValue = Convert.ToDecimal(trip.EstimatedPrice);

                                                            //var taxvalue = Convert.ToInt32(totalValue) * (taxpercentages / 100);
                                                            //int? anonymuscharges = 0;
                                                            //var anonymusdata = _context.Anonymoustripcharges.Where(c => c.TriptypeId == trip.TripTypeId).FirstOrDefault();
                                                            //if (anonymusdata != null)
                                                            //{
                                                            //    anonymuscharges = anonymusdata.Amount;
                                                            //}
                                                            //// Calculate drivervalue using decimal arithmetic to preserve precision
                                                            //decimal drivervalue = (totalValue - Convert.ToDecimal(taxvalue) - Convert.ToDecimal(careprice)) * percentage / 100;

                                                            //// Convert drivervalue to string for assignment
                                                            //trip.DriversPrice = drivervalue.ToString(CultureInfo.InvariantCulture);

                                                            int baseHourCharge = trip.hoursCharge ?? 0;
                                                            int returnCharge = trip.driverReturnCharges ?? 0;
                                                            var nightCharge = tripVM.NightCharges ?? 0;
                                                            // Taxes & Fees: ₹25 fixed + 5% GST on (hour + night + return)
                                                            decimal taxBase = baseHourCharge + returnCharge + nightCharge;
                                                            decimal taxGst = taxBase * 5 / 100;
                                                            decimal taxAndFee = 25 + taxGst;
                                                            trip.taxandfee = Convert.ToInt32(taxAndFee);

                                                            // Driver fee 
                                                            decimal driverFee = taxBase; // hour + night + return only
                                                                                         //trip.DriverFee = Math.Round(driverFee, 2);
                                                            tripVM.DriverPrice = driverFee.ToString();
                                                        }
                                                    }
                                                }


                                                // Optionally, update StartDateTime if necessary
                                                // tripdata.StartDateTime = tripdata.StartDateTime; // This line seems redundant unless you have a specific reason to reassign StartDateTime to itself


                                            }
                                            else
                                            {
                                                trip.TotalTripValue = trip.EstimatedPrice;
                                                var tripprice = trip.TotalTripValue;
                                                var driversubdata = _context.Driversubscriptions.Where(c => c.DriverId == trip.DriverId && c.Expirydate <= DateTime.Now).FirstOrDefault();
                                                if (driversubdata != null)
                                                {
                                                    var subscriptiondata = _context.Subscriptions.Find(driversubdata.SubscriptionId);
                                                    if (subscriptiondata != null)
                                                    {
                                                        var percentage = subscriptiondata.Percentage;
                                                        if (percentage > 0)
                                                        {
                                                            //int? anonymuscharges = 0;
                                                            //var anonymusdata = _context.Anonymoustripcharges.Where(c => c.TriptypeId == trip.TripTypeId).FirstOrDefault();
                                                            //if (anonymusdata != null)
                                                            //{
                                                            //    anonymuscharges = anonymusdata.Amount;
                                                            //}
                                                            //var taxvalue = Convert.ToInt32(tripprice) * (taxpercentages / 100);

                                                            //// Calculate drivervalue using decimal arithmetic to preserve precision
                                                            //var drivervalue = (Convert.ToInt32(tripprice) - taxvalue - Convert.ToDecimal(careprice) - anonymuscharges) * (percentage / 100);
                                                            //trip.DriversPrice = drivervalue.ToString();

                                                            int baseHourCharge = trip.hoursCharge ?? 0;
                                                            int returnCharge = trip.driverReturnCharges ?? 0;
                                                            var nightCharge = tripVM.NightCharges ?? 0;
                                                            // Taxes & Fees: ₹25 fixed + 5% GST on (hour + night + return)
                                                            decimal taxBase = baseHourCharge + returnCharge + nightCharge;
                                                            decimal taxGst = taxBase * 18 / 100;
                                                            decimal taxAndFee = 25 + taxGst;
                                                            trip.taxandfee = Convert.ToInt32(taxAndFee);

                                                            // Driver fee 
                                                            decimal driverFee = taxBase; // hour + night + return only
                                                                                         //trip.DriverFee = Math.Round(driverFee, 2);
                                                            tripVM.DriverPrice = driverFee.ToString();

                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    var percentage = 80;
                                                    if (percentage > 0)
                                                    {
                                                        //trip.TotalTripValue = Math.Abs(Convert.ToDecimal(trip.EstimatedPrice)).ToString();
                                                        //tripprice = trip.TotalTripValue;
                                                        //// Convert EstimatedPrice and totalprice to decimal and add them
                                                        //decimal totalValue = Convert.ToDecimal(trip.EstimatedPrice);
                                                        //var taxvalue = Convert.ToInt32(totalValue) * (taxpercentages / 100);
                                                        //// Calculate drivervalue using decimal arithmetic to preserve precision
                                                        //decimal drivervalue = (totalValue - Convert.ToDecimal(taxvalue) - Convert.ToDecimal(careprice)) * percentage / 100;

                                                        //// Convert drivervalue to string for assignment
                                                        //trip.DriversPrice = Math.Round(drivervalue, 0).ToString(CultureInfo.InvariantCulture);

                                                        int baseHourCharge = trip.hoursCharge ?? 0;
                                                        int returnCharge = trip.driverReturnCharges ?? 0;
                                                        var nightCharge = tripVM.NightCharges ?? 0;
                                                        // Taxes & Fees: ₹25 fixed + 5% GST on (hour + night + return)
                                                        decimal taxBase = baseHourCharge + returnCharge + nightCharge;
                                                        decimal taxGst = taxBase * 5 / 100;
                                                        decimal taxAndFee = 25 + taxGst;
                                                        trip.taxandfee = Convert.ToInt32(taxAndFee);

                                                        // Driver fee 
                                                        decimal driverFee = taxBase; // hour + night + return only
                                                                                     //trip.DriverFee = Math.Round(driverFee, 2);
                                                        tripVM.DriverPrice = driverFee.ToString();

                                                    }
                                                }
                                            }
                                            tripVM.DriverPrice = trip.DriversPrice.ToString();
                                            tripVMList.Add(tripVM);
                                        }
                                    }
                                }
                            }
                            return Ok(tripVMList);
                        }
                        else
                        {
                            return NoContent();
                        }
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
            var clientId = "757136676860-tkvm5fpjliku503a1geo2f2n631ibi6q.apps.googleuserconte" +
                "3nt.com";
            string authUri = $"https://accounts.google.com/o/oauth2/v2/auth?client_id={clientId}&response_type=code&scope=email%20profile";

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
                    var status = json["status"].ToString();
                    if (status == "REQUEST_DENIED")
                    {
                        return 0;
                    }
                    var responsestatus = json["rows"][0]["elements"][0]["status"].ToString();
                    if (responsestatus == "NOT_FOUND")
                    {
                        return 0;
                    }
                    // Extract the distance from the JSON
                    var distance = json["rows"][0]["elements"][0]["distance"]["value"];
                    int durationInTraffic = (int)json["rows"][0]["elements"][0]["duration_in_traffic"]["value"];
                    return distance;
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    return BadRequest($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            return NoContent();
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


        [HttpPost]
        public async Task<ActionResult<Trip>> Updateimages(int tripid, string images, int flag)
        {
            try
            {
                var tripdata = _context.Trips.Find(tripid);
                if (tripdata != null)
                {
                    if (flag == 1)
                    {
                        tripdata.ImageUrlsList = images;
                    }

                    if (flag == 2)
                    {
                        tripdata.ImageUrlsList = tripdata.ImageUrlsList + ',' + images;
                    }
                    _context.Entry(tripdata).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return Ok(tripdata);
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
