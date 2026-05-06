using GoChauffeurWebApi.Data;
using GoChauffeurWebApi.Models;
using GoChauffeurWebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using Serilog;
using System.Data.SqlTypes;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class usercompletedtrips : ControllerBase
    {
        private readonly DataContext _context;

        public usercompletedtrips(DataContext context)
        {
            _context = context;
        }



        [HttpGet("{userId}/flag")]
        public async Task<ActionResult<IEnumerable<UserTripsVM>>> GetTrips(int userId)
        {
            try
            {
                var tripsList = _context.Trips.Where(c => c.UserId == userId && c.IsCancelled != true && c.IsTripCompByDriver==true && c.StartDateTime >= DateTime.Now).ToList();
                var tripsVMList = new List<UserTripsVM>();

                if (tripsList.Count > 0)
                {
                    foreach (var trip in tripsList)
                    {
                        var tripVM = new UserTripsVM
                        {
                            TripId = trip.TripId,
                            DriverId = trip.DriverId,
                            FromTime = trip.StartDateTime,
                            PickupLocation = trip.FromLocationName,
                            DropLocation = trip.ToLocationName,
                            Distance = trip.ExpectedDistance,
                            Drivermeansoftransport = trip.DriverMeansOfTransport,
                            IsAccepted = trip.IsAccepted
                        };

                        var driverdata = _context.Drivers.Find(tripVM.DriverId);
                        if (driverdata != null && tripVM.IsAccepted == true)
                        {
                            tripVM.DriverName = driverdata.DriverName;
                            tripVM.Image = driverdata.Image;
                            tripVM.PhoneNumber = driverdata.PhoneNumber;
                        }

                        tripVM.IsOnroute = !string.IsNullOrEmpty(tripVM.Drivermeansoftransport);

                        var triptypedata = _context.TripTypes.Find(trip.TripTypeId);
                        if (triptypedata != null)
                        {
                            tripVM.TripTypeName = triptypedata.TripName;
                        }

                        var vehicletypedata = _context.VehicleTypes.Find(trip.VehicleTypeId);
                        if (vehicletypedata != null)
                        {
                            tripVM.VehicleTypeName = vehicletypedata.VehicleTypeName;
                        }

                        var transmissiontypedata = _context.TransmissionTypes.Find(trip.TransmissionTypeId);
                        if (transmissiontypedata != null)
                        {
                            tripVM.TransmissionTypeName = transmissiontypedata.TransmissionName;
                        }

                        tripVM.Istripcompleted = trip.IsTripCompByDriver;
                        if (tripVM.Istripcompleted == true)
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
                        tripVM.IsCancelled = trip.IsCancelled;
                        tripVM.IsProcessing = trip.IsProcessing;
                        tripVM.IsAccepted = trip.IsAccepted;
                        tripVM.IsReserved = trip.IsReserved;
                        tripVM.IsdriverArrived = trip.IsdriverArrived;
                        tripVM.IsTripStarted = trip.IsTripStarted;

                        tripVM.CuponId = trip.CuponId;
                        var coupansdata = _context.Cupons.Find(tripVM.CuponId);
                        if (coupansdata != null)
                        {
                            tripVM.CuponCode = coupansdata.CuponCode;
                            tripVM.CuponName = coupansdata.CuponName;
                            tripVM.Percentage = coupansdata.Percentage;
                            tripVM.Price = decimal.TryParse(coupansdata.Price, out decimal couponprice) ? couponprice : 0;
                            tripVM.Description = coupansdata.Description;
                        }
                        tripVM.TaxIds = trip.TaxIds;

                        // Convert the TaxIds to int
                        int taxId;
                        if (int.TryParse(tripVM.TaxIds, out taxId))
                        {
                            var TaxIdsdata = _context.Taxes.Find(taxId);
                            if (TaxIdsdata != null)
                            {
                                tripVM.TaxPrice = Convert.ToString(TaxIdsdata.Percentage);
                            }
                        }


                        var paymenttypedata = _context.PaymentTypes.Find(trip.PaymentTypeId);
                        if (paymenttypedata != null)
                        {
                            tripVM.PaymentType = paymenttypedata.PaymentTypeName;
                        }

                        tripsVMList.Add(tripVM);
                    }
                }
                var monthlydatalist = await _context.Monthlies.Where(c => c.UserId == userId && c.CloseTrip == true).ToListAsync();
                var monthlyVMList = new List<MonthlyVM>();

                if (monthlydatalist.Count > 0)
                {
                    foreach (var monthly in monthlydatalist)
                    {
                        var monthlyVM = new MonthlyVM
                        {
                            MonthlyId = monthly.MonthlyId,
                            UniqueMonthlyId = monthly.UniqueMonthlyId,
                            TripVarientId = monthly.TripVarientId
                        };
                        monthlyVM.IsdriverArrived = monthly.IsdriverArrived;
                        monthlyVM.istripcompleted = monthly.IsTripCompByDriver;
                        monthlyVM.IsTripStarted = monthly.IsTripStarted;
                        monthlyVM.IsProcessing = monthly.IsProcessing;
                        monthlyVM.IsCancelled = monthly.IsCancelled;
                        monthlyVM.DriverId = monthly.DriverId;

                        monthlyVM.IsAccepted = monthly.IsAccepted;
                        var driverdata = _context.Drivers.Find(monthlyVM.DriverId);
                        if (driverdata != null && monthlyVM.IsAccepted == true)
                        {
                            monthlyVM.DriverName = driverdata.DriverName;
                            monthlyVM.Image = driverdata.Image;
                            monthlyVM.driverPhoneNumber = driverdata.PhoneNumber;
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
                                var dateVM = new MonthlyDateListVM
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
                                    IsTripStarted = datelist.IsTripStarted
                                };

                                datelistVM.Add(dateVM);
                            }
                        }

                        monthlyVM.DateList = datelistVM;
                        monthlyVMList.Add(monthlyVM);
                    }
                }

                var flexiModelList = _context.Flexis.Where(c => c.UserId == userId && c.CloseTrip == true).ToList();
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
                            VehicleTypeId = flexiModel.VehicleTypeId
                        };

                        flexivm.UsersTripsCancelResonsId = flexiModel.UsersTripsCancelResonsId;
                        var datausertripcancelresondata = _context.UsersTripsCancelResons.Find(flexivm.UsersTripsCancelResonsId);
                        if (datausertripcancelresondata != null)
                        {
                            flexivm.UserTripsCancelResonsName = datausertripcancelresondata.UserTripsCancelResonsName;
                        }
                        flexivm.IsdriverArrived = flexiModel.IsdriverArrived;
                        flexivm.istripcompleted = flexiModel.IsTripCompByDriver;
                        flexivm.IsTripStarted = flexiModel.IsTripStarted;
                        flexivm.IsProcessing = flexiModel.IsProcessing;
                        flexivm.IsReserved = flexiModel.IsReserved;
                        flexivm.IsCancelled = flexiModel.IsCancelled;
                        flexivm.PickUPMapURL = flexiModel.PickUPMapURL;
                        flexivm.DriverId = flexiModel.DriverId;

                        flexivm.IsAccepted = flexiModel.IsAccepted;
                        var driverdata = _context.Drivers.Find(flexivm.DriverId);
                        if (driverdata != null && flexivm.IsAccepted == true)
                        {
                            flexivm.DriverName = driverdata.DriverName;
                            flexivm.Image = driverdata.Image;
                            flexivm.driverPhoneNumber = driverdata.PhoneNumber;
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
                        flexivm.EstimatedPrice = flexiModel.EstimatedPrice;
                        flexivm.EstimatedTaxValue = flexiModel.EstimatedTaxValue;
                        flexivm.EstimatedCuponPrice = flexiModel.EstimatedCuponPrice;
                        flexivm.DriverId = flexiModel.DriverId;
                        flexivm.IsAdvancePaid = flexiModel.IsAdvancePaid;
                        flexivm.AdvancePaid = flexiModel.AdvancePaid;
                        flexivm.NoofDays = flexiModel.NoofDays;
                        flexivm.FlexiSelectedDateTime = flexiModel.FlexiSelectedDateTime;
                        var datelistVM = new List<FlexiDateListVM>();
                        var datelists = await _context.FlexiDatesLists.Where(c => c.FlexiId == flexivm.FlexiId).ToListAsync();
                        if (datelists.Count > 0)
                        {
                            foreach (var datelist in datelists)
                            {
                                var dateVM = new FlexiDateListVM
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

                                datelistVM.Add(dateVM);
                            }
                        }

                        flexivm.DateList = datelistVM;

                        flexiVmList.Add(flexivm);
                    }
                }

                return Ok(new { Trip = tripsVMList, Flexi = flexiVmList, Monthly = monthlyVMList });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
