using BloggerProUI.Models.UserDashboard;

namespace BloggerProUI.Web.Models
{
    public class UserDashboardViewModel
    {
        public UserDashboardStatsDto Stats { get; set; } = new();
        public List<UserActivityDto> Activities { get; set; } = new();
        public List<RecentPostDto> RecentPosts { get; set; } = new();
        public Dictionary<string, int> ReadingStats { get; set; } = new();
    }
}