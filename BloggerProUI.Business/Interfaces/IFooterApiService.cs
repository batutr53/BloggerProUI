using BloggerProUI.Models.Footer;
using BloggerProUI.Shared.Utilities.Results;

namespace BloggerProUI.Business.Interfaces;

public interface IFooterApiService
{
    Task<DataResult<List<FooterDto>>> GetAllFootersAsync();
    Task<DataResult<FooterDto>> GetFooterByIdAsync(Guid id);
    Task<DataResult<List<FooterDto>>> GetActiveFootersAsync();
    Task<DataResult<List<FooterDto>>> GetFootersByTypeAsync(string footerType);
    Task<Result> CreateFooterAsync(FooterCreateDto dto);
    Task<Result> UpdateFooterAsync(FooterUpdateDto dto);
    Task<Result> DeleteFooterAsync(Guid id);
    Task<Result> ToggleFooterStatusAsync(Guid id);
}