using Live_Bidding_System_App.Repositories.Chat;
using Live_Bidding_System_App.Repositories.Chat.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Live_Bidding_System_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChattingRepository _chattingRepository;

        public ChatController(IChattingRepository chattingRepository)
        {
            _chattingRepository = chattingRepository;
        }

        // [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("createChat")]
        public async Task<IActionResult> CreateChat(CreatechatDto createchatDto, int auctionId)
        {
            try
            {
                string userId = "30c19a4a-6b00-4a93-b0e4-4aedc446409e";
                var result = await _chattingRepository.CreateChat(createchatDto, auctionId, userId);

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

        [HttpGet("getAllChatsByRoomId/{roomId}")]
        public async Task<IActionResult> GetAllChatsByRoomId(int roomId)
        {
            try
            {
                var result = await _chattingRepository.GetAllChatsByRoomId(roomId);

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
