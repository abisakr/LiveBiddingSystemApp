using Live_Bidding_System_App.DataContext;
using Live_Bidding_System_App.Helper;
using Live_Bidding_System_App.Models.Seller;

namespace Live_Bidding_System_App.Repositories.Admin
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ApprovalNotification _approvalNotification;
        private readonly CreateBid _createBid;

        public AdminRepository(ApplicationDbContext dbContext, ApprovalNotification approvalNotification, CreateBid createBid)
        {
            _dbContext = dbContext;
            _approvalNotification = approvalNotification;
            _createBid = createBid;
        }

        public async Task<OperationResult<string>> ItemApproval(int itemId, AuctionItemStatus status)
        {
            try
            {
                var auctionItem = await _dbContext.AuctionItemsTbl.FindAsync(itemId);
                if (auctionItem == null)
                    return OperationResult<string>.NotFoundResult();
                auctionItem.Status = status;
                _dbContext.AuctionItemsTbl.Update(auctionItem);
                var result = await _dbContext.SaveChangesAsync();

                if (result > 0)
                {
                    if (status == AuctionItemStatus.Approved)
                    {
                        await _createBid.CreateAuctionBid(auctionItem, status);
                    }

                    await _approvalNotification.OnAuctionItemApprovalRejection(auctionItem.Name, auctionItem.UserId, status);
                    string message = status == AuctionItemStatus.Approved ? "Auction item approved and bid created" : "Auction item rejected";
                    return OperationResult<string>.SuccessResult(message);
                }

                return OperationResult<string>.FailureResult("Failed to update auction item status.");
            }
            catch (Exception ex)
            {
                return OperationResult<string>.FailureResult($"An error occurred: {ex.Message}");
            }
        }

    }
}
