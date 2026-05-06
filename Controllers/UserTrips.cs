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
	public class UserTrips : ControllerBase
	{
		private readonly DataContext _context;

		public UserTrips(DataContext context)
		{
			_context = context;
		}


		[HttpGet]
		public async Task<ActionResult<IEnumerable<UserTripsVM>>> GetTrips()
		{
			try
			{
				var tripsList = _context.Trips.ToList();
				var tripsVMList = new List<UserTripsVM>();
				var nightCfg = _context.NightCharges.FirstOrDefault();
				if (tripsList.Count > 0)
				{
					foreach (var trip in tripsList)
					{
						// Apply night charge from DB based on scheduled StartDateTime
						if (trip.StartDateTime.HasValue && nightCfg != null && nightCfg.StartTime.HasValue && nightCfg.EndTime.HasValue)
						{
							var scheduledTime = trip.StartDateTime.Value.TimeOfDay;
							var nightStart = nightCfg.StartTime.Value.TimeOfDay;
							var nightEnd = nightCfg.EndTime.Value.TimeOfDay;
							bool isNight = scheduledTime >= nightStart || scheduledTime <= nightEnd;
							trip.nightCharge = isNight ? Convert.ToInt32(nightCfg.Charges ?? 0) : 0;
						}
						var tripVM = new UserTripsVM();
						tripVM.TripId = trip.TripId;
						tripVM.FromTime = trip.StartDateTime;
						tripVM.PickupLocation = trip.FromLocationName;
						tripVM.DropLocation = trip.ToLocationName;
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
						if(tripVM.Istripcompleted == true)
						{
							tripVM.TotalTripValue = trip.TotalTripValue;
							tripVM.Hours = trip.NoOfHoursActual;
							tripVM.FromTime = trip.StartDateTime;


						}
						else
						{
							tripVM.TotalTripValue = trip.EstimatedPrice;
							tripVM.Hours = trip.NoOfHoursSelected;
							tripVM.ToTime = trip.EndDateTime;


						}
						var pymenttypedata =_context.PaymentTypes.Find(trip.PaymentTypeId);
						if (pymenttypedata != null)
						{
							tripVM.PaymentType = pymenttypedata.PaymentTypeName;
						}
						tripVM.IsCancelled = trip.IsCancelled;
						tripVM.IsProcessing = trip.IsProcessing;
						tripVM.Istripcompleted = trip.IsTripCompByDriver;
						tripsVMList.Add(tripVM);
					}
					return Ok(tripsVMList);
				}
				else
				{
					return NoContent();
				}

			}
			catch(Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("{userId}/flag")]
		public async Task<ActionResult<IEnumerable<UserTripsVM>>> GetTrips(int userId)
		{
			try
			{
				//needed to add today for any discrepancies in conversion
				var today = DateTime.Now.Date;
				var tripsList = _context.Trips.Where(c => c.UserId == userId && c.IsCancelled != true && c.StartDateTime.Value.Date == today).ToList();

				//var tripsList = _context.Trips
				//   .Where(c => c.UserId == userId
				//		 && c.IsCancelled != true
				//		 && c.StartDateTime.Value.Date == today)
				//.ToList();
				var tripslistquery = _context.Trips.Where(c => c.UserId == userId && c.IsCancelled != true && c.StartDateTime.Value.Date == today).ToQueryString();


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
							tripVM.TotalTripValue = (trip.TotalTripValue ?? 0) + (trip.nightCharge ?? 0);
							tripVM.Hours = trip.NoOfHoursActual;
							tripVM.ToTime = trip.ActualEndTime;
						}
						else
						{
							tripVM.TotalTripValue = trip.EstimatedPrice + (trip.nightCharge ?? 0);
							tripVM.Hours = trip.NoOfHoursSelected;
							tripVM.ToTime = trip.EndDateTime;
						}
						
						tripVM.IsCancelled = trip.IsCancelled;
						tripVM.IsProcessing = trip.IsProcessing;
						tripVM.IsAccepted = trip.IsAccepted;
						tripVM.IsReserved = trip.IsReserved;
						tripVM.IsdriverArrived = trip.IsdriverArrived;
						tripVM.IsTripStarted = trip.IsTripStarted;
						tripVM.Istripcompleted = trip.IsTripCompByDriver;

						tripVM.CuponId = trip.CuponId;
						var coupansdata = _context.Cupons.Find(tripVM.CuponId);
						if (coupansdata != null)
						{
							tripVM.CuponCode = coupansdata.CuponCode;
							tripVM.CuponName = coupansdata.CuponName;
							tripVM.Percentage = coupansdata.Percentage;
							tripVM.Price = decimal.TryParse(coupansdata.Price, out decimal couponprice) ? couponprice :0;
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
				var monthlydatalist = await _context.Monthlies.Where(c => c.UserId == userId ).ToListAsync();
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
						monthlyVM.isalltripscompleted =Convert.ToBoolean( monthly.CloseTrip);
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

				var flexiModelList = _context.Flexis.Where(c => c.UserId == userId).ToList();
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
						flexivm.istripcompleted =Convert.ToBoolean( flexiModel.CloseTrip);
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
						var datelists = await _context.FlexiDatesLists.Where(c => c.FlexiId== flexivm.FlexiId).ToListAsync();
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


		[HttpGet("{userId}/{tripstatusid}")]
		public async Task<ActionResult<IEnumerable<UserTripsVM>>> GetTrip(int userId, int tripstatusid)
		{
			try
			{
				var tripsList = _context.Trips.Where(c=>c.UserId == userId&&c.TripStatus == tripstatusid).ToList();

				var tripsVMList = new List<UserTripsVM>();
				var nightCfg = _context.NightCharges.FirstOrDefault();
				if (tripsList.Count > 0)
				{
					foreach (var trip in tripsList)
					{
						if (trip.StartDateTime.HasValue && nightCfg != null
							&& nightCfg.StartTime.HasValue && nightCfg.EndTime.HasValue)
						{
							var scheduledTime = trip.StartDateTime.Value.TimeOfDay;
							var nightStart = nightCfg.StartTime.Value.TimeOfDay;
							var nightEnd = nightCfg.EndTime.Value.TimeOfDay;
							bool isNight = scheduledTime >= nightStart || scheduledTime <= nightEnd;
							trip.nightCharge = isNight ? Convert.ToInt32(nightCfg.Charges ?? 0) : 0;
						}
						var tripVM = new UserTripsVM();
						tripVM.TripId = trip.TripId;
						tripVM.DriverId = trip.DriverId;
						tripVM.FromTime = trip.StartDateTime;
						tripVM.PickupLocation = trip.FromLocationName;
						tripVM.DropLocation = trip.ToLocationName;
						tripVM.Istripcompleted = trip.IsTripCompByDriver;
						tripVM.Distance = trip.ExpectedDistance;
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
							tripVM.TotalTripValue = (trip.TotalTripValue ?? 0) + (trip.nightCharge ?? 0);
							tripVM.Hours = trip.NoOfHoursActual;
							tripVM.ToTime = trip.ActualEndTime;
						}
						else
						{
							tripVM.TotalTripValue = trip.EstimatedPrice + (trip.nightCharge ?? 0);
							tripVM.Hours = trip.NoOfHoursSelected;
							tripVM.ToTime = trip.EndDateTime;
						}
						//if (trip.StartDateTime.HasValue)
						//if (trip.StartDateTime.HasValue)
						//{
						//	if (trip.StartDateTime.Value.Kind == DateTimeKind.Utc)
						//	{
						//		TimeZoneInfo ist = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
						//		trip.StartDateTime =
						//			TimeZoneInfo.ConvertTimeFromUtc(trip.StartDateTime.Value, ist);
						//	}
						//	if (trip.StartDateTime.Value.TimeOfDay >= new TimeSpan(21, 0, 0))
						//	{
						//		trip.nightCharge = 200;
						//	}
						//}
						var pymenttypedata =_context.PaymentTypes.Find(trip.PaymentTypeId);
						if (pymenttypedata != null)
						{
							tripVM.PaymentType = pymenttypedata.PaymentTypeName;
						}

						tripVM.IsCancelled = trip.IsCancelled;
						tripVM.IsProcessing = trip.IsProcessing;
						tripVM.Istripcompleted = trip.IsTripCompByDriver;
						tripsVMList.Add(tripVM);
					}
					return Ok(tripsVMList);
				}
				else
				{
					return NoContent();
				}

			}
			catch(Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}



		[HttpGet("{userId}/{tripstatusid}/onetrip")]
		public async Task<ActionResult<IEnumerable<UserTripsVM>>> GetTriplasttrip(int userId, int tripstatusid)
		{
			try
			{
				var trip = _context.Trips.Where(c => c.UserId == userId && c.TripStatus == tripstatusid).OrderByDescending(c => c.TripId) .FirstOrDefault();


				var tripVM = new UserTripsVM();
				if(trip != null)
				{
					tripVM.TripId = trip.TripId;
					tripVM.DriverId = trip.DriverId;
					tripVM.FromTime = trip.StartDateTime;
					tripVM.PickupLocation = trip.FromLocationName;
					tripVM.DropLocation = trip.ToLocationName;
					tripVM.Distance = trip.ExpectedDistance;
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
					if (trip.StartDateTime.HasValue)
					{
						var nightCfg = _context.NightCharges.FirstOrDefault();
						if (nightCfg != null && nightCfg.StartTime.HasValue && nightCfg.EndTime.HasValue)
						{
							var scheduledTime = trip.StartDateTime.Value.TimeOfDay;
							var nightStart = nightCfg.StartTime.Value.TimeOfDay;
							var nightEnd = nightCfg.EndTime.Value.TimeOfDay;
							bool isNight = scheduledTime >= nightStart || scheduledTime <= nightEnd;
							trip.nightCharge = isNight ? Convert.ToInt32(nightCfg.Charges ?? 0) : 0;
						}
					}
					tripVM.Istripcompleted = trip.IsTripCompByDriver;
					tripVM.Istripcompleted = trip.IsTripCompByDriver;
					if (tripVM.Istripcompleted == true)
					{
						tripVM.TotalTripValue = trip.TotalTripValue + trip.nightCharge;
						tripVM.Hours = trip.NoOfHoursActual;
						tripVM.ToTime = trip.ActualEndTime;


					}
					else
					{
						tripVM.TotalTripValue = trip.EstimatedPrice + trip.nightCharge;
						tripVM.Hours = trip.NoOfHoursSelected;
						tripVM.ToTime = trip.EndDateTime;


					}

					var pymenttypedata =_context.PaymentTypes.Find(trip.PaymentTypeId);
					if (pymenttypedata != null)
					{
						tripVM.PaymentType = pymenttypedata.PaymentTypeName;
					}


					return Ok(tripVM);
				}
				else
				{
					return NoContent();
				}

			}
			catch(Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}



		[HttpGet("{Id}")]
		public async Task<ActionResult<IEnumerable<TripByIdVM>>> GetTrip(int Id)
		{
			try
			{
				var trip = _context.Trips.Find(Id);
				var tripVM = new TripByIdVM();
				if (trip != null)
				{
					tripVM.TripId = trip.TripId;
					tripVM.PickupLocation = trip.FromLocationName;
					tripVM.DropLocation = trip.ToLocationName;
					tripVM.EndTime = trip.EndDateTime;
					tripVM.FromTime = trip.StartDateTime;
					tripVM.Distance= trip.ExpectedDistance;
					tripVM.FromCoordinates = trip.FromLocation;
					tripVM.IsProcessing = trip.IsProcessing;
					tripVM.IsCancelled = trip.IsCancelled;
					tripVM.IsSelfBook = trip.IsSelfBook;
					tripVM.IsdriverArrived = trip.IsdriverArrived ;
					tripVM.IsProcessing = trip.IsProcessing ;
					tripVM.IsReserved = trip.IsReserved ;
					tripVM.IsAccepted = trip.IsAccepted ;
					tripVM.IsCancelled = trip.IsCancelled;
					tripVM.EstimatedPrice = trip.EstimatedPrice;
					tripVM.IsTripStarted = trip.IsTripStarted;
					tripVM.IsAtDrop = trip.IsAtDrop;
					tripVM.PaymentTypeId = trip.PaymentTypeId;
					tripVM.UsersTripsCancelResonsId = trip.UsersTripsCancelResonsId;
					tripVM.UserId = trip.UserId;
					var usertripcancelresondata = _context.UsersTripsCancelResons.Find(tripVM.UsersTripsCancelResonsId);
					if(usertripcancelresondata != null)
					{
						tripVM.UserTripsCancelResonsName = usertripcancelresondata.UserTripsCancelResonsName;
					}
					var triptypedata = _context.TripTypes.Find(trip.TripTypeId);
					if (triptypedata != null)
					{
						tripVM.TripTypeName = triptypedata.TripName;
					}
					var userid = trip.UserId;
					tripVM.Imageurls = trip.ImageUrlsList;
					if (userid != 0 && userid != null)
					{
						var userdata = _context.Users.Find(userid);
						if(userdata!=null)
						{
							if(tripVM.IsSelfBook ==true)
							{
								tripVM.BookedForName = userdata.Name;
								tripVM.BookedForNumber = userdata.PhoneNumber;
							}
							else
							{
								tripVM.BookedForName = trip.BookedForName;
								tripVM.BookedForNumber = trip.BookedForNumber;
							}
						}
					}

					if (trip.NoOfHoursSelected == 1)
					{

						tripVM.ExpecterHours = trip.NoOfHoursSelected+"Hr";
					}
					else
					{
						tripVM.ExpecterHours = trip.NoOfHoursSelected+"Hrs";

					}
					if(trip.NoOfHoursActual == 1)
					{

						tripVM.Hours = trip.NoOfHoursActual+"Hr";
					}
					else
					{
						tripVM.Hours = trip.NoOfHoursActual+"Hrs";

					}

					tripVM.VehicleTypeId = trip.VehicleTypeId;
					var vehicleTypeData = _context.VehicleTypes.Find(tripVM.VehicleTypeId);
					if (vehicleTypeData != null)
					{
						tripVM.VehicleTypeName = vehicleTypeData.VehicleTypeName;
					}
					tripVM.TransamissionTypeId = trip.TransmissionTypeId;
					var transmissonData = _context.TransmissionTypes.Find(tripVM.TransamissionTypeId);
					if(transmissonData != null)
					{
						tripVM.TransmissionTypeName = transmissonData.TransmissionName;
					}
					tripVM.TripTime = trip.StartDateTime;

             

                    tripVM.FromCoordinates = trip.FromLocation;
					tripVM.ToCoordinates = trip.ToLocation;

         
					tripVM.DriverId = trip.DriverId;
					var driverData = _context.Drivers.Find(tripVM.DriverId);
					if(driverData != null)
					{
						tripVM.DriverName = driverData.DriverName;
						tripVM.DriverContactNumber = driverData.PhoneNumber;
						tripVM.Image = driverData.Image;
						if (driverData.IsSubscribed == true)
						{
							if(driverData.ExpiredDate> DateTime.Now)
							{
								driverData.IsSubscribed = false;
								driverData.SubscriptionId = 1;
								_context.Entry(driverData).State = EntityState.Modified;
								await _context.SaveChangesAsync();

								var subscribeddata = _context.Subscriptions.Find(driverData.SubscriptionId);
								if (subscribeddata != null)
								{
									tripVM.SubscriptionName = subscribeddata.SubName;
								}

							}
							else
							{
								var subscribeddata = _context.Subscriptions.Find(driverData.SubscriptionId);
								if (subscribeddata != null)
								{
									tripVM.SubscriptionName = subscribeddata.SubName;
								}
							}
						}
						else
						{
							driverData.IsSubscribed = false;
							driverData.SubscriptionId = 1;
							_context.Entry(driverData).State = EntityState.Modified;
							await _context.SaveChangesAsync();

							var subscribeddata = _context.Subscriptions.Find(driverData.SubscriptionId);
							if (subscribeddata != null)
							{
								tripVM.SubscriptionName = subscribeddata.SubName;
							}

						}
					}
					tripVM.CouponId = trip.CuponId;
					if (tripVM.CouponId != null)
					{
						var coupundata = _context.Cupons.Find(tripVM.CouponId);
						if (coupundata != null)
						{
							tripVM.RideFare = trip.EstimatedPrice;
							var ridevalue = Convert.ToInt32(Math.Round(Convert.ToDecimal(tripVM.RideFare)));

							if (coupundata.Price != null && Convert.ToDecimal(coupundata.Price) != 0)
							{
								tripVM.CoupunPrice = decimal.TryParse(coupundata.Price, out decimal couponprice) ? couponprice :0;
							}
							if(coupundata.Percentage!=0&& coupundata.Percentage != null)
							{
								var coupunprice = ridevalue * coupundata.Percentage / 100;
								tripVM.CoupunPrice = coupunprice;
							}
						}
					}
					tripVM.PaymentTypeId = trip.PaymentTypeId;
					var paymentTypeData = _context.PaymentTypes.Find(tripVM.PaymentTypeId);
					if(paymentTypeData != null)
					{
						tripVM.PaymentTypeName = paymentTypeData.PaymentTypeName;
					}

					tripVM.ExtraTimeUsed = trip.ExtraTimeUsed ?? 0;
					tripVM.ExtraTimeCharge = trip.ExtraTimeCost ?? 0;

					tripVM.ExtraKilometersUsed = trip.ExtraKilometersUsed ?? 0;
					tripVM.ExtraKilometerCost = trip.ExtraKilometerCost ?? 0;

					tripVM.Istripcompleted = trip.IsTripCompByDriver;
					tripVM.RideFare = trip.TotalTripValue;
					if (trip.StartDateTime.HasValue)
					{
						var nightCfg = _context.NightCharges.FirstOrDefault();
						if (nightCfg != null && nightCfg.StartTime.HasValue && nightCfg.EndTime.HasValue)
						{
							var scheduledTime = trip.StartDateTime.Value.TimeOfDay;
							var nightStart = nightCfg.StartTime.Value.TimeOfDay;
							var nightEnd = nightCfg.EndTime.Value.TimeOfDay;
							bool isNight = scheduledTime >= nightStart || scheduledTime <= nightEnd;
							trip.nightCharge = isNight ? Convert.ToInt32(nightCfg.Charges ?? 0) : 0;
						}
					}
					tripVM.NightCharges = trip.nightCharge;
					 

					if (tripVM.Istripcompleted == true)
					{
						tripVM.TaxIds = trip.TaxIds;
						int taxId;
						if (int.TryParse(tripVM.TaxIds, out taxId))
						{
							var TaxIdsdata = _context.Taxes.Find(taxId);
							if (TaxIdsdata != null)
							{
								tripVM.TaxPrice = Convert.ToString(TaxIdsdata.Percentage);

							}
						}
						var taxpriceparsedata = Convert.ToDecimal(tripVM.TaxPrice);
						tripVM.CouponId = trip.CuponId;
						if (tripVM.CouponId != null)
						{
							var coupundata = _context.Cupons.Find(tripVM.CouponId);
							if (coupundata != null)
							{
								var ridevalue = Convert.ToInt32(Math.Round(Convert.ToDecimal(tripVM.RideFare)));
								tripVM.CouponPercentage = coupundata.Percentage;
								var percentagevalue = ridevalue * tripVM.CouponPercentage / 100;
								var taxprice = ridevalue * Convert.ToDecimal(tripVM.TaxPrice) / 100;
								tripVM.TaxPrice = taxprice.ToString();
								var diffridefacrevalue = ridevalue - percentagevalue;
								var parsePrice = Convert.ToString(diffridefacrevalue);
								var coupanprize = Convert.ToDecimal(parsePrice);
								var nightCharges = tripVM.NightCharges;
								decimal extraTimeCharge = trip.ExtraTimeCost ?? 0;
								decimal extraKmCharge = trip.ExtraKilometerCost ?? 0;

								var totalgrandvalue =
									Convert.ToDecimal(tripVM.EstimatedPrice)
									+ Convert.ToDecimal(nightCharges ?? 0)
									+ coupanprize
									+ taxprice
									+ extraTimeCharge
									+ extraKmCharge;

								var roundedTotalGrandValue = Math.Ceiling(totalgrandvalue); // Round off to the next integer value
								tripVM.TotalTripValue = Convert.ToString(roundedTotalGrandValue);
								tripVM.FromTime = trip.StartDateTime;

							}
							else if (coupundata == null )
							{
								var ridevalue = Convert.ToInt32(Math.Round(Convert.ToDecimal(tripVM.RideFare)));
								var taxprice = ridevalue * Convert.ToDecimal(tripVM.TaxPrice) / 100;
								tripVM.TaxPrice = taxprice.ToString();
								var nightCharges = tripVM.NightCharges;

								decimal extraTimeCharge = trip.ExtraTimeCost ?? 0;
								decimal extraKmCharge = trip.ExtraKilometerCost ?? 0;

								var totalgrandvalue =
									Convert.ToDecimal(tripVM.EstimatedPrice)
									+ Convert.ToDecimal(nightCharges ?? 0)
									+ taxprice
									+ extraTimeCharge
									+ extraKmCharge;

								var roundedTotalGrandValue = Math.Ceiling(totalgrandvalue); // Round off to the next integer value
								tripVM.TotalTripValue = Convert.ToString(roundedTotalGrandValue+taxprice);
								tripVM.FromTime = trip.StartDateTime;
							}

						}




					}

					else
					{
						tripVM.RideFare = trip.EstimatedPrice;
						tripVM.TaxIds = trip.TaxIds;
						int taxId;
						if (int.TryParse(tripVM.TaxIds, out taxId))
						{
							var TaxIdsdata = _context.Taxes.Find(taxId);
							if (TaxIdsdata != null)
							{
								tripVM.TaxPrice = Convert.ToString(TaxIdsdata.Percentage);

							}
						}
						var coupundata = _context.Cupons.Find(tripVM.CouponId);
						if (coupundata != null)
						{

							var ridevalue = Convert.ToInt32(Math.Round(Convert.ToDecimal(tripVM.RideFare)));
							tripVM.CouponPercentage = coupundata.Percentage;
							var percentagevalue = ridevalue * tripVM.CouponPercentage / 100;
							var taxprice = ridevalue * Convert.ToDecimal(tripVM.TaxPrice) / 100;
							tripVM.TaxPrice = taxprice.ToString();
							var diffridefacrevalue = ridevalue - percentagevalue;
							var parsePrice = Convert.ToString(diffridefacrevalue);
							var coupanprize = Convert.ToDecimal(parsePrice);
							var totalgrandvalue = Convert.ToInt32(tripVM.EstimatedPrice )+ coupanprize + taxprice;
							var roundedTotalGrandValue = Math.Ceiling(totalgrandvalue); // Round off to the next integer value
							tripVM.TotalTripValue = Convert.ToString(roundedTotalGrandValue);
							tripVM.FromTime = trip.StartDateTime;
						}
						else if (coupundata == null)
						{
							var ridevalue = Convert.ToDecimal(tripVM.EstimatedPrice);
							var taxprice = ridevalue * Convert.ToDecimal(tripVM.TaxPrice) / 100;
							tripVM.TaxPrice = taxprice.ToString();
							var totalgrandvalue = taxprice + ridevalue;
							var roundedTotalGrandValue = Math.Ceiling(totalgrandvalue); // Round off to the next integer value
							tripVM.TotalTripValue = Convert.ToString(roundedTotalGrandValue + taxprice);
							tripVM.FromTime = trip.StartDateTime;
						}

					}

					return Ok(tripVM);

				}
				else
				{
					return NoContent();
				}

			}
			catch(Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}


		[HttpPost("{Id}")]
		public async Task<ActionResult<Trip>> CancelTrip(int Id, int usertripcancelid)
		{
			try
			{
				Log.Information($"API got hit");
				var tripdata = await _context.Trips.FindAsync(Id);
				if (tripdata != null)
				{
					if(tripdata.IsTripCompByDriver==true)
					{
						Log.Information($"API returned: Trip is already completed ");
						return BadRequest("Trip is already completed");
					}
					tripdata.TripStatus = 4;
					tripdata.IsCancelled = true;
					tripdata.IsProcessing= false;
					tripdata.UsersTripsCancelResonsId = usertripcancelid;
					_context.Entry(tripdata).State = EntityState.Modified;

					await _context.SaveChangesAsync();
					Log.Information($"API returned: Cancelled");

					return tripdata;

				}
				else
				{
					Log.Information($"API returned: Badrequest ");

					return BadRequest("No data Found");
				}
			}
			catch(Exception ex)
			{
				Log.Information($"API returned error{ex.Message}");

				return BadRequest(ex.Message);

			}
		}



		[HttpPost("{Id}/felxi")]
		public async Task<ActionResult<Flexi>> CancelfelxiTrip(int Id, int usertripcancelid)
		{
			try
			{
				Log.Information($"API got hit");
				var flexitripdata = await _context.Flexis.FindAsync(Id);
				if (flexitripdata != null)
				{
					if (flexitripdata.IsTripCompByDriver == true)
					{
						Log.Information($"API returned: Trip is already completed ");
						return BadRequest("Trip is already completed");
					}

					flexitripdata.IsCancelled = true;
					flexitripdata.IsProcessing = false;
					flexitripdata.UsersTripsCancelResonsId = usertripcancelid;
					_context.Entry(flexitripdata).State = EntityState.Modified;

					await _context.SaveChangesAsync();
					Log.Information($"API returned: Cancelled");

					return flexitripdata;

				}
				else
				{
					Log.Information($"API returned: Badrequest ");

					return BadRequest("No data Found");
				}
			}
			catch (Exception ex)
			{
				Log.Information($"API returned error{ex.Message}");

				return BadRequest(ex.Message);

			}
		}



		[HttpPost("{Id}/monthly")]
		public async Task<ActionResult<Monthly>> CancelmothlyTrip(int Id, int usertripcancelid)
		{
			try
			{
				Log.Information($"API got hit");
				var monthlytripdata = await _context.Monthlies.FindAsync(Id);
				if (monthlytripdata != null)
				{
					if (monthlytripdata.IsTripCompByDriver == true)
					{
						Log.Information($"API returned: Trip is already completed ");
						return BadRequest("Trip is already completed");
					}

					monthlytripdata.IsCancelled = true;
					monthlytripdata.IsProcessing = false;
					monthlytripdata.UsersTripsCancelResonsId = usertripcancelid;
					_context.Entry(monthlytripdata).State = EntityState.Modified;

					await _context.SaveChangesAsync();
					Log.Information($"API returned: Cancelled");

					return monthlytripdata;

				}
				else
				{
					Log.Information($"API returned: Badrequest ");

					return BadRequest("No data Found");
				}
			}
			catch (Exception ex)
			{
				Log.Information($"API returned error{ex.Message}");

				return BadRequest(ex.Message);

			}
		}
	}
}
