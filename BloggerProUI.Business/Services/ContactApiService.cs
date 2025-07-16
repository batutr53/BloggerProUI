using BloggerProUI.Business.Interfaces;
using BloggerProUI.Models.Contact;
using BloggerProUI.Models.Pagination;
using BloggerProUI.Shared.Utilities.Results;
using System.Net.Http.Json;
using System.Text.Json;

namespace BloggerProUI.Business.Services;

public class ContactApiService : IContactApiService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public ContactApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<Result> CreateContactAsync(ContactCreateDto contactCreateDto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("contact", contactCreateDto);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                {
                    return new ErrorResult("Sunucudan boş yanıt alındı.");
                }

                var result = JsonSerializer.Deserialize<Result>(content, _jsonOptions);
                return result ?? new ErrorResult("Yanıt işlenirken hata oluştu.");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(errorContent))
            {
                try
                {
                    var errorResult = JsonSerializer.Deserialize<Result>(errorContent, _jsonOptions);
                    return errorResult ?? new ErrorResult("İletişim mesajı gönderilirken hata oluştu.");
                }
                catch
                {
                    return new ErrorResult("İletişim mesajı gönderilirken hata oluştu.");
                }
            }

            return new ErrorResult("İletişim mesajı gönderilirken hata oluştu.");
        }
        catch (HttpRequestException)
        {
            return new ErrorResult("Sunucu ile bağlantı kurulamadı.");
        }
        catch (JsonException)
        {
            return new ErrorResult("Sunucudan gelen yanıt işlenirken hata oluştu.");
        }
        catch (Exception)
        {
            return new ErrorResult("İletişim mesajı gönderilirken beklenmeyen bir hata oluştu.");
        }
    }

    public async Task<DataResult<PaginatedResultDto<ContactListDto>>> GetAllContactsAsync(int page = 1, int pageSize = 10, bool? isReplied = null)
    {
        try
        {
            var query = $"contact?page={page}&pageSize={pageSize}";
            if (isReplied.HasValue)
            {
                query += $"&isReplied={isReplied.Value}";
            }

            var response = await _httpClient.GetAsync(query);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                {
                    return new ErrorDataResult<PaginatedResultDto<ContactListDto>>("Sunucudan boş yanıt alındı.");
                }

                var result = JsonSerializer.Deserialize<DataResult<PaginatedResultDto<ContactListDto>>>(content, _jsonOptions);
                return result ?? new ErrorDataResult<PaginatedResultDto<ContactListDto>>("Yanıt işlenirken hata oluştu.");
            }

            return new ErrorDataResult<PaginatedResultDto<ContactListDto>>("İletişim mesajları alınırken hata oluştu.");
        }
        catch (HttpRequestException)
        {
            return new ErrorDataResult<PaginatedResultDto<ContactListDto>>("Sunucu ile bağlantı kurulamadı.");
        }
        catch (JsonException)
        {
            return new ErrorDataResult<PaginatedResultDto<ContactListDto>>("Sunucudan gelen yanıt işlenirken hata oluştu.");
        }
        catch (Exception)
        {
            return new ErrorDataResult<PaginatedResultDto<ContactListDto>>("İletişim mesajları alınırken beklenmeyen bir hata oluştu.");
        }
    }

    public async Task<DataResult<ContactListDto>> GetContactByIdAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"contact/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                {
                    return new ErrorDataResult<ContactListDto>("Sunucudan boş yanıt alındı.");
                }

                var result = JsonSerializer.Deserialize<DataResult<ContactListDto>>(content, _jsonOptions);
                return result ?? new ErrorDataResult<ContactListDto>("Yanıt işlenirken hata oluştu.");
            }

            return new ErrorDataResult<ContactListDto>("İletişim mesajı alınırken hata oluştu.");
        }
        catch (HttpRequestException)
        {
            return new ErrorDataResult<ContactListDto>("Sunucu ile bağlantı kurulamadı.");
        }
        catch (JsonException)
        {
            return new ErrorDataResult<ContactListDto>("Sunucudan gelen yanıt işlenirken hata oluştu.");
        }
        catch (Exception)
        {
            return new ErrorDataResult<ContactListDto>("İletişim mesajı alınırken beklenmeyen bir hata oluştu.");
        }
    }

    public async Task<Result> ReplyToContactAsync(Guid id, ContactReplyDto contactReplyDto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"contact/{id}/reply", contactReplyDto);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                {
                    return new ErrorResult("Sunucudan boş yanıt alındı.");
                }

                var result = JsonSerializer.Deserialize<Result>(content, _jsonOptions);
                return result ?? new ErrorResult("Yanıt işlenirken hata oluştu.");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(errorContent))
            {
                try
                {
                    var errorResult = JsonSerializer.Deserialize<Result>(errorContent, _jsonOptions);
                    return errorResult ?? new ErrorResult("Yanıt gönderilirken hata oluştu.");
                }
                catch
                {
                    return new ErrorResult("Yanıt gönderilirken hata oluştu.");
                }
            }

            return new ErrorResult("Yanıt gönderilirken hata oluştu.");
        }
        catch (HttpRequestException)
        {
            return new ErrorResult("Sunucu ile bağlantı kurulamadı.");
        }
        catch (JsonException)
        {
            return new ErrorResult("Sunucudan gelen yanıt işlenirken hata oluştu.");
        }
        catch (Exception)
        {
            return new ErrorResult("Yanıt gönderilirken beklenmeyen bir hata oluştu.");
        }
    }

    public async Task<Result> DeleteContactAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"contact/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                {
                    return new ErrorResult("Sunucudan boş yanıt alındı.");
                }

                var result = JsonSerializer.Deserialize<Result>(content, _jsonOptions);
                return result ?? new ErrorResult("Yanıt işlenirken hata oluştu.");
            }

            return new ErrorResult("İletişim mesajı silinirken hata oluştu.");
        }
        catch (HttpRequestException)
        {
            return new ErrorResult("Sunucu ile bağlantı kurulamadı.");
        }
        catch (JsonException)
        {
            return new ErrorResult("Sunucudan gelen yanıt işlenirken hata oluştu.");
        }
        catch (Exception)
        {
            return new ErrorResult("İletişim mesajı silinirken beklenmeyen bir hata oluştu.");
        }
    }

    public async Task<Result> MarkAsRepliedAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.PatchAsync($"contact/{id}/mark-replied", null);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                {
                    return new ErrorResult("Sunucudan boş yanıt alındı.");
                }

                var result = JsonSerializer.Deserialize<Result>(content, _jsonOptions);
                return result ?? new ErrorResult("Yanıt işlenirken hata oluştu.");
            }

            return new ErrorResult("Mesaj işaretlenirken hata oluştu.");
        }
        catch (HttpRequestException)
        {
            return new ErrorResult("Sunucu ile bağlantı kurulamadı.");
        }
        catch (JsonException)
        {
            return new ErrorResult("Sunucudan gelen yanıt işlenirken hata oluştu.");
        }
        catch (Exception)
        {
            return new ErrorResult("Mesaj işaretlenirken beklenmeyen bir hata oluştu.");
        }
    }
}