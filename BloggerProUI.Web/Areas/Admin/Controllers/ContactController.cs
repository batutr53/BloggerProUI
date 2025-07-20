using BloggerProUI.Business.Interfaces;
using BloggerProUI.Models.Contact;
using BloggerProUI.Web.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloggerProUI.Web.Areas.Admin.Controllers;

[Area("Admin")]
[AdminAuthorize]
public class ContactController : Controller
{
    private readonly IContactApiService _contactApiService;

    public ContactController(IContactApiService contactApiService)
    {
        _contactApiService = contactApiService;
    }

    public async Task<IActionResult> Index(int page = 1, int pageSize = 10, bool? isReplied = null)
    {
        try
        {
            var result = await _contactApiService.GetAllContactsAsync(page, pageSize, isReplied);
            
            if (result.Success)
            {
                ViewBag.CurrentPage = page;
                ViewBag.PageSize = pageSize;
                ViewBag.IsReplied = isReplied;
                
                // Debug: Check if data is null or empty
                if (result.Data == null)
                {
                    TempData["ErrorMessage"] = "API'den null data alındı.";
                    return View(new BloggerProUI.Models.Pagination.PaginatedResultDto<ContactListDto>());
                }
                
                return View(result.Data);
            }
            
            TempData["ErrorMessage"] = "API Hatası: " + (result.Message?.FirstOrDefault() ?? "İletişim mesajları yüklenirken bir hata oluştu.");
            return View(new BloggerProUI.Models.Pagination.PaginatedResultDto<ContactListDto>());
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "Exception: " + ex.Message + " | " + ex.StackTrace;
            return View(new BloggerProUI.Models.Pagination.PaginatedResultDto<ContactListDto>());
        }
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var result = await _contactApiService.GetContactByIdAsync(id);
        
        if (result.Success)
        {
            return View(result.Data);
        }
        
        TempData["ErrorMessage"] = result.Message?.FirstOrDefault() ?? "İletişim mesajı bulunamadı.";
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Reply(Guid id)
    {
        var result = await _contactApiService.GetContactByIdAsync(id);
        
        if (result.Success)
        {
            ViewBag.Contact = result.Data;
            return View(new ContactReplyDto());
        }
        
        TempData["ErrorMessage"] = result.Message?.FirstOrDefault() ?? "İletişim mesajı bulunamadı.";
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reply(Guid id, ContactReplyDto model)
    {
        if (!ModelState.IsValid)
        {
            var contactResult = await _contactApiService.GetContactByIdAsync(id);
            if (contactResult.Success)
            {
                ViewBag.Contact = contactResult.Data;
            }
            return View(model);
        }

        var result = await _contactApiService.ReplyToContactAsync(id, model);
        
        if (result.Success)
        {
            TempData["SuccessMessage"] = "Yanıt başarıyla gönderildi.";
            return RedirectToAction("Index");
        }
        
        TempData["ErrorMessage"] = result.Message?.FirstOrDefault() ?? "Yanıt gönderilirken bir hata oluştu.";
        
        var contactResult2 = await _contactApiService.GetContactByIdAsync(id);
        if (contactResult2.Success)
        {
            ViewBag.Contact = contactResult2.Data;
        }
        
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> MarkAsReplied(Guid id)
    {
        var result = await _contactApiService.MarkAsRepliedAsync(id);
        
        if (result.Success)
        {
            return Json(new { success = true, message = "Mesaj yanıtlandı olarak işaretlendi." });
        }
        
        return Json(new { success = false, message = result.Message?.FirstOrDefault() ?? "İşlem sırasında bir hata oluştu." });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _contactApiService.DeleteContactAsync(id);
        
        if (result.Success)
        {
            return Json(new { success = true, message = "İletişim mesajı başarıyla silindi." });
        }
        
        return Json(new { success = false, message = result.Message?.FirstOrDefault() ?? "Silme işlemi sırasında bir hata oluştu." });
    }
}