using BloggerProUI.Business.Interfaces;
using BloggerProUI.Models.Auth;
using BloggerProUI.Shared.Utilities.Results;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;

namespace BloggerProUI.Business.Services
{
    public class AuthApiService : IAuthApiService
    {
        private readonly HttpClient _httpClient;

        public AuthApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IDataResult<string>> LoginAsync(LoginDto dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("auth/login", dto);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<DataResult<AccessTokenDto>>(responseContent);

                    if (result != null && result.Data != null && result.Success)
                    {
                        return new SuccessDataResult<string>(result.Data.Token, "Giriş başarılı.");
                    }

                    return new ErrorDataResult<string>(
                        result?.Message != null ? string.Join(" | ", result.Message) : "Bilinmeyen API hatası"
                    );
                }

                // Hatalı durum - response status code 400/401/500
                var errorResult = JsonConvert.DeserializeObject<ErrorResult>(responseContent);
                if (errorResult == null || response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    return new ErrorDataResult<string>("Sunucu hatası: veri boş döndü.");
                }

                return new ErrorDataResult<string>(
                    errorResult.Message != null ? string.Join(" | ", errorResult.Message) : "Giriş başarısız.",
                    errorResult.HttpStatusCode,
                    errorResult.StatusCode
                );
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(DateTime.Now.ToString("yyyy.MM.dd:HH:mm:ss") + " | " + ex.Message);
                return new ErrorDataResult<string>("İstisna oluştu: " + ex.Message);
            }
        }

        public async Task<IResult> RegisterAsync(RegisterDto dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("auth/register", dto);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<Result>(responseContent);
                    
                    if (result != null && result.Success)
                    {
                        return new SuccessResult("Kayıt başarılı.");
                    }

                    return new ErrorResult(
                        result?.Message != null ? string.Join(" | ", result.Message) : "Bilinmeyen API hatası"
                    );
                }

                // Hatalı durum - response status code 400/401/500
                var errorResult = JsonConvert.DeserializeObject<ErrorResult>(responseContent);
                if (errorResult == null || response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    return new ErrorResult("Sunucu hatası: veri boş döndü.");
                }

                return new ErrorResult(
                    errorResult.Message != null ? string.Join(" | ", errorResult.Message) : "Kayıt başarısız.",
                    errorResult.HttpStatusCode,
                    errorResult.StatusCode
                );
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(DateTime.Now.ToString("yyyy.MM.dd:HH:mm:ss") + " | " + ex.Message);
                return new ErrorResult("İstisna oluştu: " + ex.Message);
            }
        }
    }
}