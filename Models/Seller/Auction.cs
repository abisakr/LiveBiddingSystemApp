using Live_Bidding_System_App.Models.Buyer;
using Live_Bidding_System_App.Models.Chats;
using System.ComponentModel.DataAnnotations;

namespace Live_Bidding_System_App.Models.Seller
{
    public class Auction
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsClosed { get; set; }

        public int AuctionItemId { get; set; }
        public AuctionItem AuctionItem { get; set; }

        // Navigation properties
        public ICollection<Bid> Bids { get; set; }
        public ChatRoom ChatRoom { get; set; }

        public ICollection<PaymentHistory> PaymentHistories { get; set; }

        // Optional: Property to track the highest bid for better performance
        //  public decimal HighestBidAmount { get; set; }
    }


}
