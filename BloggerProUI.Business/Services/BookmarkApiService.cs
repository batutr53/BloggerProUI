using BloggerProUI.Business.Interfaces;
using BloggerProUI.Models.Bookmark;
using BloggerProUI.Shared.Utilities.Results;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace BloggerProUI.Business.Services
{
    public class BookmarkApiService : IBookmarkApiService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public BookmarkApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<IResult> AddBookmarkAsync(BookmarkCreateDto bookmarkCreateDto)
        {
            try
            {
                var json = JsonSerializer.Serialize(bookmarkCreateDto, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/bookmark/add", content);

                if (response.IsSuccessStatusCode)
                {
                    return new SuccessResult("Yazı favorilere eklendi");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new ErrorResult($"Hata: {response.StatusCode} - {errorContent}");
                }
            }
            catch (HttpRequestException)
            {
                return new ErrorResult("Sunucuya bağlanırken bir hata oluştu");
            }
            catch (Exception ex)
            {
                return new ErrorResult($"Beklenmeyen bir hata oluştu: {ex.Message}");
            }
        }

        public async Task<IResult> RemoveBookmarkAsync(Guid postId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/api/bookmark/remove/{postId}");

                if (response.IsSuccessStatusCode)
                {
                    return new SuccessResult("Yazı favorilerden kaldırıldı");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new ErrorResult($"Hata: {response.StatusCode} - {errorContent}");
                }
            }
            catch (HttpRequestException)
            {
                return new ErrorResult("Sunucuya bağlanırken bir hata oluştu");
            }
            catch (Exception ex)
            {
                return new ErrorResult($"Beklenmeyen bir hata oluştu: {ex.Message}");
            }
        }

        public async Task<IDataResult<bool>> IsBookmarkedAsync(Guid postId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/bookmark/is-bookmarked/{postId}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<IDataResult<bool>>(jsonResponse, _jsonOptions);
                    
                    return result?.Data == true 
                        ? new SuccessDataResult<bool>(true, "Favorilerde")
                        : new SuccessDataResult<bool>(false, "Favorilerde değil");
                }
                else
                {
                    return new ErrorDataResult<bool>(false, "Favori durumu kontrol edilemedi");
                }
            }
            catch (HttpRequestException)
            {
                return new ErrorDataResult<bool>(false, "Sunucuya bağlanırken bir hata oluştu");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<bool>(false, $"Beklenmeyen bir hata oluştu: {ex.Message}");
            }
        }

        public async Task<IDataResult<List<BookmarkListDto>>> GetMyBookmarksAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/bookmark/my-bookmarks");

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<IDataResult<List<BookmarkListDto>>>(jsonResponse, _jsonOptions);
                    
                    return result?.Data != null 
                        ? new SuccessDataResult<List<BookmarkListDto>>(result.Data, "Favoriler getirildi")
                        : new SuccessDataResult<List<BookmarkListDto>>(new List<BookmarkListDto>(), "Favori bulunamadı");
                }
                else
                {
                    return new ErrorDataResult<List<BookmarkListDto>>(new List<BookmarkListDto>(), "Favoriler getirilemedi");
                }
            }
            catch (HttpRequestException)
            {
                return new ErrorDataResult<List<BookmarkListDto>>(new List<BookmarkListDto>(), "Sunucuya bağlanırken bir hata oluştu");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<BookmarkListDto>>(new List<BookmarkListDto>(), $"Beklenmeyen bir hata oluştu: {ex.Message}");
            }
        }

        public async Task<IDataResult<int>> GetBookmarkCountAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/bookmark/count");

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<IDataResult<int>>(jsonResponse, _jsonOptions);
                    
                    return result?.Data != null 
                        ? new SuccessDataResult<int>(result.Data, "Favori sayısı getirildi")
                        : new SuccessDataResult<int>(0, "Favori sayısı bulunamadı");
                }
                else
                {
                    return new ErrorDataResult<int>(0, "Favori sayısı getirilemedi");
                }
            }
            catch (HttpRequestException)
            {
                return new ErrorDataResult<int>(0, "Sunucuya bağlanırken bir hata oluştu");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<int>(0, $"Beklenmeyen bir hata oluştu: {ex.Message}");
            }
        }
    }
}