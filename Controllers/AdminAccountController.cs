using Live_Bidding_System_App.Models.Seller;
using Live_Bidding_System_App.Repositories.Admin;
using Microsoft.AspNetCore.Mvc;

namespace Live_Bidding_System_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminAccountController : ControllerBase
    {
        private readonly IAdminRepository _adminRepository;

        public AdminAccountController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        // [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("createAuctionItem")]
        public async Task<IActionResult> CreateAuctionItem(int itemId, AuctionItemStatus status)
        {
            try
            {
                var result = await _adminRepository.ItemApproval(itemId, status);

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
    }
}
