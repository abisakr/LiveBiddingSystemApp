using Live_Bidding_System_App.DataContext;
using Live_Bidding_System_App.Helper;
using Live_Bidding_System_App.Models.Seller;
using Live_Bidding_System_App.Repositories.Seller.DTO;
using Microsoft.EntityFrameworkCore;

namespace Live_Bidding_System_App.Repositories.Seller
{

    public class SellerRepository : ISellerRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ApprovalNotification _approvalNotification;

        public SellerRepository(ApplicationDbContext dbContext, ApprovalNotification approvalNotification)
        {
            _dbContext = dbContext;
            _approvalNotification = approvalNotification;
        }

        public async Task<OperationResult<string>> CreateAuctionItem(CreateAuctionItemDto createAuctionItemDto, string userId)
        {
            try
            {
                using var stream = createAuctionItemDto.Photo.OpenReadStream();
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);

                var auctionItem = new AuctionItem
                {
                    Name = createAuctionItemDto.Name,
                    Description = createAuctionItemDto.Description,
                    Photo = memoryStream.ToArray(),
                    UserId = userId
                };

                await _dbContext.AuctionItemsTbl.AddAsync(auctionItem);
                var result = await _dbContext.SaveChangesAsync();

                if (result > 0)
                {
                    await _approvalNotification.OnAuctionItemCreated(auctionItem.Name);
                    return OperationResult<string>.SuccessResult("Auction Item Successfully saved");
                }
                return OperationResult<string>.FailureResult("Failed to save auction item");

            }
            catch (Exception ex)
            {
                return OperationResult<string>.FailureResult($"An error occurred: {ex.Message}");
            }
        }

        public async Task<OperationResult<string>> EditAuctionItem(int itemId, EditAuctionItemDto editAuctionItemDto)
        {
            try
            {
                var auctionItem = await _dbContext.AuctionItemsTbl.FindAsync(itemId);
                if (auctionItem == null)
                    return OperationResult<string>.NotFoundResult();

                // Only update properties if provided
                if (!string.IsNullOrEmpty(editAuctionItemDto.Name))
                {
                    auctionItem.Name = editAuctionItemDto.Name;
                }

                if (!string.IsNullOrEmpty(editAuctionItemDto.Description))
                {
                    auctionItem.Description = editAuctionItemDto.Description;
                }

                if (editAuctionItemDto.Photo != null)
                {
                    using var stream = editAuctionItemDto.Photo.OpenReadStream();
                    using var memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);
                    auctionItem.Photo = memoryStream.ToArray();
                }

                _dbContext.AuctionItemsTbl.Update(auctionItem);
                var result = await _dbContext.SaveChangesAsync();

                return result > 0
                    ? OperationResult<string>.SuccessResult("Auction item edited successfully")
                    : OperationResult<string>.FailureResult("Failed to edit auction item");
            }
            catch (Exception ex)
            {
                return OperationResult<string>.FailureResult($"An error occurred: {ex.Message}");
            }
        }


        public async Task<OperationResult<string>> DeleteAuctionItem(int itemId)
        {
            try
            {
                var auctionItem = await _dbContext.AuctionItemsTbl.FindAsync(itemId);
                if (auctionItem == null)
                    return OperationResult<string>.NotFoundResult();

                _dbContext.AuctionItemsTbl.Remove(auctionItem);
                var result = await _dbContext.SaveChangesAsync();

                return result > 0
                    ? OperationResult<string>.SuccessResult("Auction item deleted successfully")
                    : OperationResult<string>.FailureResult("Failed to delete auction item");
            }
            catch (Exception ex)
            {
                return OperationResult<string>.FailureResult($"An error occurred: {ex.Message}");
            }
        }

        public async Task<OperationResult<ViewAuctionItemDto>> GetAuctionItemById(int itemId)
        {
            try
            {
                var auctionItem = await _dbContext.AuctionItemsTbl.FindAsync(itemId);
                if (auctionItem == null)
                    return OperationResult<ViewAuctionItemDto>.NotFoundResult();

                var auctionItemDto = new ViewAuctionItemDto
                {
                    Name = auctionItem.Name,
                    Description = auctionItem.Description,
                    Photo = Convert.ToBase64String(auctionItem.Photo),
                    Status = auctionItem.Status.ToString()
                };

                return OperationResult<ViewAuctionItemDto>.SuccessResult("Item found", auctionItemDto);
            }
            catch (Exception ex)
            {
                return OperationResult<ViewAuctionItemDto>.FailureResult($"An error occurred: {ex.Message}");
            }
        }

        public async Task<OperationResult<IEnumerable<ViewAuctionItemDto>>> GetAllAuctionItems(AuctionItemStatus? status)
        {
            try
            {
                // Retrieve items based on status or all items if status is null
                var auctionItemsQuery = _dbContext.AuctionItemsTbl.AsQueryable();

                // If a status is provided, filter by that status
                if (status.HasValue)
                {
                    auctionItemsQuery = auctionItemsQuery.Where(a => a.Status == status.Value);
                }

                var auctionItems = await auctionItemsQuery.ToListAsync();

                // Check if no items are found
                if (auctionItems == null || !auctionItems.Any())
                {
                    return OperationResult<IEnumerable<ViewAuctionItemDto>>.NotFoundResult();
                }

                // Map to DTO
                var auctionItemDtos = auctionItems.Select(auctionItem => new ViewAuctionItemDto
                {
                    Name = auctionItem.Name,
                    Description = auctionItem.Description,
                    Status = auctionItem.Status.ToString(),
                    Photo = Convert.ToBase64String(auctionItem.Photo)
                }).ToList();

                return OperationResult<IEnumerable<ViewAuctionItemDto>>.SuccessResult("Auction items retrieved successfully", auctionItemDtos);
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<ViewAuctionItemDto>>.FailureResult($"An error occurred: {ex.Message}");
            }
        }
    }



}