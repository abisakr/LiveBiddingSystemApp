using Live_Bidding_System_App.Repositories.Buyer;
using Microsoft.AspNetCore.Mvc;

namespace Live_Bidding_System_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuyerController : ControllerBase
    {
        private readonly IBuyerRepository _buyerRepository;

        public BuyerController(IBuyerRepository buyerRepository)
        {
            _buyerRepository = buyerRepository;
        }

        // [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("placeBid/{auctionId}")]
        public async Task<IActionResult> PlaceBid(decimal amount, int auctionId)
        {
            try
            {
                string userId = "30c19a4a-6b00-4a93-b0e4-4aedc446409e";
                var result = await _buyerRepository.PlaceBid(amount, auctionId, userId);

                if (result.IsSuccess)
                {
                    return Ok(result.Message); // Success case
                }

                return BadRequest(result.Message); // Failure case
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing your request: {ex.Message}");
            }
        }

        //get all auctions using filter
        //get my placed auctions by user id
        //get auctions by category
    }
}
