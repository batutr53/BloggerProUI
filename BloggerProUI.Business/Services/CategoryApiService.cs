using BloggerProUI.Business.Interfaces;
using BloggerProUI.Models.Category;
using BloggerProUI.Shared.Utilities.Results;
using System.Net.Http.Json;

namespace BloggerProUI.Business.Services;

public class CategoryApiService : ICategoryApiService
{
    private readonly HttpClient _httpClient;

    public CategoryApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IDataResult<List<CategoryDto>>> GetAllAsync()
    {
        var response = await _httpClient.GetAsync("/api/Categories");
        return await response.Content.ReadFromJsonAsync<DataResult<List<CategoryDto>>>();
    }

    public async Task<IDataResult<CategoryDto>> GetByIdAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"/api/Categories/{id}");
        return await response.Content.ReadFromJsonAsync<DataResult<CategoryDto>>();
    }

    public async Task<IResult> CreateAsync(CategoryCreateDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/Categories", dto);
        return await response.Content.ReadFromJsonAsync<Result>();
    }

    public async Task<IResult> UpdateAsync(CategoryUpdateDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync("/api/Categories", dto);
        return await response.Content.ReadFromJsonAsync<Result>();
    }

    public async Task<IResult> DeleteAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"/api/Categories/{id}");
        return await response.Content.ReadFromJsonAsync<Result>();
    }
}
