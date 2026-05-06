using GoChauffeurWebApi.Data;
using GoChauffeurWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateDriverLocation : ControllerBase
    {
        private readonly DataContext _context;

        public UpdateDriverLocation(DataContext context)
        {
            _context = context;
        }



        [HttpPost]
        public async Task<ActionResult<Driver>> UpdateLocation(int id ,string latitude ,string longitude)
        {
            try
            {
                var driverData = await _context.Drivers.FindAsync(id);
                if (driverData != null)
                {
                    driverData.Latitude = latitude;
                    driverData.Longitude= longitude;
                    _context.Entry(driverData).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return Ok(driverData);
                }
                else {
                    return NoContent();
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
