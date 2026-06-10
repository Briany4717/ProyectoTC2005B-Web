using Microsoft.AspNetCore.Mvc;
using WhirlpoolPromptWeb.Filters;
using WhirlpoolPromptWeb.Models;
using WhirlpoolPromptWeb.Services;

namespace WhirlpoolPromptWeb.Controllers;

[RequireSession]
public class PromptController : Controller
{
    private readonly IProfileService _profileService;

    public PromptController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    private void SetNavbarData()
    {
        ViewData["Coins"] = HttpContext.Session.GetInt32("Coins") ?? 0;
        ViewData["ProfilePhoto"] = HttpContext.Session.GetString("ProfileAddr") ?? "mario-bros.png";
    }

    public async Task<IActionResult> Detail(int promptId, string? returnUrl = null)
    {
        SetNavbarData();

        int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
        string userName = HttpContext.Session.GetString("Name") ?? "";

        var prompt = await _profileService.GetPromptDetailAsync(promptId, userId, userName);
        if (prompt == null) return NotFound();

        return View(new PromptDetailViewModel { Prompt = prompt, ReturnUrl = returnUrl });
    }

    [HttpPost]
    public async Task<IActionResult> ToggleLike(int promptId, string? returnUrl)
    {
        int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
        await _profileService.ToggleLike(promptId, userId);
        return RedirectToAction("Detail", new { promptId, returnUrl });
    }

    [HttpPost]
    public async Task<IActionResult> ToggleSave(int promptId, bool isSaved, string? returnUrl)
    {
        int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
        await _profileService.ToggleSave(promptId, userId, isSaved);
        return RedirectToAction("Detail", new { promptId, returnUrl });
    }
}
