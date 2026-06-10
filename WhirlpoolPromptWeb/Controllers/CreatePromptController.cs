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

    private void SetNavbarData()
    {
        ViewData["Coins"] = HttpContext.Session.GetInt32("Coins") ?? 0;
        ViewData["ProfilePhoto"] = HttpContext.Session.GetString("ProfileAddr") ?? "mario-bros.png";
    }

    [HttpGet]
    public async Task<IActionResult> CreatePrompt()
    {
        SetNavbarData();
        ViewData["Categories"] = await _createPromptService.GetCategoriesConId();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreatePrompt(CreatePromptModel model)
    {
        SetNavbarData();

        var categories = await _createPromptService.GetCategoriesConId();
        ViewData["Categories"] = categories; 

        if (!ModelState.IsValid) return View(model);

        var categoria = categories.FirstOrDefault(c => c.Nombre == model.Category);
        if (categoria == null)
        {
            ModelState.AddModelError(nameof(model.Category), "La categoría seleccionada no es válida.");
            return View(model);
        }

        int authorId = HttpContext.Session.GetInt32("UserId") ?? 0;

        var resultado = await _createPromptService.InsertarPrompt(
            model.Title.Trim(),
            model.Content.Trim(),
            categoria.IdCategoria,
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

        return RedirectToAction("Index", "Library");
    }
}