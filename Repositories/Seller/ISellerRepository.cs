using Live_Bidding_System_App.Repositories.Seller.DTO;

namespace Live_Bidding_System_App.Repositories.Seller
{
    public interface ISellerRepository
    {
        public Task<string> CreateAuctionItem(CreateAuctionItemDto createAuctionItemDto);
        public Task<string> EditAuctionItem(int itemId, CreateAuctionItemDto createAuctionItemDto);
        public Task<string> DeleteAuctionItem(int itemId);
        public Task<ViewAuctionItemDto> GetAuctionItemById(int itemId);
        public Task<IEnumerable<ViewAuctionItemDto>> GetAllAuctionItems();
    }
}
