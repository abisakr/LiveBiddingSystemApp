namespace Live_Bidding_System_App.Repositories.Seller.DTO
{
    public class EditAuctionItemDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
