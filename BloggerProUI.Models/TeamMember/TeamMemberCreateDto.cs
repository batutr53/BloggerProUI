using System.ComponentModel.DataAnnotations;

namespace BloggerProUI.Models.TeamMember;

public class TeamMemberCreateDto
{
    [Required(ErrorMessage = "İsim alanı zorunludur.")]
    [StringLength(100, ErrorMessage = "İsim en fazla 100 karakter olabilir.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Pozisyon alanı zorunludur.")]
    [StringLength(100, ErrorMessage = "Pozisyon en fazla 100 karakter olabilir.")]
    public string Position { get; set; } = string.Empty;

    [Required(ErrorMessage = "Departman alanı zorunludur.")]
    [StringLength(100, ErrorMessage = "Departman en fazla 100 karakter olabilir.")]
    public string Department { get; set; } = string.Empty;

    public string? Bio { get; set; }

    public string? ImageUrl { get; set; }

    [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
    public string? Email { get; set; }

    [Url(ErrorMessage = "Geçerli bir LinkedIn URL'si giriniz.")]
    public string? LinkedInUrl { get; set; }

    [Url(ErrorMessage = "Geçerli bir Twitter URL'si giriniz.")]
    public string? TwitterUrl { get; set; }

    public bool IsActive { get; set; } = true;

    [Range(0, int.MaxValue, ErrorMessage = "Sıralama 0'dan büyük olmalıdır.")]
    public int SortOrder { get; set; } = 0;
}