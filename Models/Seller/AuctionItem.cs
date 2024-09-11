using System.ComponentModel.DataAnnotations;

namespace Live_Bidding_System_App.Models.Seller
{
    public class AuctionItem
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Photo { get; set; }  // Item image stored as byte array
        public string Description { get; set; }
        public AuctionItemStatus Status { get; set; } = AuctionItemStatus.Pending;
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<Auction> Auctions { get; set; }
    }

    public enum AuctionItemStatus
    {
        Pending,
        Approved,
        Rejected
    }
}
