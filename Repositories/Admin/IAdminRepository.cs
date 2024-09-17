using Live_Bidding_System_App.Helper;
using Live_Bidding_System_App.Models.Seller;

namespace Live_Bidding_System_App.Repositories.Admin
{
    public interface IAdminRepository
    {
        public Task<OperationResult<string>> ItemApproval(int itemId, AuctionItemStatus status);
    }
}
