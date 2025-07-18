namespace BloggerProUI.Models.UserDashboard
{
    public class UserActivityDto
    {
        public Guid Id { get; set; }
        public string ActivityType { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime ActivityDate { get; set; }
        public string PostSlug { get; set; } = string.Empty;
        public string PostTitle { get; set; } = string.Empty;
        public string PostImage { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
    }
}