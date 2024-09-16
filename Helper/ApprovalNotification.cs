using Live_Bidding_System_App.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Live_Bidding_System_App.Helper
{
    public class ApprovalNotification
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public ApprovalNotification(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task OnAuctionItemCreated(string name)
        {
            // Notify admins about the new item
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", $"New auction item created: {name}");

        }
    }
}
