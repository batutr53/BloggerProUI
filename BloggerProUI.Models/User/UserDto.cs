using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggerProUI.Models.User
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public string? ProfileImage { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? Bio { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsModerator { get; set; }

        // Additional properties can be added as needed
        public string DisplayName => Username;

        public string? GetProfileImageUrl()
        {
            if (!string.IsNullOrEmpty(ProfileImageUrl))
                return ProfileImageUrl;

            // Return a default profile image if none is set
            return "/images/default-profile.png";
        }
    }
}
