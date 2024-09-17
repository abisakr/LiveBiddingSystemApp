using Live_Bidding_System_App.Hubs;
using Live_Bidding_System_App.Models.Seller;
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

        public async Task OnAuctionItemApprovalRejection(string name, string userId, AuctionItemStatus status)
        {
            // Notify User about the  item rejection
            if (status == AuctionItemStatus.Rejected)
                await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", $"Your auction item {name} is Rejected.");

            // Notify User about the  item approval
            else if (status == AuctionItemStatus.Approved) await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", $"Your auction item {name} is Approved and Bid is Created.");

        }
    }
}
