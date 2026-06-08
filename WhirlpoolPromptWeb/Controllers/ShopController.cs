using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WhirlpoolPromptWeb.Filters;
using WhirlpoolPromptWeb.Models;
using WhirlpoolPromptWeb.Services;

namespace WhirlpoolPromptWeb.Controllers;

[RequireSession]
public class ShopController : Controller
{
    private readonly IShopService _shopService;

    public ShopController(IShopService shopService)
    {
        _shopService = shopService;
    }

    private void SetNavbarData()
    {
        ViewData["Coins"] = HttpContext.Session.GetInt32("Coins") ?? 0;
        ViewData["ProfilePhoto"] = HttpContext.Session.GetString("ProfileAddr") ?? "mario-bros.png";
    }

    public async Task<IActionResult> Shop()
    {
        SetNavbarData();

        int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
        var products = await _shopService.GetProductsAsync(userId);

        var viewModel = new ShopViewModel { Products = products };

        if (TempData["PurchaseResult"] is string json)
            viewModel.LastPurchaseResult = JsonSerializer.Deserialize<PurchaseResult>(json);

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Purchase(int productId)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        var coins = HttpContext.Session.GetInt32("Coins");

        PurchaseResult result;
        if (userId == null || coins == null)
        {
            result = new PurchaseResult { Success = false, Message = "Sesión no válida.", NewCoinBalance = 0 };
        }
        else
        {
            result = await _shopService.PurchaseProductAsync(productId, userId.Value, coins.Value);
            if (result.Success)
                HttpContext.Session.SetInt32("Coins", result.NewCoinBalance);
        }

        TempData["PurchaseResult"] = JsonSerializer.Serialize(result);
        return RedirectToAction("Shop");
    }
}
