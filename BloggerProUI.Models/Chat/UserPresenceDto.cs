using System;

namespace BloggerProUI.Models.Chat
{
    public class UserPresenceDto
    {
        public Guid UserId { get; set; }
        public bool IsOnline { get; set; }
        public DateTime LastSeen { get; set; }
    }
}