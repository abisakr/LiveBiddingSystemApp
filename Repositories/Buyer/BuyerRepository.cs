using Live_Bidding_System_App.DataContext;
using Live_Bidding_System_App.Helper;
using Live_Bidding_System_App.Models;
using Live_Bidding_System_App.Models.Buyer;
using Microsoft.EntityFrameworkCore;

namespace Live_Bidding_System_App.Repositories.Buyer
{
    public class BuyerRepository : IBuyerRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ApprovalNotification _approvalNotification;

        public BuyerRepository(ApplicationDbContext dbContext, ApprovalNotification approvalNotification)
        {
            _dbContext = dbContext;
            _approvalNotification = approvalNotification;
        }

        public async Task<OperationResult<string>> PlaceBid(decimal amount, int auctionId, string userId)
        {
            try
            {
                var auction = await _dbContext.AuctionsTbl.FindAsync(auctionId);
                if (auction == null)
                    return OperationResult<string>.FailureResult("Auction not found");

                if (auction.IsClosed)
                    return OperationResult<string>.FailureResult("Auction is closed");

                var highestBid = await _dbContext.BidsTbl
                    .Where(b => b.AuctionId == auctionId)
                    .OrderByDescending(b => b.Amount)
                    .FirstOrDefaultAsync();

                if (highestBid != null && amount <= highestBid.Amount)
                    return OperationResult<string>.FailureResult("Bid amount must be greater than the current highest bid.");

                var bidResult = await UpdateOrAddBid(amount, auctionId, userId);

                if (!bidResult.IsSuccess)
                    return bidResult;

                // Add the user to the chat room
                var chatRoom = await _dbContext.ChatRoomsTbl
                    .FirstOrDefaultAsync(cr => cr.AuctionId == auctionId);

                if (chatRoom != null)
                {
                    var user = await _dbContext.Users.FindAsync(userId);
                    if (user != null)
                    {
                        if (chatRoom.Participants == null)
                        {
                            chatRoom.Participants = new List<ApplicationUser>();
                        }

                        if (!chatRoom.Participants.Contains(user))
                        {
                            chatRoom.Participants.Add(user);
                            _dbContext.ChatRoomsTbl.Update(chatRoom);
                            await _dbContext.SaveChangesAsync();
                        }
                    }
                }

                return OperationResult<string>.SuccessResult("Bid placed or updated successfully");
            }
            catch (Exception ex)
            {
                return OperationResult<string>.FailureResult($"An error occurred: {ex.Message}");
            }
        }

        private async Task<OperationResult<string>> UpdateOrAddBid(decimal amount, int auctionId, string userId)
        {
            var existingBid = await _dbContext.BidsTbl
                .FirstOrDefaultAsync(b => b.AuctionId == auctionId && b.UserId == userId);

            if (existingBid != null)
            {
                // Update existing bid
                existingBid.Amount = amount;
                existingBid.BidTime = DateTime.UtcNow;
                _dbContext.BidsTbl.Update(existingBid);
            }
            else
            {
                // Place new bid
                var bid = new Bid
                {
                    Amount = amount,
                    AuctionId = auctionId,
                    BidTime = DateTime.UtcNow,
                    UserId = userId
                };

                await _dbContext.BidsTbl.AddAsync(bid);
            }

            // Attempt to save changes
            try
            {
                await _dbContext.SaveChangesAsync();
                return OperationResult<string>.SuccessResult("Bid processed successfully");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return OperationResult<string>.FailureResult($"A concurrency error occurred: {ex.Message}");
            }
        }






        //    public async Task<OperationResult<string>> EditAuctionItem(int itemId, EditAuctionItemDto editAuctionItemDto)
        //    {
        //        try
        //        {
        //            var auctionItem = await _dbContext.AuctionItemsTbl.FindAsync(itemId);
        //            if (auctionItem == null)
        //                return OperationResult<string>.NotFoundResult();

        //            // Only update properties if provided
        //            if (!string.IsNullOrEmpty(editAuctionItemDto.Name))
        //            {
        //                auctionItem.Name = editAuctionItemDto.Name;
        //            }

        //            if (!string.IsNullOrEmpty(editAuctionItemDto.Description))
        //            {
        //                auctionItem.Description = editAuctionItemDto.Description;
        //            }

        //            if (editAuctionItemDto.Photo != null)
        //            {
        //                using var stream = editAuctionItemDto.Photo.OpenReadStream();
        //                using var memoryStream = new MemoryStream();
        //                await stream.CopyToAsync(memoryStream);
        //                auctionItem.Photo = memoryStream.ToArray();
        //            }

        //            _dbContext.AuctionItemsTbl.Update(auctionItem);
        //            var result = await _dbContext.SaveChangesAsync();

        //            return result > 0
        //                ? OperationResult<string>.SuccessResult("Auction item edited successfully")
        //                : OperationResult<string>.FailureResult("Failed to edit auction item");
        //        }
        //        catch (Exception ex)
        //        {
        //            return OperationResult<string>.FailureResult($"An error occurred: {ex.Message}");
        //        }
        //    }


        //    public async Task<OperationResult<string>> DeleteAuctionItem(int itemId)
        //    {
        //        try
        //        {
        //            var auctionItem = await _dbContext.AuctionItemsTbl.FindAsync(itemId);
        //            if (auctionItem == null)
        //                return OperationResult<string>.NotFoundResult();

        //            _dbContext.AuctionItemsTbl.Remove(auctionItem);
        //            var result = await _dbContext.SaveChangesAsync();

        //            return result > 0
        //                ? OperationResult<string>.SuccessResult("Auction item deleted successfully")
        //                : OperationResult<string>.FailureResult("Failed to delete auction item");
        //        }
        //        catch (Exception ex)
        //        {
        //            return OperationResult<string>.FailureResult($"An error occurred: {ex.Message}");
        //        }
        //    }

        //    public async Task<OperationResult<ViewAuctionItemDto>> GetAuctionItemById(int itemId)
        //    {
        //        try
        //        {
        //            var auctionItem = await _dbContext.AuctionItemsTbl.FindAsync(itemId);
        //            if (auctionItem == null)
        //                return OperationResult<ViewAuctionItemDto>.NotFoundResult();

        //            var auctionItemDto = new ViewAuctionItemDto
        //            {
        //                Name = auctionItem.Name,
        //                Description = auctionItem.Description,
        //                Photo = Convert.ToBase64String(auctionItem.Photo),
        //                Status = auctionItem.Status.ToString()
        //            };

        //            return OperationResult<ViewAuctionItemDto>.SuccessResult("Item found", auctionItemDto);
        //        }
        //        catch (Exception ex)
        //        {
        //            return OperationResult<ViewAuctionItemDto>.FailureResult($"An error occurred: {ex.Message}");
        //        }
        //    }

        //    public async Task<OperationResult<IEnumerable<ViewAuctionItemDto>>> GetAllAuctionItems(AuctionItemStatus? status)
        //    {
        //        try
        //        {
        //            // Retrieve items based on status or all items if status is null
        //            var auctionItemsQuery = _dbContext.AuctionItemsTbl.AsQueryable();

        //            // If a status is provided, filter by that status
        //            if (status.HasValue)
        //            {
        //                auctionItemsQuery = auctionItemsQuery.Where(a => a.Status == status.Value);
        //            }

        //            var auctionItems = await auctionItemsQuery.ToListAsync();

        //            // Check if no items are found
        //            if (auctionItems == null || !auctionItems.Any())
        //            {
        //                return OperationResult<IEnumerable<ViewAuctionItemDto>>.NotFoundResult();
        //            }

        //            // Map to DTO
        //            var auctionItemDtos = auctionItems.Select(auctionItem => new ViewAuctionItemDto
        //            {
        //                Name = auctionItem.Name,
        //                Description = auctionItem.Description,
        //                Status = auctionItem.Status.ToString(),
        //                Photo = Convert.ToBase64String(auctionItem.Photo)
        //            }).ToList();

        //            return OperationResult<IEnumerable<ViewAuctionItemDto>>.SuccessResult("Auction items retrieved successfully", auctionItemDtos);
        //        }
        //        catch (Exception ex)
        //        {
        //            return OperationResult<IEnumerable<ViewAuctionItemDto>>.FailureResult($"An error occurred: {ex.Message}");
        //        }
        //    }
        //}

    }
}

