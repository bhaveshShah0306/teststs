using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class RideController : ControllerBase
{
    private readonly IHubContext<RideHub> _rideHubContext;

    public RideController(IHubContext<RideHub> rideHubContext)
    {
        _rideHubContext = rideHubContext;
    }

    [HttpPost("sendRequest")]
    public async Task<IActionResult> SendRideRequest(string userId, string pickupLocation)
    {
        // Send ride request to SignalR hub
        await _rideHubContext.Clients.All.SendAsync("ReceiveRideRequest", userId, pickupLocation);
        return Ok();
    }
}
