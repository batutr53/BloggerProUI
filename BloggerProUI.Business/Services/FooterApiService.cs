using BloggerProUI.Business.Interfaces;
using BloggerProUI.Models.Footer;
using BloggerProUI.Shared.Utilities.Results;
using System.Net.Http.Json;

namespace BloggerProUI.Business.Services;

public class FooterApiService : IFooterApiService
{
    private readonly HttpClient _httpClient;

    public FooterApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<DataResult<List<FooterDto>>> GetAllFootersAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("Footer");
            
            if (!response.IsSuccessStatusCode)
            {
                return new ErrorDataResult<List<FooterDto>>("Footer bilgileri alınırken bir hata oluştu.");
            }

            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(content))
            {
                return new ErrorDataResult<List<FooterDto>>("Boş yanıt alındı.");
            }

            var result = await response.Content.ReadFromJsonAsync<DataResult<List<FooterDto>>>();
            return result ?? new ErrorDataResult<List<FooterDto>>("Veriler işlenirken bir hata oluştu.");
        }
        catch (Exception ex)
        {
            return new ErrorDataResult<List<FooterDto>>($"Hata: {ex.Message}");
        }
    }

    public async Task<DataResult<FooterDto>> GetFooterByIdAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"Footer/{id}");
            
            if (!response.IsSuccessStatusCode)
            {
                return new ErrorDataResult<FooterDto>("Footer bilgisi alınırken bir hata oluştu.");
            }

            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(content))
            {
                return new ErrorDataResult<FooterDto>("Boş yanıt alındı.");
            }

            var result = await response.Content.ReadFromJsonAsync<DataResult<FooterDto>>();
            return result ?? new ErrorDataResult<FooterDto>("Veriler işlenirken bir hata oluştu.");
        }
        catch (Exception ex)
        {
            return new ErrorDataResult<FooterDto>($"Hata: {ex.Message}");
        }
    }

    public async Task<DataResult<List<FooterDto>>> GetActiveFootersAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("Footer/active");
            
            if (!response.IsSuccessStatusCode)
            {
                return new ErrorDataResult<List<FooterDto>>("Aktif footer bilgileri alınırken bir hata oluştu.");
            }

            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(content))
            {
                return new ErrorDataResult<List<FooterDto>>("Boş yanıt alındı.");
            }

            var result = await response.Content.ReadFromJsonAsync<DataResult<List<FooterDto>>>();
            return result ?? new ErrorDataResult<List<FooterDto>>("Veriler işlenirken bir hata oluştu.");
        }
        catch (Exception ex)
        {
            return new ErrorDataResult<List<FooterDto>>($"Hata: {ex.Message}");
        }
    }

    public async Task<DataResult<List<FooterDto>>> GetFootersByTypeAsync(string footerType)
    {
        try
        {
            var response = await _httpClient.GetAsync($"Footer/type/{footerType}");
            
            if (!response.IsSuccessStatusCode)
            {
                return new ErrorDataResult<List<FooterDto>>("Footer bilgileri alınırken bir hata oluştu.");
            }

            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(content))
            {
                return new ErrorDataResult<List<FooterDto>>("Boş yanıt alındı.");
            }

            var result = await response.Content.ReadFromJsonAsync<DataResult<List<FooterDto>>>();
            return result ?? new ErrorDataResult<List<FooterDto>>("Veriler işlenirken bir hata oluştu.");
        }
        catch (Exception ex)
        {
            return new ErrorDataResult<List<FooterDto>>($"Hata: {ex.Message}");
        }
    }

    public async Task<Result> CreateFooterAsync(FooterCreateDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("Footer", dto);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(errorContent))
                {
                    try
                    {
                        var errorResult = System.Text.Json.JsonSerializer.Deserialize<dynamic>(errorContent);
                        return new ErrorResult($"API Hatası ({response.StatusCode}): {errorContent}");
                    }
                    catch
                    {
                        return new ErrorResult($"Footer oluşturulurken bir hata oluştu ({response.StatusCode}): {errorContent}");
                    }
                }
                return new ErrorResult($"Footer oluşturulurken bir hata oluştu ({response.StatusCode}).");
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

    public async Task<Result> UpdateFooterAsync(FooterUpdateDto dto)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"Footer/{dto.Id}", dto);
            
            if (!response.IsSuccessStatusCode)
            {
                return new ErrorResult("Footer güncellenirken bir hata oluştu.");
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

    public async Task<Result> DeleteFooterAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"Footer/{id}");
            
            if (!response.IsSuccessStatusCode)
            {
                return new ErrorResult("Footer silinirken bir hata oluştu.");
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

    public async Task<Result> ToggleFooterStatusAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.PostAsync($"Footer/{id}/toggle-status", null);
            
            if (!response.IsSuccessStatusCode)
            {
                return new ErrorResult("Footer durumu güncellenirken bir hata oluştu.");
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