using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WhirlpoolPromptWeb.Models;

namespace WhirlpoolPromptWeb.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        User user = new User();
        user.Name = "Mario";
        user.LastName = "Bros";
        user.ProfilePhoto = ProfilePhoto.mario;
        user.Coins = 247;
        ViewData["Coins"] = user.Coins;

        switch (user.ProfilePhoto)
        {
            case ProfilePhoto.mario:
                ViewData["ProfilePhoto"] = "mario-bros.png";
                break;
            default:
                ViewData["ProfilePhoto"] = "mario-bros.png";
                break;

        }
        return View(user);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
