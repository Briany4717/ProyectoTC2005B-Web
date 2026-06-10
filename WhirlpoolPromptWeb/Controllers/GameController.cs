using Microsoft.AspNetCore.Mvc;
using WhirlpoolPromptWeb.Filters;

namespace WhirlpoolPromptWeb.Controllers;

[RequireSession]
public class GameController : Controller
{
    private void SetNavbarData()
    {
        ViewData["Coins"] = HttpContext.Session.GetInt32("Coins") ?? 0;
        ViewData["ProfilePhoto"] = HttpContext.Session.GetString("ProfileAddr") ?? "mario-bros.png";
    }

    [HttpGet]
    public IActionResult Index()
    {
        SetNavbarData();
        return View();
    }
}
