using BloggerProUI.Models.Post;
using BloggerProUI.Models.User;

namespace BloggerProUI.Models.Dashboard
{
    public class AdminDashboardStatsDto
    {
        public int TotalUsers { get; set; }
        public int TotalPosts { get; set; }
        public int TotalComments { get; set; }
        public int TotalLikes { get; set; }
        public int TotalRatings { get; set; }

        public List<PostDto> TopLikedPosts { get; set; }
        public List<PostDto> TopRatedPosts { get; set; }
        public List<ActiveUserDto> MostActiveUsers { get; set; }
    }
}
