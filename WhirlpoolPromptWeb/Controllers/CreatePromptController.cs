using Microsoft.AspNetCore.Mvc;
using WhirlpoolPromptWeb.Controllers;
using WhirlpoolPromptWeb.Filters;
using WhirlpoolPromptWeb.Models;

namespace WhirlpoolPromptWeb.Controllers;

[RequireSession]
public class CreatePromptController : Controller
{
    private const int CoinsReward = 150;

    private Tag GetTagFromCategory(string category)
    {
        switch (category)
        {
            case "Código":
                return new Tag { Label = "Código", Icon = "code" };
            case "Educación":
                return new Tag { Label = "Educación", Icon = "school" };
            case "Diseño":
                return new Tag { Label = "Diseño", Icon = "brush" };
            case "Marketing":
                return new Tag { Label = "Marketing", Icon = "campaign" };
            default:
                return null;
        }
    }

    private void SetNavbarData()
    {
        ViewData["Coins"] = HttpContext.Session.GetInt32("Coins") ?? 0;
        ViewData["ProfilePhoto"] = HttpContext.Session.GetString("ProfileAddr") ?? "mario-bros.png";
    }

    [HttpGet]
    public IActionResult CreatePrompt()
    {
        SetNavbarData();
        return View();
    }

    [HttpPost]
    public IActionResult CreatePrompt(CreatePromptModel model)
    {
        SetNavbarData();

        if (!ModelState.IsValid) return View(model);

        var tag = GetTagFromCategory(model.Category);
        if (tag == null)
        {
            ModelState.AddModelError(nameof(model.Category), "La categoría seleccionada no es válida.");
            return View(model);
        }

        int authorId = HttpContext.Session.GetInt32("UserId") ?? 0;
        int newId = HomeController._prompts.Count > 0
            ? HomeController._prompts.Max(p => p.Id) + 1
            : 1;

        var newPrompt = new Prompt
        {
            Id = newId,
            AuthorId = authorId,
            Title = model.Title.Trim(),
            Content = model.Content.Trim(),
            Tag = tag,
            date = DateTime.Now,
            Likes = 0,
            IsLikedByUser = false,
            Comments = new int[0]
        };

        HomeController._prompts.Add(newPrompt);

        int currentCoins = HttpContext.Session.GetInt32("Coins") ?? 0;
        HttpContext.Session.SetInt32("Coins", currentCoins + CoinsReward);

        TempData["SuccessMessage"] = $"¡Prompt publicado correctamente! Has ganado {CoinsReward} monedas.";

        return RedirectToAction("Library", "Home");
    }
}