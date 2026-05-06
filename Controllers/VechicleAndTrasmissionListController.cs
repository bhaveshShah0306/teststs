using GoChauffeurWebApi.Data;
using GoChauffeurWebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VechicleAndTrasmissionListController : ControllerBase
    {
        private readonly DataContext _context;
        public VechicleAndTrasmissionListController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleTypeListVM>>> GetVTlist()
        {
            try
            {
                var transmissionTypes = _context.TransmissionTypes.ToList();
                var vehicleTypes = _context.VehicleTypes.ToList();

                var vehicleTypeVMList = new List<VehicleTypeListVM>();

                if (transmissionTypes.Count > 0)
                {
                    // List transmissions by VehicleTypeId
                    var ListData = transmissionTypes.GroupBy(t => t.VehicleTypeId);

                    foreach (var group in ListData)
                    {
                        var vehicleTypeId = group.Key;
                        var vehicleData = vehicleTypes.FirstOrDefault(v => v.VehicleTypeId == vehicleTypeId);
                        if (vehicleData != null)
                        {
                            var vehicleTypeVM = new VehicleTypeListVM
                            {
                                VehicleTypeId = vehicleData.VehicleTypeId,
                                VehicleTypeName = vehicleData.VehicleTypeName,
                                VehicleIcon = vehicleData.Icon,
                                Transmissions = new List<TransmissionTypelistVM>()
                            };

                            foreach (var transmission in group)
                            {
                                var transmissionVM = new TransmissionTypelistVM
                                {
                                    TransmissionTypeId = transmission.TransmissionTypeId,
                                    TransmissionName = transmission.TransmissionName,
                                    TransmissionIcon = transmission.icon
                                };
                                vehicleTypeVM.Transmissions.Add(transmissionVM);
                            }

                            vehicleTypeVMList.Add(vehicleTypeVM);
                        }
                    }
                    return Ok(vehicleTypeVMList);
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
