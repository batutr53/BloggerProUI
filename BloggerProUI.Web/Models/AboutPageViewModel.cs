using BloggerProUI.Models.AboutUs;
using BloggerProUI.Models.TeamMember;

namespace BloggerProUI.Web.Models;

public class AboutPageViewModel
{
    public AboutUsDto? AboutUs { get; set; }
    public List<TeamMemberDto> TeamMembers { get; set; } = new List<TeamMemberDto>();
}