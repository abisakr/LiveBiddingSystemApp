using Live_Bidding_System_App.Helper;
using Live_Bidding_System_App.Models.Seller;
using Live_Bidding_System_App.Repositories.Seller.DTO;

namespace Live_Bidding_System_App.Repositories.Seller
{
    public interface ISellerRepository
    {
        Delegates.NotifyAdminDelegate NotifyAdmin { get; set; }
        public Task<OperationResult<string>> CreateAuctionItem(CreateAuctionItemDto createAuctionItemDto, string userId);
        public Task<OperationResult<string>> EditAuctionItem(int itemId, EditAuctionItemDto editAuctionItemDto);
        public Task<OperationResult<string>> DeleteAuctionItem(int itemId);
        public Task<OperationResult<IEnumerable<ViewAuctionItemDto>>> GetAllAuctionItems(AuctionItemStatus? status);
        public Task<OperationResult<ViewAuctionItemDto>> GetAuctionItemById(int itemId);

    }
}
