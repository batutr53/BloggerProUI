namespace BloggerProUI.Models.Comment
{
    public class CommentListDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = null!;
        public string Username { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public int LikeCount { get; set; }
        public bool HasLiked { get; set; }
        public List<CommentListDto> Replies { get; set; } = new();
        public Guid? PostId { get; set; }
        public string? PostTitle { get; set; }
    }
}
