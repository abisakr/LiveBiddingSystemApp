using System.ComponentModel.DataAnnotations;

namespace Live_Bidding_System_App.Models.Chats
{
    public class ChatMessage
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime SentTime { get; set; }

        public int ChatRoomId { get; set; }
        public ChatRoom ChatRoom { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }

}
