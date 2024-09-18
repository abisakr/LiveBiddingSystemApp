using Live_Bidding_System_App.Models.Chats;
using Microsoft.AspNetCore.SignalR;

namespace Live_Bidding_System_App.Hubs
{
    public class ChatHub : Hub
    {
        // Method to join a chat room
        public async Task JoinChatRoom(string chatRoomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatRoomId);
        }

        // Method to leave a chat room
        public async Task LeaveChatRoom(string chatRoomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatRoomId);
        }

        // Method to send a message to a specific group
        public async Task SendMessage(string chatRoomId, ChatMessage message)
        {
            await Clients.Group(chatRoomId).SendAsync("ReceiveMessage", message);
        }
    }

}
