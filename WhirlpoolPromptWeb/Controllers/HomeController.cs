using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WhirlpoolPromptWeb.Filters;
using WhirlpoolPromptWeb.Models;

namespace WhirlpoolPromptWeb.Controllers;


[RequireSession]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

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

    // TODO: Reemplazar con llamada real a la base de datos
    private List<LeaderboardEntry> GenerarLeaderboardFalso(string league)
    {
        var nacional = new List<LeaderboardEntry>
        {
            new LeaderboardEntry { Rank = 1,  Name = "Princesa P.", Avatar = "mario-bros.png", Coins = 550, Prompts = 53,  Useful = 400 },
            new LeaderboardEntry { Rank = 2,  Name = "Luigi B.",    Avatar = "mario-bros.png", Coins = 450, Prompts = 23,  Useful = 310 },
            new LeaderboardEntry { Rank = 3,  Name = "Bowser",      Avatar = "mario-bros.png", Coins = 230, Prompts = 81,  Useful = 560 },
            new LeaderboardEntry { Rank = 4,  Name = "Yoshi",       Avatar = "mario-bros.png", Coins = 180, Prompts = 90,  Useful = 673 },
            new LeaderboardEntry { Rank = 5,  Name = "Toad",        Avatar = "mario-bros.png", Coins = 106, Prompts = 12,  Useful = 822 },
            new LeaderboardEntry { Rank = 6,  Name = "Waluigi",     Avatar = "mario-bros.png", Coins = 98,  Prompts = 44,  Useful = 210 },
            new LeaderboardEntry { Rank = 7,  Name = "Wario",       Avatar = "mario-bros.png", Coins = 87,  Prompts = 31,  Useful = 190 },
            new LeaderboardEntry { Rank = 8,  Name = "Donkey K.",   Avatar = "mario-bros.png", Coins = 75,  Prompts = 20,  Useful = 155 },
            new LeaderboardEntry { Rank = 9,  Name = "Peach",       Avatar = "mario-bros.png", Coins = 64,  Prompts = 17,  Useful = 130 },
            new LeaderboardEntry { Rank = 10, Name = "Rosalina",    Avatar = "mario-bros.png", Coins = 60,  Prompts = 15,  Useful = 120 },
            new LeaderboardEntry { Rank = 11, Name = "Birdo",       Avatar = "mario-bros.png", Coins = 55,  Prompts = 11,  Useful = 110 },
            new LeaderboardEntry { Rank = 12, Name = "Koopa T.",    Avatar = "mario-bros.png", Coins = 50,  Prompts = 9,   Useful = 90  },
            new LeaderboardEntry { Rank = 13, Name = "Boo",         Avatar = "mario-bros.png", Coins = 44,  Prompts = 7,   Useful = 80  },
            new LeaderboardEntry { Rank = 14, Name = "Shy Guy",     Avatar = "mario-bros.png", Coins = 38,  Prompts = 6,   Useful = 70  },
            new LeaderboardEntry { Rank = 15, Name = "Kamek",       Avatar = "mario-bros.png", Coins = 30,  Prompts = 5,   Useful = 60  },
            new LeaderboardEntry { Rank = 16, Name = "Lakitu",      Avatar = "mario-bros.png", Coins = 25,  Prompts = 4,   Useful = 50  },
            new LeaderboardEntry { Rank = 17, Name = "Mario Bros",  Avatar = "mario-bros.png", Coins = 50,  Prompts = 10,  Useful = 40,  IsCurrentUser = true },
            new LeaderboardEntry { Rank = 18, Name = "Hammer Bro.", Avatar = "mario-bros.png", Coins = 18,  Prompts = 3,   Useful = 30  },
            new LeaderboardEntry { Rank = 19, Name = "Chain Chomp", Avatar = "mario-bros.png", Coins = 12,  Prompts = 2,   Useful = 20  },
            new LeaderboardEntry { Rank = 20, Name = "Goomba",      Avatar = "mario-bros.png", Coins = 5,   Prompts = 1,   Useful = 10  },
        };

        var local = new List<LeaderboardEntry>
        {
            new LeaderboardEntry { Rank = 1, Name = "Toad",       Avatar = "mario-bros.png", Coins = 822, Prompts = 12, Useful = 150 },
            new LeaderboardEntry { Rank = 2, Name = "Yoshi",      Avatar = "mario-bros.png", Coins = 673, Prompts = 90, Useful = 120 },
            new LeaderboardEntry { Rank = 3, Name = "Mario Bros", Avatar = "mario-bros.png", Coins = 257, Prompts = 10, Useful = 40,  IsCurrentUser = true },
            new LeaderboardEntry { Rank = 4, Name = "Bowser",     Avatar = "mario-bros.png", Coins = 230, Prompts = 81, Useful = 100 },
            new LeaderboardEntry { Rank = 5, Name = "Luigi B.",   Avatar = "mario-bros.png", Coins = 100, Prompts = 5,  Useful = 30  },
            new LeaderboardEntry { Rank = 6, Name = "Kamek",      Avatar = "mario-bros.png", Coins = 80,  Prompts = 3,  Useful = 25  },
            new LeaderboardEntry { Rank = 7, Name = "Boo",        Avatar = "mario-bros.png", Coins = 60,  Prompts = 2,  Useful = 18  },
        };

        return league == "Local" ? local : nacional;
    }

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {

        var user = getUserFromSession();

        ViewData["Coins"] = user.Coins;
        ViewData["ProfilePhoto"] = GetProfileAddr(user.ProfilePhoto);

        return View(user);
    }

    public IActionResult Leaderboard(string league = "Nacional", int page = 1, string searchTerm = null)
    {
        User user = getUserFromSession();
        ViewData["Coins"] = user.Coins;
        ViewData["ProfilePhoto"] = HttpContext.Session.GetString("ProfileAddr");

        const int pageSize = 5;
        var allEntries = GenerarLeaderboardFalso(league);
        var myPosition = allEntries.FirstOrDefault(e => e.IsCurrentUser);
        var tableEntries = allEntries.Where(e => !e.IsCurrentUser).ToList();

        if (!string.IsNullOrWhiteSpace(searchTerm))
            tableEntries = tableEntries
                .Where(e => e.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();

        int totalPages = (int)Math.Ceiling(tableEntries.Count / (double)pageSize);
        var pageEntries = tableEntries.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        var viewModel = new LeaderboardViewModel
        {
            Entries = pageEntries,
            MyPosition = myPosition,
            ActiveLeague = league,
            CurrentPage = page,
            TotalPages = totalPages,
            PageSize = pageSize,
            SearchTerm = searchTerm
        };

        return View(viewModel);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


}
