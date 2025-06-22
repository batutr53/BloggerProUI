namespace BloggerProUI.Models.PostModule
{
    public class PostStatsDto
    {
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public double? AverageRating { get; set; }
        public int RatingCount { get; set; }
        public Dictionary<int, int> RatingDistribution { get; set; } = new();
    }
}
