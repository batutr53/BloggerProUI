using BloggerProUI.Business.Interfaces;
using BloggerProUI.Models.Tag;
using BloggerProUI.Shared.Utilities.Results;
using System.Net.Http.Json;

namespace BloggerProUI.Business.Services
{
    public class TagApiService : ITagApiService
    {
        private readonly HttpClient _httpClient;

        public TagApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IDataResult<List<TagDto>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("/api/Tags");
            if (!response.IsSuccessStatusCode)
                return new ErrorDataResult<List<TagDto>>("Etiketler getirilemedi.");

            var result = await response.Content.ReadFromJsonAsync<DataResult<List<TagDto>>>();
            return result ?? new ErrorDataResult<List<TagDto>>("Boş veri geldi.");
        }

        public async Task<IDataResult<TagDto>> GetByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/Tags/{id}");
            if (!response.IsSuccessStatusCode)
                return new ErrorDataResult<TagDto>("Etiket bulunamadı.");

            var result = await response.Content.ReadFromJsonAsync<DataResult<TagDto>>();
            return result ?? new ErrorDataResult<TagDto>("Boş veri geldi.");
        }

        public async Task<IResult> CreateAsync(TagCreateDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Tags", dto);
            if (!response.IsSuccessStatusCode)
                return new ErrorResult("Etiket oluşturulamadı.");

            return new SuccessResult("Etiket başarıyla oluşturuldu.");
        }

        public async Task<IResult> UpdateAsync(TagUpdateDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync("/api/Tags", dto);
            if (!response.IsSuccessStatusCode)
                return new ErrorResult("Etiket güncellenemedi.");

            return new SuccessResult("Etiket başarıyla güncellendi.");
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Tags/{id}");
            if (!response.IsSuccessStatusCode)
                return new ErrorResult("Etiket silinemedi.");

            return new SuccessResult("Etiket silindi.");
        }
    }

}
