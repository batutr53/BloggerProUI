using BloggerProUI.Business.Interfaces;
using BloggerProUI.Models.AboutUs;
using BloggerProUI.Shared.Utilities.Results;
using System.Net.Http.Json;

namespace BloggerProUI.Business.Services;

public class AboutUsApiService : IAboutUsApiService
{
    private readonly HttpClient _httpClient;

    public AboutUsApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<DataResult<List<AboutUsDto>>> GetAllAboutUsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("aboutus");
            
            if (!response.IsSuccessStatusCode)
            {
                return new ErrorDataResult<List<AboutUsDto>>("Hakkımızda bilgileri alınırken bir hata oluştu.");
            }

            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(content))
            {
                return new ErrorDataResult<List<AboutUsDto>>("Boş yanıt alındı.");
            }

            var result = await response.Content.ReadFromJsonAsync<DataResult<List<AboutUsDto>>>();
            return result ?? new ErrorDataResult<List<AboutUsDto>>("Veriler işlenirken bir hata oluştu.");
        }
        catch (Exception ex)
        {
            return new ErrorDataResult<List<AboutUsDto>>($"Hata: {ex.Message}");
        }
    }

    public async Task<DataResult<AboutUsDto>> GetAboutUsByIdAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"aboutus/{id}");
            
            if (!response.IsSuccessStatusCode)
            {
                return new ErrorDataResult<AboutUsDto>("Hakkımızda bilgisi alınırken bir hata oluştu.");
            }

            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(content))
            {
                return new ErrorDataResult<AboutUsDto>("Boş yanıt alındı.");
            }

            var result = await response.Content.ReadFromJsonAsync<DataResult<AboutUsDto>>();
            return result ?? new ErrorDataResult<AboutUsDto>("Veriler işlenirken bir hata oluştu.");
        }
        catch (Exception ex)
        {
            return new ErrorDataResult<AboutUsDto>($"Hata: {ex.Message}");
        }
    }

    public async Task<Result> CreateAboutUsAsync(AboutUsCreateDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("aboutus", dto);
            
            if (!response.IsSuccessStatusCode)
            {
                return new ErrorResult("Hakkımızda bilgisi oluşturulurken bir hata oluştu.");
            }

            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(content))
            {
                return new ErrorResult("Boş yanıt alındı.");
            }

            var result = await response.Content.ReadFromJsonAsync<Result>();
            return result ?? new ErrorResult("İşlem tamamlanamadı.");
        }
        catch (Exception ex)
        {
            return new ErrorResult($"Hata: {ex.Message}");
        }
    }

    public async Task<Result> UpdateAboutUsAsync(AboutUsUpdateDto dto)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"aboutus/{dto.Id}", dto);
            
            if (!response.IsSuccessStatusCode)
            {
                return new ErrorResult("Hakkımızda bilgisi güncellenirken bir hata oluştu.");
            }

            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(content))
            {
                return new ErrorResult("Boş yanıt alındı.");
            }

            var result = await response.Content.ReadFromJsonAsync<Result>();
            return result ?? new ErrorResult("İşlem tamamlanamadı.");
        }
        catch (Exception ex)
        {
            return new ErrorResult($"Hata: {ex.Message}");
        }
    }

    public async Task<Result> DeleteAboutUsAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"aboutus/{id}");
            
            if (!response.IsSuccessStatusCode)
            {
                return new ErrorResult("Hakkımızda bilgisi silinirken bir hata oluştu.");
            }

            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(content))
            {
                return new ErrorResult("Boş yanıt alındı.");
            }

            var result = await response.Content.ReadFromJsonAsync<Result>();
            return result ?? new ErrorResult("İşlem tamamlanamadı.");
        }
        catch (Exception ex)
        {
            return new ErrorResult($"Hata: {ex.Message}");
        }
    }

    public async Task<Result> ToggleAboutUsStatusAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.PostAsync($"aboutus/{id}/toggle-status", null);
            
            if (!response.IsSuccessStatusCode)
            {
                return new ErrorResult("Durum güncellenirken bir hata oluştu.");
            }

            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(content))
            {
                return new ErrorResult("Boş yanıt alındı.");
            }

            var result = await response.Content.ReadFromJsonAsync<Result>();
            return result ?? new ErrorResult("İşlem tamamlanamadı.");
        }
        catch (Exception ex)
        {
            return new ErrorResult($"Hata: {ex.Message}");
        }
    }
}