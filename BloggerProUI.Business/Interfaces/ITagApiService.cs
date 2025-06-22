using BloggerProUI.Models.Tag;
using BloggerProUI.Shared.Utilities.Results;

namespace BloggerProUI.Business.Interfaces
{
    public interface ITagApiService
    {
        Task<IDataResult<List<TagDto>>> GetAllAsync();
        Task<IDataResult<TagDto>> GetByIdAsync(Guid id);
        Task<IResult> CreateAsync(TagCreateDto dto);
        Task<IResult> UpdateAsync(TagUpdateDto dto);
        Task<IResult> DeleteAsync(Guid id);
    }
}
