using GoChauffeurWebApi.Data;
using GoChauffeurWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverNearBy : ControllerBase
    {
        private readonly DataContext _context;

        public DriverNearBy(DataContext context)
        {
            _context = context;
        }


        public class Driverlocations
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<dynamic>>> drivernearby(int userid)
        {
            try
            {
                var userdata = _context.Users.Find(userid);
                if (userdata == null)
                {
                    return NoContent();
                }
                if (userdata.Latitude == null && userdata.Latitude == string.Empty && userdata.Logitude == null && userdata.Logitude == string.Empty)
                {
                    return NoContent();
                }
                var drivers = await _context.Drivers.ToListAsync();
                var driverlocationlist = new List<Driverlocations>();
                if (drivers.Count > 0)
                {
                    foreach (var driver in drivers)
                    {
                        var driverlocation = new Driverlocations();
                        if (driver.Latitude != null && driver.Latitude != string.Empty && driver.Longitude != null && driver.Longitude != string.Empty)
                        {

                            var dlat = driver.Latitude;
                            var dlong = driver.Longitude;
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
                            var distance = CalculateDistance(Convert.ToDouble(dlat), Convert.ToDouble(dlong), Convert.ToDouble(userdata.Latitude), Convert.ToDouble(userdata.Logitude));
                            if (distance < distancetocover)
                            {
                                driverlocation.Latitude = Convert.ToDouble(dlat);
                                driverlocation.Longitude = Convert.ToDouble(dlong);
                                driverlocationlist.Add(driverlocation);
                            }
                        }

                    }
                    return Ok(driverlocationlist);
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


        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371e3; // Radius of the earth in meters
            var φ1 = lat1 * Math.PI / 180; // Convert degrees to radians
            var φ2 = lat2 * Math.PI / 180;
            var Δφ = (lat2 - lat1) * Math.PI / 180;
            var Δλ = (lon2 - lon1) * Math.PI / 180;

            var a = Math.Sin(Δφ / 2) * Math.Sin(Δφ / 2) +
                    Math.Cos(φ1) * Math.Cos(φ2) *
                    Math.Sin(Δλ / 2) * Math.Sin(Δλ / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            var d = (R * c) / 1000; // Distance in meters

            return d;
        }
    }
}
