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
        //   [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("createAuctionItem")]
        public async Task<IActionResult> CreateAuctionItem([FromForm] CreateAuctionItemDto createAuctionItemDto)
        {
            try
            {
                var result = await _sellerRepository.CreateAuctionItem(createAuctionItemDto);

                if (result == "Successfully Saved")
                {
                    return Ok(result);
                }
                return BadRequest("Failed to save Auction Item");
            }

            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        //  [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut("editAuctionItem/{itemId}")]
        public async Task<IActionResult> EditAuctionItem(int itemId, [FromForm] CreateAuctionItemDto createAuctionItemDto)
        {
            try
            {
                var result = await _sellerRepository.EditAuctionItem(itemId, createAuctionItemDto);

                if (result == "Auction Item Edited Successfully")
                {
                    return Ok(result);
                }
                return NotFound(result);
            }

            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        //    [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("deleteAuctionItem/{itemId}")]
        public async Task<IActionResult> DeleteAuctionItem(int itemId)
        {
            try
            {
                var result = await _sellerRepository.DeleteAuctionItem(itemId);

                if (result == "Auction Item Deleted Successfully")
                {
                    return Ok(result);
                }
                return NotFound(result);
            }

            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("getAllAuctionItems")]
        public async Task<IActionResult> GetAllAuctionItems()
        {
            try
            {
                var result = await _sellerRepository.GetAllAuctionItems();
                if (result != null && result.Any())
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound();
                }
            }

            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpGet("getAuctionItemById/{itemId}")]
        public async Task<IActionResult> GetAuctionItemById(int itemId)
        {
            try
            {
                var result = await _sellerRepository.GetAuctionItemById(itemId);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound();
                }
            }

            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
