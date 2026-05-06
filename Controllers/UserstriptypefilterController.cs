using GoChauffeurWebApi.Data;
using GoChauffeurWebApi.Models;
using GoChauffeurWebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserstriptypefilterController : ControllerBase
    {
        private readonly DataContext _context;

        public UserstriptypefilterController(DataContext context)
        {
            _context = context;
        }


        [HttpGet("{triptypeId}")]
        public async Task<ActionResult<IEnumerable<UserTripsVM>>> GetTrips(int triptypeId)
        {
            try
            {
                var tripsList = _context.Trips.Where(c=>c.TripTypeId==triptypeId).ToList();

                var tripsVMList = new List<UserTripsVM>();
                if (tripsList.Count > 0)
                {
                    foreach (var trip in tripsList)
                    {
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
                        if (tripVM.Istripcompleted == true)
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
                        var pymenttypedata = _context.PaymentTypes.Find(trip.PaymentTypeId);
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{triptypeId}/{status}")]
        public async Task<ActionResult<IEnumerable<UserTripsVM>>> GetTripbystatus(int triptypeId , int status)
        {
            try
            {
                var tripsList = _context.Trips.Where(c => c.TripTypeId == triptypeId&& c.TripStatus == status).ToList();

                var tripsVMList = new List<UserTripsVM>();
                if (tripsList.Count > 0)
                {
                    foreach (var trip in tripsList)
                    {
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
                        if (tripVM.Istripcompleted == true)
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
                        var pymenttypedata = _context.PaymentTypes.Find(trip.PaymentTypeId);
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
