using System.ComponentModel.DataAnnotations;

namespace BloggerProUI.Models.Contact;

public class ContactReplyDto
{
    [Required(ErrorMessage = "Yanıt metni zorunludur.")]
    [StringLength(2000, ErrorMessage = "Yanıt en fazla 2000 karakter olabilir.")]
    public string AdminReply { get; set; } = string.Empty;
}