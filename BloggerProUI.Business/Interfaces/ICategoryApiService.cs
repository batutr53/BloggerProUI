using BloggerProUI.Models.Category;
using BloggerProUI.Shared.Utilities.Results;

namespace BloggerProUI.Business.Interfaces
{
    public interface ICategoryApiService
    {
        Task<IDataResult<List<CategoryDto>>> GetAllAsync();
        Task<IDataResult<CategoryDto>> GetByIdAsync(Guid id);
        Task<IResult> CreateAsync(CategoryCreateDto dto);
        Task<IResult> UpdateAsync(CategoryUpdateDto dto);
        Task<IResult> DeleteAsync(Guid id);
    }
}
