using Microsoft.AspNetCore.Mvc;
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

        var products = await _shopService.GetProductsAsync();
        var viewModel = new ShopViewModel { Products = products };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Purchase(int productId)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        var coins = HttpContext.Session.GetInt32("Coins");

        if (userId == null || coins == null)
            return Json(new PurchaseResult { Success = false, Message = "Sesión no válida.", NewCoinBalance = 0 });

        var result = await _shopService.PurchaseProductAsync(productId, userId.Value, coins.Value);

        if (result.Success)
            HttpContext.Session.SetInt32("Coins", result.NewCoinBalance);

        return Json(result);
    }
}
