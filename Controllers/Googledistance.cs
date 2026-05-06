using GoChauffeurWebApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Googledistance : ControllerBase
    {
        private readonly DataContext _context;

        public Googledistance(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<dynamic>> getgoogledistance(string lat1, string lon1, string lat2, string lon2)
        {
            // Replace "YOUR_API_KEY" with your actual Google Maps API key
            //string apiKey = "AIzaSyCKkBWbhsJgwsPBxSC2IHOnnAVdmymFvPs";
            string apiKey = "AIzaSyCKkBWbhsJgwsPBxSC2IHOnnAVdmymFvPs";

            // Replace the coordinates with your actual latitude and longitude values
            string origin = lat1 +','+ lon1; // San Francisco, CA
            string destination = lat2 + ',' + lon2; // Los Angeles, CA
            var clientId = "757136676860-tkvm5fpjliku503a1geo2f2n631ibi6q.apps.googleuserconte" +
                "3nt.com";
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
                    int distance =(int)json["rows"][0]["elements"][0]["distance"]["value"];
                    int durationInTraffic = (int)json["rows"][0]["elements"][0]["duration_in_traffic"]["value"];
                    Console.WriteLine($"Distance between the two points: {distance} meters");
                    
                    return Ok(distance);
                }
                else
                {
                    return BadRequest($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
        }
    }  
}
