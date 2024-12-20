﻿using Live_Bidding_System_App.DataContext;
using Live_Bidding_System_App.Helper;
using Live_Bidding_System_App.Models;
using Live_Bidding_System_App.Models.Buyer;
using Live_Bidding_System_App.Repositories.Buyer.DTO;
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

        public async Task<OperationResult<IEnumerable<ViewAuctionsDto>>> GetAllAuctions()
        {
            try
            {
                var auctions = _dbContext.AuctionsTbl.Include(i => i.AuctionItem).ThenInclude(i => i.AuctionItemCategory).Where(i => i.IsClosed == false);
                var auctionsDto = await auctions.Select(auctions => new ViewAuctionsDto
                {
                    Title = auctions.Title,
                    Description = auctions.Description,
                    Photo = Convert.ToBase64String(auctions.AuctionItem.Photo),
                    Category = auctions.AuctionItem.AuctionItemCategory.CategoryName,
                    StartDate = auctions.StartDate,
                    EndDate = auctions.EndDate
                }).ToListAsync();
                return OperationResult<IEnumerable<ViewAuctionsDto>>.SuccessResult("Auctions retrieved successfully", auctionsDto);

            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<ViewAuctionsDto>>.FailureResult($"An error occurred: {ex.Message}");
            }
        }
        public async Task<OperationResult<IEnumerable<ViewAuctionsDto>>> GetAuctionsByUserId(string userId)
        {
            try
            {
                var auctions = _dbContext.AuctionsTbl.Include(i => i.AuctionItem).ThenInclude(i => i.AuctionItemCategory).Where(i => i.IsClosed == false && i.AuctionItem.UserId == userId);
                var auctionsDto = await auctions.Select(auctions => new ViewAuctionsDto
                {
                    Title = auctions.Title,
                    Description = auctions.Description,
                    Photo = Convert.ToBase64String(auctions.AuctionItem.Photo),
                    Category = auctions.AuctionItem.AuctionItemCategory.CategoryName,
                    StartDate = auctions.StartDate,
                    EndDate = auctions.EndDate
                }).ToListAsync();
                return OperationResult<IEnumerable<ViewAuctionsDto>>.SuccessResult("Auctions retrieved successfully", auctionsDto);

            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<ViewAuctionsDto>>.FailureResult($"An error occurred: {ex.Message}");
            }
        }
        public async Task<OperationResult<IEnumerable<ViewAuctionsDto>>> GetAuctionsByCategory(string category)
        {
            try
            {
                var auctions = _dbContext.AuctionsTbl.Include(i => i.AuctionItem).ThenInclude(i => i.AuctionItemCategory).Where(i => i.IsClosed == false && i.AuctionItem.AuctionItemCategory.CategoryName == category);
                var auctionsDto = await auctions.Select(auctions => new ViewAuctionsDto
                {
                    Title = auctions.Title,
                    Description = auctions.Description,
                    Photo = Convert.ToBase64String(auctions.AuctionItem.Photo),
                    Category = auctions.AuctionItem.AuctionItemCategory.CategoryName,
                    StartDate = auctions.StartDate,
                    EndDate = auctions.EndDate
                }).ToListAsync();
                return OperationResult<IEnumerable<ViewAuctionsDto>>.SuccessResult("Auctions retrieved successfully", auctionsDto);

            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<ViewAuctionsDto>>.FailureResult($"An error occurred: {ex.Message}");
            }
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
        public async Task<OperationResult<IEnumerable<ViewBidsDto>>> GetMyBidsByUserId(string userId)
        {
            try
            {
                var auctions = _dbContext.BidsTbl.Include(i => i.Auction).ThenInclude(i => i.AuctionItem.AuctionItemCategory).Where(i => i.UserId == userId);
                var auctionsDto = await auctions.Select(auctions => new ViewBidsDto
                {
                    Title = auctions.Auction.Title,
                    Description = auctions.Auction.Description,
                    Photo = Convert.ToBase64String(auctions.Auction.AuctionItem.Photo),
                    Category = auctions.Auction.AuctionItem.AuctionItemCategory.CategoryName,
                    Amount = (double)auctions.Amount,
                    StartDate = auctions.Auction.StartDate,
                    EndDate = auctions.Auction.EndDate
                }).ToListAsync();
                return OperationResult<IEnumerable<ViewBidsDto>>.SuccessResult("Bids retrieved successfully", auctionsDto);

            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<ViewBidsDto>>.FailureResult($"An error occurred: {ex.Message}");
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
    }
}

