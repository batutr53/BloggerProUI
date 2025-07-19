using System.ComponentModel.DataAnnotations;

namespace BloggerProUI.Models.User
{
    public class UpdateUserProfileDto
    {
        [StringLength(50, ErrorMessage = "Ad 50 karakterden fazla olamaz")]
        public string? FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Soyad 50 karakterden fazla olamaz")]
        public string? LastName { get; set; }

        [StringLength(500, ErrorMessage = "Biyografi 500 karakterden fazla olamaz")]
        public string? Bio { get; set; }

        [Url(ErrorMessage = "Lütfen geçerli bir profil resmi URL'si girin")]
        public string? ProfileImageUrl { get; set; }

        [StringLength(100, ErrorMessage = "Konum 100 karakterden fazla olamaz")]
        public string? Location { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [Url(ErrorMessage = "Lütfen geçerli bir URL girin")]
        public string? Website { get; set; }

        [Url(ErrorMessage = "Lütfen geçerli bir Facebook URL'si girin")]
        public string? FacebookUrl { get; set; }

        [Url(ErrorMessage = "Lütfen geçerli bir Twitter URL'si girin")]
        public string? TwitterUrl { get; set; }

        [Url(ErrorMessage = "Lütfen geçerli bir Instagram URL'si girin")]
        public string? InstagramUrl { get; set; }

        [Url(ErrorMessage = "Lütfen geçerli bir LinkedIn URL'si girin")]
        public string? LinkedInUrl { get; set; }

        [Url(ErrorMessage = "Lütfen geçerli bir TikTok URL'si girin")]
        public string? TikTokUrl { get; set; }

        [Url(ErrorMessage = "Lütfen geçerli bir YouTube URL'si girin")]
        public string? YouTubeUrl { get; set; }
    }
}