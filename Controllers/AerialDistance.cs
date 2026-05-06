using GoChauffeurWebApi.Data;
using GunturPickles_Grocery_WebAPI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AerialDistance : ControllerBase
    {

        private readonly DataContext _context;

        public AerialDistance(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public dynamic distance(double lat1, double lon1, double lat2, double lon2, char unit)
        {
            // Replace these coordinates with your actual latitude and longitude values
            var origin = new GeoCoordinate(17.4062, 78.3763); // San Francisco, CA
            var destination = new GeoCoordinate(17.3271, 78.6053); // Los Angeles, CA
            //  var origin = new GeoCoordinate(lat1, lon1); // San Francisco, CA
            //var destination = new GeoCoordinate(lat2, lon2); // Los Angeles, CA

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
}
