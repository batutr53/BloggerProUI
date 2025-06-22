namespace BloggerProUI.Models.User
{
    public class ActiveUserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public int TotalComments { get; set; }
        public int TotalPosts { get; set; }
        public int TotalRatings { get; set; }
    }
}
