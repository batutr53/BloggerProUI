using BloggerProUI.Business.Interfaces;
using BloggerProUI.Models.TeamMember;
using Microsoft.AspNetCore.Mvc;

namespace BloggerProUI.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class TeamMemberController : Controller
{
    private readonly ITeamMemberApiService _teamMemberApiService;

    public TeamMemberController(ITeamMemberApiService teamMemberApiService)
    {
        _teamMemberApiService = teamMemberApiService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _teamMemberApiService.GetAllTeamMembersAsync();
        
        if (result.Success)
        {
            return View(result.Data);
        }
        
        TempData["ErrorMessage"] = result.Message?.FirstOrDefault() ?? "Ekip üyeleri yüklenirken bir hata oluştu.";
        return View(new List<TeamMemberDto>());
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TeamMemberCreateDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _teamMemberApiService.CreateTeamMemberAsync(model);
        
        if (result.Success)
        {
            TempData["SuccessMessage"] = "Ekip üyesi başarıyla eklendi.";
            return RedirectToAction("Index");
        }
        
        TempData["ErrorMessage"] = result.Message?.FirstOrDefault() ?? "Ekleme işlemi sırasında bir hata oluştu.";
        return View(model);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var result = await _teamMemberApiService.GetTeamMemberByIdAsync(id);
        
        if (result.Success)
        {
            var updateDto = new TeamMemberUpdateDto
            {
                Id = result.Data.Id,
                Name = result.Data.Name,
                Position = result.Data.Position,
                Department = result.Data.Department,
                Bio = result.Data.Bio,
                ImageUrl = result.Data.ImageUrl,
                Email = result.Data.Email,
                LinkedInUrl = result.Data.LinkedInUrl,
                TwitterUrl = result.Data.TwitterUrl,
                IsActive = result.Data.IsActive,
                SortOrder = result.Data.SortOrder
            };
            
            return View(updateDto);
        }
        
        TempData["ErrorMessage"] = result.Message?.FirstOrDefault() ?? "Ekip üyesi bulunamadı.";
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(TeamMemberUpdateDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _teamMemberApiService.UpdateTeamMemberAsync(model);
        
        if (result.Success)
        {
            TempData["SuccessMessage"] = "Ekip üyesi başarıyla güncellendi.";
            return RedirectToAction("Index");
        }
        
        TempData["ErrorMessage"] = result.Message?.FirstOrDefault() ?? "Güncelleme işlemi sırasında bir hata oluştu.";
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _teamMemberApiService.DeleteTeamMemberAsync(id);
        
        if (result.Success)
        {
            return Json(new { success = true, message = "Ekip üyesi başarıyla silindi." });
        }
        
        return Json(new { success = false, message = result.Message?.FirstOrDefault() ?? "Silme işlemi sırasında bir hata oluştu." });
    }

    [HttpPost]
    public async Task<IActionResult> ToggleStatus(Guid id)
    {
        var result = await _teamMemberApiService.ToggleTeamMemberStatusAsync(id);
        
        if (result.Success)
        {
            return Json(new { success = true, message = "Durum başarıyla güncellendi." });
        }
        
        return Json(new { success = false, message = result.Message?.FirstOrDefault() ?? "İşlem sırasında bir hata oluştu." });
    }
}