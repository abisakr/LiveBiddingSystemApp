using Live_Bidding_System_App.Helper;
using Live_Bidding_System_App.Repositories.Seller.DTO;

namespace Live_Bidding_System_App.Repositories.Seller
{
    public interface ISellerRepository
    {
        public Task<OperationResult<string>> CreateAuctionItem(CreateAuctionItemDto createAuctionItemDto);
        public Task<OperationResult<string>> EditAuctionItem(int itemId, EditAuctionItemDto editAuctionItemDto);
        public Task<OperationResult<string>> DeleteAuctionItem(int itemId);
        public Task<OperationResult<IEnumerable<ViewAuctionItemDto>>> GetAllAuctionItems();
        public Task<OperationResult<ViewAuctionItemDto>> GetAuctionItemById(int itemId);

    }
}
