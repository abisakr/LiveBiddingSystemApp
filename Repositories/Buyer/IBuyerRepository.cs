using Live_Bidding_System_App.Helper;

namespace Live_Bidding_System_App.Repositories.Buyer
{
    public interface IBuyerRepository
    {
        public Task<OperationResult<string>> PlaceBid(decimal amount, int auctionId, string userId);
        //public Task<OperationResult<string>> EditAuctionItem(int itemId, EditAuctionItemDto editAuctionItemDto);
        //public Task<OperationResult<string>> DeleteAuctionItem(int itemId);
        //public Task<OperationResult<IEnumerable<ViewAuctionItemDto>>> GetAllAuctionItems(AuctionItemStatus? status);
        //public Task<OperationResult<ViewAuctionItemDto>> GetAuctionItemById(int itemId);
    }
}
