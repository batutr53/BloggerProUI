namespace BloggerProUI.Models.Contact;

public class ContactListDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsReplied { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? RepliedAt { get; set; }
    public string? AdminReply { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
}