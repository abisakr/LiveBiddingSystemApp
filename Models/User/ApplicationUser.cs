using Live_Bidding_System_App.Models.Buyer;
using Live_Bidding_System_App.Models.Chats;
using Microsoft.AspNetCore.Identity;

namespace Live_Bidding_System_App.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Address { get; set; }

        public ICollection<Bid> Bids { get; set; }
        public ICollection<ChatRoom> ChatRooms { get; set; }
        public ICollection<PaymentHistory> PaymentHistories { get; set; }
    }

}
