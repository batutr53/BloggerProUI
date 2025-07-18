namespace BloggerProUI.Models.Bookmark
{
    public class BookmarkCreateDto
    {
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public string? Notes { get; set; }
    }
}