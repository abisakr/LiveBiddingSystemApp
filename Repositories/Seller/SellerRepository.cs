using Live_Bidding_System_App.DataContext;
using Live_Bidding_System_App.Models.Seller;
using Live_Bidding_System_App.Repositories.Seller.DTO;
using Microsoft.EntityFrameworkCore;

namespace Live_Bidding_System_App.Repositories.Seller
{
    public class SellerRepository : ISellerRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SellerRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> CreateAuctionItem(CreateAuctionItemDto createAuctionItemDto)
        {
            try
            {
                // Read the photo into a byte array
                using var stream = createAuctionItemDto.Photo.OpenReadStream();
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                string userId = "dddd";
                // Create AuctionItem entity
                var auctionItem = new AuctionItem
                {
                    Name = createAuctionItemDto.Name,
                    Description = createAuctionItemDto.Description,
                    Photo = memoryStream.ToArray(),
                    UserId = userId
                };

                // Add and save to the database
                await _dbContext.AuctionItemsTbl.AddAsync(auctionItem);
                var result = await _dbContext.SaveChangesAsync();

                return result > 0 ? "Successfully Saved" : "Failed to save Auction Item";
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the Auction Item.", ex);
            }
        }

        public async Task<string> EditAuctionItem(int itemId, CreateAuctionItemDto createAuctionItemDto)
        {
            try
            {
                var auctionItem = await _dbContext.AuctionItemsTbl.FindAsync(itemId);

                if (auctionItem == null)
                    return "Auction Item Not Found";

                // Update fields
                auctionItem.Name = createAuctionItemDto.Name;
                auctionItem.Description = createAuctionItemDto.Description;
                // Update Photo if a new one is uploaded
                if (createAuctionItemDto.Photo != null)
                {
                    using var stream = createAuctionItemDto.Photo.OpenReadStream();
                    using var memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);
                    auctionItem.Photo = memoryStream.ToArray();
                }

                _dbContext.AuctionItemsTbl.Update(auctionItem);

                var result = await _dbContext.SaveChangesAsync();
                return result > 0 ? "Auction Item Edited Successfully" : "Failed to Edit Auction Item";
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while editing the Auction Item.", ex);
            }
        }

        public async Task<string> DeleteAuctionItem(int itemId)
        {
            try
            {
                var auctionItem = await _dbContext.AuctionItemsTbl.FindAsync(itemId);

                if (auctionItem == null)
                    return "Auction Item Not Found";

                _dbContext.AuctionItemsTbl.Remove(auctionItem);
                var result = await _dbContext.SaveChangesAsync();

                return result > 0 ? "Auction Item Deleted Successfully" : "Failed to Delete Auction Item";
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the Auction Item.", ex);
            }
        }

        public async Task<ViewAuctionItemDto> GetAuctionItemById(int itemId)
        {
            try
            {
                var auctionItem = await _dbContext.AuctionItemsTbl.FirstOrDefaultAsync(p => p.Id == itemId);

                if (auctionItem == null)
                    return null;

                return new ViewAuctionItemDto
                {
                    Name = auctionItem.Name,
                    Description = auctionItem.Description,
                    Photo = Convert.ToBase64String(auctionItem.Photo),
                    Status = auctionItem.Status.ToString()
                };
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching the Auction Item.", ex);
            }
        }

        public async Task<IEnumerable<ViewAuctionItemDto>> GetAllAuctionItems()
        {
            try
            {
                var auctionItems = await _dbContext.AuctionItemsTbl.ToListAsync();

                return auctionItems.Select(auctionItem => new ViewAuctionItemDto
                {
                    Name = auctionItem.Name,
                    Description = auctionItem.Description,
                    Status = auctionItem.Status.ToString(),
                    Photo = Convert.ToBase64String(auctionItem.Photo)
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Auction Items.", ex);
            }
        }
    }
}
