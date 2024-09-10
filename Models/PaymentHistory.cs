using Live_Bidding_System_App.Models.Seller;
using System.ComponentModel.DataAnnotations;

namespace Live_Bidding_System_App.Models
{
    public class PaymentHistory
    {
        [Key]
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }

        public int AuctionId { get; set; }
        public Auction Auction { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public PaymentStatus Status { get; set; }  // Payment status (e.g., Completed, Pending)
    }

    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed
    }


}
