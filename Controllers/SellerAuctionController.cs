using Live_Bidding_System_App.Models.Seller;
using Live_Bidding_System_App.Repositories.Seller;
using Live_Bidding_System_App.Repositories.Seller.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Live_Bidding_System_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellerAuctionController : ControllerBase
    {
        private readonly ISellerRepository _sellerRepository;

        public SellerAuctionController(ISellerRepository sellerRepository)
        {
            _sellerRepository = sellerRepository;
        }

        // [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("createAuctionItem")]
        public async Task<IActionResult> CreateAuctionItem([FromForm] CreateAuctionItemDto createAuctionItemDto)
        {
            try
            {
                var result = await _sellerRepository.CreateAuctionItem(createAuctionItemDto);

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

        // [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut("editAuctionItem/{itemId}")]
        public async Task<IActionResult> EditAuctionItem(int itemId, [FromForm] EditAuctionItemDto editAuctionItemDto)
        {
            try
            {
                var result = await _sellerRepository.EditAuctionItem(itemId, editAuctionItemDto);

                if (result.IsSuccess)
                {
                    return Ok(result.Message); // Success case
                }

                return result.Message == "Item not found" ? NotFound(result.Message) : BadRequest(result.Message); // NotFound or Failure case
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing your request: {ex.Message}");
            }
        }

        // [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("deleteAuctionItem/{itemId}")]
        public async Task<IActionResult> DeleteAuctionItem(int itemId)
        {
            try
            {
                var result = await _sellerRepository.DeleteAuctionItem(itemId);

                if (result.IsSuccess)
                {
                    return Ok(result.Message); // Success case
                }

                return result.Message == "Item not found" ? NotFound(result.Message) : BadRequest(result.Message); // NotFound or Failure case
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing your request: {ex.Message}");
            }
        }

        [HttpGet("getAllAuctionItems")]
        public async Task<IActionResult> GetAllAuctionItems(AuctionItemStatus? status)
        {
            try
            {
                var result = await _sellerRepository.GetAllAuctionItems(status);

                if (result.IsSuccess && result.Data != null && result.Data.Any())
                {
                    return Ok(result.Data); // Success case
                }

                return NotFound(result.Message); // NotFound case
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing your request: {ex.Message}");
            }
        }

        [HttpGet("getAuctionItemById/{itemId}")]
        public async Task<IActionResult> GetAuctionItemById(int itemId)
        {
            try
            {
                var result = await _sellerRepository.GetAuctionItemById(itemId);

                if (result.IsSuccess)
                {
                    return Ok(result.Data); // Success case
                }

                return result.Message == "Item not found" ? NotFound(result.Message) : BadRequest(result.Message); // NotFound or Failure case
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing your request: {ex.Message}");
            }
        }
    }
}
