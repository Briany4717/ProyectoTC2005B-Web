using Microsoft.AspNetCore.Mvc;

namespace WhirlpoolPromptWeb.Controllers;

public class CreatePromptController : Controller
{
    public IActionResult CreatePrompt()
    {
        return View();
    }
}