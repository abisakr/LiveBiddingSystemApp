using Live_Bidding_System_App.Helper;
using Live_Bidding_System_App.Repositories.Buyer.DTO;

namespace Live_Bidding_System_App.Repositories.Buyer
{
    public interface IBuyerRepository
    {
        public Task<OperationResult<string>> PlaceBid(decimal amount, int auctionId, string userId);
        public Task<OperationResult<IEnumerable<ViewBidsDto>>> GetMyBidsByUserId(string userId);
        public Task<OperationResult<IEnumerable<ViewAuctionsDto>>> GetAllAuctions();
        public Task<OperationResult<IEnumerable<ViewAuctionsDto>>> GetAuctionsByUserId(string userId);
        public Task<OperationResult<IEnumerable<ViewAuctionsDto>>> GetAuctionsByCategory(string category);
        //public Task<OperationResult<string>> DeleteAuctionItem(int itemId);
        //public Task<OperationResult<ViewAuctionItemDto>> GetAuctionItemById(int itemId);
    }
}

