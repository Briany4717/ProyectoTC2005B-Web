using Microsoft.AspNetCore.Mvc;
using WhirlpoolPromptWeb.Controllers;
using WhirlpoolPromptWeb.Filters;
using WhirlpoolPromptWeb.Models;
using WhirlpoolPromptWeb.Services;

namespace WhirlpoolPromptWeb.Controllers;

[RequireSession]
public class CreatePromptController : Controller
{
    private const int CoinsReward = 150;
    private readonly ICreatePromptService _createPromptService;

    public CreatePromptController(ICreatePromptService createPromptService)
    {
        _createPromptService = createPromptService;
    }

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
    public async Task<IActionResult> CreatePrompt(CreatePromptModel model)
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

        // Mapear la categoría a su id numérico para la API
        int idCategoria = model.Category switch
        {
            "Código"     => 1,
            "Educación"  => 2,
            "Diseño"     => 3,
            "Marketing"  => 4,
            _            => 0
        };

        var resultado = await _createPromptService.InsertarPrompt(
            model.Title.Trim(),
            model.Content.Trim(),
            idCategoria,
            authorId
        );

        if (resultado == null)
        {
            ModelState.AddModelError(string.Empty, "No se pudo guardar el prompt. Intenta de nuevo.");
            return View(model);
        }

        int currentCoins = HttpContext.Session.GetInt32("Coins") ?? 0;
        HttpContext.Session.SetInt32("Coins", currentCoins + CoinsReward);

        TempData["SuccessMessage"] = $"¡Prompt publicado correctamente! Has ganado {CoinsReward} monedas.";

        return RedirectToAction("Library", "Home");
    }
}