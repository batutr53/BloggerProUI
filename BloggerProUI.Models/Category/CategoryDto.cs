namespace BloggerProUI.Models.Category
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? PostCount { get; set; }
    }
}
