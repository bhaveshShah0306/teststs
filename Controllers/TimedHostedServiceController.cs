using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using GoChauffeurWebApi.Service;
using GoChauffeurWebApi.Data;
using GoChauffeurWebApi.Models;
using Microsoft.EntityFrameworkCore;
using Nest;
using Serilog;
using static System.Net.WebRequestMethods;

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimedHostedServiceController : ControllerBase
    {
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly DataContext _context;

        public TimedHostedServiceController(IBackgroundTaskQueue taskQueue, IHttpClientFactory httpClientFactory, DataContext context)
        {
            _taskQueue = taskQueue;
            _httpClientFactory = httpClientFactory;
            _context = context;
        }

        [HttpPost("start/{tripId}")]
        public async Task<ActionResult<Trip>> StartJob(int tripId, int min)
        {
            var tripdata = await _context.Trips.FindAsync(tripId);
            Log.Information($"api initiated ");
            if (tripdata == null)
            {
                return BadRequest("Data Not Found");
            }

            tripdata.IsProcessing = true;
            _context.Entry(tripdata).State = EntityState.Modified;
            _context.SaveChanges();
            Log.Information($"api updated to processing ");

            _taskQueue.QueueBackgroundWorkItem(async token =>
            {
                var jsonContent = "{}";
                var content = new StringContent(jsonContent);
                var httpClient = _httpClientFactory.CreateClient();
                // Your background task logic here
                await Task.Delay(TimeSpan.FromMinutes(min), token);
                //add log
                // Making the HTTP GET call
                var response = await httpClient.PostAsync($"https://api.gochauffeurs.in/api/UserTrips/{tripId}", content, token);
                Log.Information($"API hit for channel: https://api.gochauffeurs.in/api/UserTrips/{tripId}");

                response.EnsureSuccessStatusCode();
                // add log
                var responseBody = await response.Content.ReadAsStringAsync();
                Log.Information($"API hit for channel: {responseBody}");
            });

            return Accepted("Job started successfully.");
        }
    }


}
