using Live_Bidding_System_App.Models.Seller;
using System.ComponentModel.DataAnnotations;

namespace Live_Bidding_System_App.Models.Chats
{

    public class ChatRoom
    {
        [Key]
        public int Id { get; set; }
        public int AuctionId { get; set; }
        public Auction Auction { get; set; }

        // Collection of messages in this chat room
        public ICollection<ChatMessage> Messages { get; set; }

        // Users who are part of this chat room (bidders)
        public ICollection<ApplicationUser> Participants { get; set; }
    }


}
