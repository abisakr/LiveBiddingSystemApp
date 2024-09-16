using Microsoft.AspNetCore.SignalR;

namespace Live_Bidding_System_App.Hubs
{
    public class NotificationHub : Hub
    {
        // Method to send notifications
        public async Task SendNotification(string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", message);
        }
    }
}
