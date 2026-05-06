using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class RideHub : Hub
{
    public async Task SendRideRequest(string userId, string pickupLocation)
    {
        // Broadcast the ride request to all connected drivers
        await Clients.All.SendAsync("ReceiveRideRequest", userId, pickupLocation);
    }
}