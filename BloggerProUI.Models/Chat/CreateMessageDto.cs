using System;

namespace BloggerProUI.Models.Chat
{
    public class CreateMessageDto
    {
        public Guid ReceiverId { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}