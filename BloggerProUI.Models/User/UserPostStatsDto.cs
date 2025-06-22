using BloggerProUI.Models.PostModule;

namespace BloggerProUI.Models.User
{
    public class UserPostStatsDto
    {
        public int TotalPosts { get; set; }
        public int PublishedPosts { get; set; }
        public int DraftPosts { get; set; }
        public int ArchivedPosts { get; set; }
        public int TotalLikesReceived { get; set; }
        public int TotalCommentsReceived { get; set; }
        public double? AveragePostRating { get; set; }
    }
}
