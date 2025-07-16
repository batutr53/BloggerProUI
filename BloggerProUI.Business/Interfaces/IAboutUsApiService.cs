using BloggerProUI.Models.AboutUs;
using BloggerProUI.Shared.Utilities.Results;

namespace BloggerProUI.Business.Interfaces;

public interface IAboutUsApiService
{
    Task<DataResult<List<AboutUsDto>>> GetAllAboutUsAsync();
    Task<DataResult<AboutUsDto>> GetAboutUsByIdAsync(Guid id);
    Task<Result> CreateAboutUsAsync(AboutUsCreateDto dto);
    Task<Result> UpdateAboutUsAsync(AboutUsUpdateDto dto);
    Task<Result> DeleteAboutUsAsync(Guid id);
    Task<Result> ToggleAboutUsStatusAsync(Guid id);
}