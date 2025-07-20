using BloggerProUI.Business.Interfaces;
using BloggerProUI.Models.AboutUs;
using BloggerProUI.Web.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloggerProUI.Web.Areas.Admin.Controllers;

[Area("Admin")]
[AdminAuthorize]
public class AboutUsController : Controller
{
    private readonly IAboutUsApiService _aboutUsApiService;

    public AboutUsController(IAboutUsApiService aboutUsApiService)
    {
        _aboutUsApiService = aboutUsApiService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _aboutUsApiService.GetAllAboutUsAsync();
        
        if (result.Success)
        {
            return View(result.Data);
        }
        
        TempData["ErrorMessage"] = result.Message?.FirstOrDefault() ?? "Hakkımızda bilgileri yüklenirken bir hata oluştu.";
        return View(new List<AboutUsDto>());
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AboutUsCreateDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _aboutUsApiService.CreateAboutUsAsync(model);
        
        if (result.Success)
        {
            TempData["SuccessMessage"] = "Hakkımızda bilgisi başarıyla oluşturuldu.";
            return RedirectToAction("Index");
        }
        
        TempData["ErrorMessage"] = result.Message?.FirstOrDefault() ?? "Oluşturma işlemi sırasında bir hata oluştu.";
        return View(model);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var result = await _aboutUsApiService.GetAboutUsByIdAsync(id);
        
        if (result.Success)
        {
            var updateDto = new AboutUsUpdateDto
            {
                Id = result.Data.Id,
                Title = result.Data.Title,
                Content = result.Data.Content,
                Mission = result.Data.Mission,
                Vision = result.Data.Vision,
                IsActive = result.Data.IsActive,
                SortOrder = result.Data.SortOrder
            };
            
            return View(updateDto);
        }
        
        TempData["ErrorMessage"] = result.Message?.FirstOrDefault() ?? "Hakkımızda bilgisi bulunamadı.";
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(AboutUsUpdateDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _aboutUsApiService.UpdateAboutUsAsync(model);
        
        if (result.Success)
        {
            TempData["SuccessMessage"] = "Hakkımızda bilgisi başarıyla güncellendi.";
            return RedirectToAction("Index");
        }
        
        TempData["ErrorMessage"] = result.Message?.FirstOrDefault() ?? "Güncelleme işlemi sırasında bir hata oluştu.";
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _aboutUsApiService.DeleteAboutUsAsync(id);
        
        if (result.Success)
        {
            return Json(new { success = true, message = "Hakkımızda bilgisi başarıyla silindi." });
        }
        
        return Json(new { success = false, message = result.Message?.FirstOrDefault() ?? "Silme işlemi sırasında bir hata oluştu." });
    }

    [HttpPost]
    public async Task<IActionResult> ToggleStatus(Guid id)
    {
        var result = await _aboutUsApiService.ToggleAboutUsStatusAsync(id);
        
        if (result.Success)
        {
            return Json(new { success = true, message = "Durum başarıyla güncellendi." });
        }
        
        return Json(new { success = false, message = result.Message?.FirstOrDefault() ?? "İşlem sırasında bir hata oluştu." });
    }
}