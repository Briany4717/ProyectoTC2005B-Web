using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WhirlpoolPromptWeb.Filters;
using WhirlpoolPromptWeb.Models;
using WhirlpoolPromptWeb.Services;

namespace WhirlpoolPromptWeb.Controllers;


[RequireSession]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ILeaderboardService _leaderboardService;

    private string GetProfileAddr(ProfilePhoto profile)
    {
        switch (profile)
        {
            case ProfilePhoto.mario:
                return "mario-bros.png";
            default:
                return "mario-bros.png";
        }
    }

    private bool isSessionStarted()
    {
        return (HttpContext.Session.GetInt32("UserId") != null) && HttpContext.Session.GetInt32("Coins") != null;
    }

    private User getUserFromId(int id)
    {
        User user = new User();

        /* TODO: Obtener el user de la base de datos */
        user.Id = id;
        user.Name = "Mario";
        user.LastName = "Bros";
        user.Coins = 257;
        user.Birthday = DateTime.Now;
        user.ProfilePhoto = ProfilePhoto.mario;
        user.LocalRanking = 5;
        user.NationalRanking = 142;

        return user;
    }

    private User getUserFromSession()
    {
        User user = new User();

        user.Id = (int)HttpContext.Session.GetInt32("UserId");
        user.Name = HttpContext.Session.GetString("Name");
        user.Coins = (int)HttpContext.Session.GetInt32("Coins");

        return user;
    }

    public HomeController(ILogger<HomeController> logger, ILeaderboardService leaderboardService)
    {
        _logger = logger;
        _leaderboardService = leaderboardService;
    }

    public IActionResult Index()
    {

        var user = getUserFromSession();

        ViewData["Coins"] = user.Coins;
        ViewData["ProfilePhoto"] = GetProfileAddr(user.ProfilePhoto);

        return View(user);
    }

    public async Task<IActionResult> Leaderboard(string league = "Nacional", int page = 1, string searchTerm = null)
    {
        User user = getUserFromSession();
        ViewData["Coins"] = user.Coins;
        ViewData["ProfilePhoto"] = HttpContext.Session.GetString("ProfileAddr");

        const int pageSize = 5;

        var apiEntries = await _leaderboardService.GetLeaderboard(league);
        var allEntries = apiEntries.Select((e, i) => new LeaderboardEntry
        {
            Rank          = i + 1,
            Name          = e.NombreUsuario,
            Avatar        = "mario-bros.png",
            Coins         = e.MonedasUsuario,
            Prompts       = e.NumeroPrompts,
            Useful        = e.VotosAcumulados,
            IsCurrentUser = e.IdUsuario == user.Id,
        }).ToList();

        var myPosition   = allEntries.FirstOrDefault(e => e.IsCurrentUser);
        var tableEntries = allEntries.Where(e => !e.IsCurrentUser).ToList();

        if (!string.IsNullOrWhiteSpace(searchTerm))
            tableEntries = tableEntries
                .Where(e => e.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();

        int totalPages = (int)Math.Ceiling(tableEntries.Count / (double)pageSize);
        var pageEntries = tableEntries.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        var viewModel = new LeaderboardViewModel
        {
            Entries      = pageEntries,
            MyPosition   = myPosition,
            ActiveLeague = league,
            CurrentPage  = page,
            TotalPages   = totalPages,
            PageSize     = pageSize,
            SearchTerm   = searchTerm
        };

        return View(viewModel);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


}
