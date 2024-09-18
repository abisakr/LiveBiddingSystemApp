using Live_Bidding_System_App.Helper;
using Live_Bidding_System_App.Repositories.Chat.DTO;

namespace Live_Bidding_System_App.Repositories.Chat
{
    public interface IChattingRepository
    {
        public Task<OperationResult<string>> CreateChat(CreatechatDto createchatDto, int auctionId, string userId);
        public Task<OperationResult<IEnumerable<ViewChatsDto>>> GetAllChatsByRoomId(int roomId);

    }
}
