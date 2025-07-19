using System;

namespace BloggerProUI.Models.Chat
{
    public class MarkMessagesAsDeliveredDto
    {
        public Guid ReceiverId { get; set; }
        public List<Guid> MessageIds { get; set; } = new List<Guid>();
    }
}