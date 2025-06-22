using System.ComponentModel.DataAnnotations;

namespace BloggerProUI.Models.Tag;

public class TagUpdateDto
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Etiket adı boş olamaz.")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
}
