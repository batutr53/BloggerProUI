using BloggerProUI.Business.Interfaces;
using BloggerProUI.Models.Bookmark;
using BloggerProUI.Shared.Utilities.Results;
using System.Net.Http;
using System.Net.Http.Json;
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

                if (!response.IsSuccessStatusCode)
                {
                    return new ErrorDataResult<bool>(false, "Favori durumu kontrol edilemedi");
                }

                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                {
                    return new ErrorDataResult<bool>(false, "Boş yanıt alındı.");
                }

                var result = await response.Content.ReadFromJsonAsync<DataResult<bool>>();
                return result ?? new ErrorDataResult<bool>(false, "Veriler işlenirken bir hata oluştu.");
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

                if (!response.IsSuccessStatusCode)
                {
                    return new ErrorDataResult<List<BookmarkListDto>>(new List<BookmarkListDto>(), "Favoriler getirilemedi");
                }

                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                {
                    return new ErrorDataResult<List<BookmarkListDto>>(new List<BookmarkListDto>(), "Boş yanıt alındı.");
                }

                var result = await response.Content.ReadFromJsonAsync<DataResult<List<BookmarkListDto>>>();
                return result ?? new ErrorDataResult<List<BookmarkListDto>>(new List<BookmarkListDto>(), "Veriler işlenirken bir hata oluştu.");
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

                if (!response.IsSuccessStatusCode)
                {
                    return new ErrorDataResult<int>(0, "Favori sayısı getirilemedi");
                }

                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                {
                    return new ErrorDataResult<int>(0, "Boş yanıt alındı.");
                }

                var result = await response.Content.ReadFromJsonAsync<DataResult<int>>();
                return result ?? new ErrorDataResult<int>(0, "Veriler işlenirken bir hata oluştu.");
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