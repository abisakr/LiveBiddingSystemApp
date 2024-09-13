namespace Live_Bidding_System_App.Repositories.Seller.DTO
{
    public class CreateAuctionItemDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Photo { get; set; }
        public string UserId { get; set; }
    }
}
