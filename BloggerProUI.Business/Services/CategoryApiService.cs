using BloggerProUI.Business.Interfaces;
using BloggerProUI.Models.Category;
using BloggerProUI.Shared.Utilities.Results;
using System.Net.Http.Json;
using System.Text.Json;

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
        
        if (!response.IsSuccessStatusCode)
        {
            return new ErrorDataResult<List<CategoryDto>>($"API Error: {response.StatusCode}");
        }

        var content = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(content))
        {
            return new ErrorDataResult<List<CategoryDto>>("Empty response from API");
        }

        try
        {
            return await response.Content.ReadFromJsonAsync<DataResult<List<CategoryDto>>>();
        }
        catch (JsonException ex)
        {
            return new ErrorDataResult<List<CategoryDto>>($"JSON parsing error: {ex.Message}");
        }
    }

    public async Task<IDataResult<CategoryDto>> GetByIdAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"/api/Categories/{id}");
        
        if (!response.IsSuccessStatusCode)
        {
            return new ErrorDataResult<CategoryDto>($"API Error: {response.StatusCode}");
        }

        var content = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(content))
        {
            return new ErrorDataResult<CategoryDto>("Empty response from API");
        }

        try
        {
            return await response.Content.ReadFromJsonAsync<DataResult<CategoryDto>>();
        }
        catch (JsonException ex)
        {
            return new ErrorDataResult<CategoryDto>($"JSON parsing error: {ex.Message}");
        }
    }

    public async Task<IResult> CreateAsync(CategoryCreateDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/Categories", dto);
        
        if (!response.IsSuccessStatusCode)
        {
            return new ErrorResult($"API Error: {response.StatusCode}");
        }

        var content = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(content))
        {
            return new ErrorResult("Empty response from API");
        }

        try
        {
            return await response.Content.ReadFromJsonAsync<Result>();
        }
        catch (JsonException ex)
        {
            return new ErrorResult($"JSON parsing error: {ex.Message}");
        }
    }

    public async Task<IResult> UpdateAsync(CategoryUpdateDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync("/api/Categories", dto);
        
        if (!response.IsSuccessStatusCode)
        {
            return new ErrorResult($"API Error: {response.StatusCode}");
        }

        var content = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(content))
        {
            return new ErrorResult("Empty response from API");
        }

        try
        {
            return await response.Content.ReadFromJsonAsync<Result>();
        }
        catch (JsonException ex)
        {
            return new ErrorResult($"JSON parsing error: {ex.Message}");
        }
    }

    public async Task<IResult> DeleteAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"/api/Categories/{id}");
        
        if (!response.IsSuccessStatusCode)
        {
            return new ErrorResult($"API Error: {response.StatusCode}");
        }

        var content = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(content))
        {
            return new ErrorResult("Empty response from API");
        }

        try
        {
            return await response.Content.ReadFromJsonAsync<Result>();
        }
        catch (JsonException ex)
        {
            return new ErrorResult($"JSON parsing error: {ex.Message}");
        }
    }
}
