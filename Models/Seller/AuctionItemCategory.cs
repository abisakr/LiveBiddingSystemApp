using System.ComponentModel.DataAnnotations;

namespace Live_Bidding_System_App.Models.Seller
{
    public class AuctionItemCategory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string CategoryName { get; set; }
        public ICollection<AuctionItem> AuctionItems { get; set; }
    }
}
