using System.ComponentModel.DataAnnotations;

namespace BloggerProUI.Models.AboutUs;

public class AboutUsCreateDto
{
    [Required(ErrorMessage = "Başlık alanı zorunludur.")]
    [StringLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "İçerik alanı zorunludur.")]
    public string Content { get; set; } = string.Empty;

    [Required(ErrorMessage = "Misyon alanı zorunludur.")]
    public string Mission { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vizyon alanı zorunludur.")]
    public string Vision { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    [Range(0, int.MaxValue, ErrorMessage = "Sıralama 0'dan büyük olmalıdır.")]
    public int SortOrder { get; set; } = 0;
}