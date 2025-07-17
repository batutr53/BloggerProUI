using BloggerProUI.Business.Interfaces;
using BloggerProUI.Models.TeamMember;
using BloggerProUI.Shared.Utilities.Results;
using System.Net.Http.Json;

namespace BloggerProUI.Business.Services;

public class TeamMemberApiService : ITeamMemberApiService
{
    private readonly HttpClient _httpClient;

    public TeamMemberApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<DataResult<List<TeamMemberDto>>> GetAllTeamMembersAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("TeamMember");
            
            if (!response.IsSuccessStatusCode)
            {
                return new ErrorDataResult<List<TeamMemberDto>>("Ekip üyeleri alınırken bir hata oluştu.");
            }

            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(content))
            {
                return new ErrorDataResult<List<TeamMemberDto>>("Boş yanıt alındı.");
            }

            var result = await response.Content.ReadFromJsonAsync<DataResult<List<TeamMemberDto>>>();
            return result ?? new ErrorDataResult<List<TeamMemberDto>>("Veriler işlenirken bir hata oluştu.");
        }
        catch (Exception ex)
        {
            return new ErrorDataResult<List<TeamMemberDto>>($"Hata: {ex.Message}");
        }
    }

    public async Task<DataResult<TeamMemberDto>> GetTeamMemberByIdAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"TeamMember/{id}");
            
            if (!response.IsSuccessStatusCode)
            {
                return new ErrorDataResult<TeamMemberDto>("Ekip üyesi alınırken bir hata oluştu.");
            }

            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(content))
            {
                return new ErrorDataResult<TeamMemberDto>("Boş yanıt alındı.");
            }

            var result = await response.Content.ReadFromJsonAsync<DataResult<TeamMemberDto>>();
            return result ?? new ErrorDataResult<TeamMemberDto>("Veriler işlenirken bir hata oluştu.");
        }
        catch (Exception ex)
        {
            return new ErrorDataResult<TeamMemberDto>($"Hata: {ex.Message}");
        }
    }

    public async Task<Result> CreateTeamMemberAsync(TeamMemberCreateDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("TeamMember", dto);
            
            if (!response.IsSuccessStatusCode)
            {
                return new ErrorResult("Ekip üyesi oluşturulurken bir hata oluştu.");
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

    public async Task<Result> UpdateTeamMemberAsync(TeamMemberUpdateDto dto)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"TeamMember/{dto.Id}", dto);
            
            if (!response.IsSuccessStatusCode)
            {
                return new ErrorResult("Ekip üyesi güncellenirken bir hata oluştu.");
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

    public async Task<Result> DeleteTeamMemberAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"TeamMember/{id}");
            
            if (!response.IsSuccessStatusCode)
            {
                return new ErrorResult("Ekip üyesi silinirken bir hata oluştu.");
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

    public async Task<Result> ToggleTeamMemberStatusAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.PostAsync($"TeamMember/{id}/toggle-status", null);
            
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