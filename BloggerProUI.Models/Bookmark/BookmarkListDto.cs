using BloggerProUI.Models.Post;

namespace BloggerProUI.Models.Bookmark
{
    public class BookmarkListDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public DateTime BookmarkedAt { get; set; }
        public string? Notes { get; set; }
        public PostListDto Post { get; set; } = null!;
    }
}