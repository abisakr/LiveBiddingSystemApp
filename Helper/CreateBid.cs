using Live_Bidding_System_App.DataContext;
using Live_Bidding_System_App.Models.Chats;
using Live_Bidding_System_App.Models.Seller;
using Microsoft.EntityFrameworkCore;

namespace Live_Bidding_System_App.Helper
{
    public class CreateBid
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ApprovalNotification _approvalNotification;
        public CreateBid(ApplicationDbContext dbContext, ApprovalNotification approvalNotification)
        {
            _dbContext = dbContext;
            _approvalNotification = approvalNotification;
        }
        public async Task<OperationResult<string>> CreateAuctionBid(AuctionItem createAuction, AuctionItemStatus status)
        {
            try
            {


                var auction = new Auction
                {
                    Title = createAuction.Name,
                    Description = createAuction.Description,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddDays(1),
                    IsClosed = false,
                    AuctionItemId = createAuction.Id

                };

                await _dbContext.AuctionsTbl.AddAsync(auction);
                var result = await _dbContext.SaveChangesAsync();
                var auctionItem = _dbContext.AuctionItemsTbl.FirstOrDefault(a => a.Id == createAuction.Id);
                if (auctionItem == null)
                    return OperationResult<string>.FailureResult("Auction item not found");
                if (result > 0)
                {
                    //logic for chatroom also created for that bid to group chat

                    await CreateAuctionChatroom(createAuction.Id);
                    await _approvalNotification.OnAuctionItemApprovalRejection(createAuction.Name, auctionItem.UserId, status);
                    return OperationResult<string>.SuccessResult("Item Approved and Auction Created Successfully");
                }
                return OperationResult<string>.FailureResult("Failed to Create auction");

            }
            catch (Exception ex)
            {
                return OperationResult<string>.FailureResult($"An error occurred: {ex.Message}");
            }
        }

        private async Task CreateAuctionChatroom(int auctionId)
        {
            try
            {
                var chatRoom = new ChatRoom
                {
                    AuctionId = auctionId,
                };

                await _dbContext.ChatRoomsTbl.AddAsync(chatRoom);
                await _dbContext.SaveChangesAsync();



            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while creating the chat room: {ex.Message}", ex);
            }
        }
        public async Task CloseExpiredAuctions()
        {
            var expiredAuctions = await _dbContext.AuctionsTbl
                .Where(a => a.EndDate <= DateTime.UtcNow && !a.IsClosed)
                .ToListAsync();

            foreach (var auction in expiredAuctions)
            {
                auction.IsClosed = true;
            }

            await _dbContext.SaveChangesAsync();
            //send the auction timeout to the participants and send winning notification to winner logic here
        }
    }
}
