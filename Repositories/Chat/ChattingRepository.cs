using Live_Bidding_System_App.DataContext;
using Live_Bidding_System_App.Helper;
using Live_Bidding_System_App.Hubs;
using Live_Bidding_System_App.Models.Chats;
using Live_Bidding_System_App.Repositories.Chat.DTO;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Live_Bidding_System_App.Repositories.Chat
{
    public class ChattingRepository : IChattingRepository
    {
        private readonly IHubContext<ChatHub> _chatHub; // Inject SignalR Hub
        private readonly ApplicationDbContext _dbContext;

        public ChattingRepository(IHubContext<ChatHub> chatHub, ApplicationDbContext dbContext)
        {
            _chatHub = chatHub;
            _dbContext = dbContext;
        }

        public async Task<OperationResult<string>> CreateChat(CreatechatDto createChatDto, int auctionId, string userId)
        {
            try
            {
                var chatRoom = await _dbContext.ChatRoomsTbl
                    .Include(a => a.Participants)
                    .Where(a => a.AuctionId == auctionId && a.Participants.Any(p => p.Id == userId))
                    .FirstOrDefaultAsync();

                if (chatRoom != null)
                {
                    var chatMessage = new ChatMessage
                    {
                        Content = createChatDto.Content,
                        ChatRoomId = chatRoom.Id,
                        UserId = userId,
                        SentTime = DateTime.UtcNow
                    };

                    await _dbContext.ChatMessagesTbl.AddAsync(chatMessage);
                    var result = await _dbContext.SaveChangesAsync();

                    if (result > 0)
                    {
                        await _chatHub.Clients.Group(chatRoom.Id.ToString())
                            .SendAsync("ReceiveMessage", new
                            {
                                Content = chatMessage.Content,
                                UserId = chatMessage.UserId,
                                SentTime = chatMessage.SentTime
                            });

                        return OperationResult<string>.SuccessResult("Message sent successfully.");
                    }

                    return OperationResult<string>.FailureResult("Failed to send message.");
                }

                return OperationResult<string>.FailureResult("User is not a participant in the chat room.");
            }
            catch (Exception ex)
            {
                return OperationResult<string>.FailureResult($"An error occurred: {ex.Message}");
            }
        }

        public async Task<OperationResult<IEnumerable<ViewChatsDto>>> GetAllChatsByRoomId(int roomId)
        {
            try
            {
                // Retrieve chat messages for the specified room
                var chats = await _dbContext.ChatMessagesTbl
                    .Include(cm => cm.User) // Include user details to get user names
                    .Where(cm => cm.ChatRoomId == roomId)
                    .OrderBy(cm => cm.SentTime) // Optionally order by time
                    .ToListAsync();

                // Check if any chats were found
                if (chats == null || !chats.Any())
                    return OperationResult<IEnumerable<ViewChatsDto>>.NotFoundResult();

                // Map chat messages to DTOs
                var chatDtos = chats.Select(chat => new ViewChatsDto
                {
                    Content = chat.Content,
                    SentTime = chat.SentTime,
                    UserName = chat.User.UserName // Assuming ApplicationUser has a UserName property
                }).ToList();

                return OperationResult<IEnumerable<ViewChatsDto>>.SuccessResult("Chats retrieved successfully.", chatDtos);

            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<ViewChatsDto>>.FailureResult($"An error occurred: {ex.Message}");
            }
        }
    }
}
