using System.ComponentModel.DataAnnotations;

namespace BloggerProUI.Models.Category
{
    public class CategoryUpdateDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Slug { get; set; }
    }
}
