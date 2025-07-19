using System;

namespace BloggerProUI.Models.Chat
{
    public class MarkMessagesAsReadDto
    {
        public Guid SenderId { get; set; }
        public List<Guid> MessageIds { get; set; } = new List<Guid>();
    }
}