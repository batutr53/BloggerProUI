using BloggerProUI.Models.Contact;
using BloggerProUI.Models.Pagination;
using BloggerProUI.Shared.Utilities.Results;

namespace BloggerProUI.Business.Interfaces;

public interface IContactApiService
{
    Task<Result> CreateContactAsync(ContactCreateDto contactCreateDto);
    Task<DataResult<PaginatedResultDto<ContactListDto>>> GetAllContactsAsync(int page = 1, int pageSize = 10, bool? isReplied = null);
    Task<DataResult<ContactListDto>> GetContactByIdAsync(Guid id);
    Task<Result> ReplyToContactAsync(Guid id, ContactReplyDto contactReplyDto);
    Task<Result> DeleteContactAsync(Guid id);
    Task<Result> MarkAsRepliedAsync(Guid id);
}