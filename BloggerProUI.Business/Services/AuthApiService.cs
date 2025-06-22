using BloggerProUI.Business.Interfaces;
using BloggerProUI.Models.Auth;
using BloggerProUI.Shared.Utilities.Results;
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
            var response = await _httpClient.PostAsJsonAsync("/auth/login", dto);
            if (!response.IsSuccessStatusCode)
                return new ErrorDataResult<string>("Giriş başarısız.");

            var token = await response.Content.ReadAsStringAsync();
            return new SuccessDataResult<string>(token);
        }
    }

}
