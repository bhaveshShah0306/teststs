using GoChauffeurWebApi.Data;
using GoChauffeurWebApi.Models;
using GoChauffeurWebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardTrips : ControllerBase
    {
        private readonly DataContext _context;

        public DashboardTrips(DataContext context)
        {
            _context = context;
        }

        // GET: api/Cupons
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<TripVM>>> GetTrips(int userId)
        //{
        //    try
        //    {
        //        var tripdata = _context.Trips.Where(c => c.UserId == userId && c.IsTripCompByDriver != true).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}
