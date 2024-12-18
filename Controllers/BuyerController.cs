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
        [HttpPost("GetMyBidsByUserId/{userId}")]
        public async Task<IActionResult> GetMyBidsByUserId()
        {
            try
            {
                string userId = "30c19a4a-6b00-4a93-b0e4-4aedc446409e";
                var result = await _buyerRepository.GetMyBidsByUserId(userId);

                if (result.IsSuccess)
                {
                    return Ok(new { result.Message, result.Data }); // Success case
                }

                return BadRequest(result.Message); // Failure case
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing your request: {ex.Message}");
            }
        }

        [HttpGet("getAllAuctions")]
        public async Task<IActionResult> GetAllAuctions()
        {
            try
            {
                var result = await _buyerRepository.GetAllAuctions();

                if (result.IsSuccess)
                {
                    return Ok(new { result.Message, result.Data }); // Success case
                }

                return BadRequest(result.Message); // Failure case
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing your request: {ex.Message}");
            }
        }

        //get my placed auctions by user id
        [HttpGet("getAuctionsByUserId/{userId}")]
        public async Task<IActionResult> GetAuctionsByUserId()
        {
            try
            {
                string userId = "30c19a4a-6b00-4a93-b0e4-4aedc446409e";
                var result = await _buyerRepository.GetAuctionsByUserId(userId);
                if (result.IsSuccess)
                {
                    return Ok(new { result.Message, result.Data }); // Success case
                }
                return BadRequest(result.Message);
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing your request: {ex.Message}");
            }
        }
        [HttpGet("getAuctionsByCategory/{category}")]
        public async Task<IActionResult> GetAuctionsByCategory(string category)
        {
            try
            {
                var result = await _buyerRepository.GetAuctionsByCategory(category);
                if (result.IsSuccess)
                {
                    return Ok(new { result.Message, result.Data }); // Success case
                }
                return BadRequest(result.Message);
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing your request: {ex.Message}");
            }
        }
    }

}

