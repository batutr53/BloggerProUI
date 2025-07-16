using BloggerProUI.Models.TeamMember;
using BloggerProUI.Shared.Utilities.Results;

namespace BloggerProUI.Business.Interfaces;

public interface ITeamMemberApiService
{
    Task<DataResult<List<TeamMemberDto>>> GetAllTeamMembersAsync();
    Task<DataResult<TeamMemberDto>> GetTeamMemberByIdAsync(Guid id);
    Task<Result> CreateTeamMemberAsync(TeamMemberCreateDto dto);
    Task<Result> UpdateTeamMemberAsync(TeamMemberUpdateDto dto);
    Task<Result> DeleteTeamMemberAsync(Guid id);
    Task<Result> ToggleTeamMemberStatusAsync(Guid id);
}