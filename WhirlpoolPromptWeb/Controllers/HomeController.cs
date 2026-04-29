using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WhirlpoolPromptWeb.Models;

namespace WhirlpoolPromptWeb.Controllers;

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
        user.LastName = HttpContext.Session.GetString("Lastname");
        user.Coins = (int)HttpContext.Session.GetInt32("Coins");
        user.LocalRanking = HttpContext.Session.GetInt32("LocalRanking") ?? 0;
        user.NationalRanking = HttpContext.Session.GetInt32("NationalRanking") ?? 0;

        return user;
    }

    private List<Prompt> ApplySearch(List<Prompt> prompts, string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm)) return prompts;
        return prompts
            .Where(p => p.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                        p.Content.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    private List<Prompt> ApplySort(List<Prompt> prompts, string sortOrder)
    {
        switch (sortOrder)
        {
            case "alpha":
                return prompts.OrderBy(p => p.Title).ToList();
            case "popularity":
                return prompts.OrderByDescending(p => p.Likes).ToList();
            default:
                return prompts.OrderByDescending(p => p.date).ToList();
        }
    }

    private (List<Prompt> page, int totalPages) ApplyPagination(List<Prompt> prompts, int page, int pageSize)
    {
        int totalPages = (int)Math.Ceiling(prompts.Count / (double)pageSize);
        var pageItems = prompts.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        return (pageItems, totalPages);
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

    // TODO: Reemplazar con llamadas reales a la base de datos
    private static List<Prompt> _prompts = new List<Prompt>
    {
        new Prompt { Id = 1,  AuthorId = 1, Title = "Aprender React desde cero",  Likes = 120, IsLikedByUser = true,  Comments = new int[1], Tag = new Tag { Label = "Educación",  Icon = "school"   }, date = DateTime.Now.AddDays(-1),  Content = "Actúa como un desarrollador experto en React y explícame los conceptos básicos de los Hooks, dando ejemplos prácticos de useState y useEffect." },
        new Prompt { Id = 2,  AuthorId = 1, Title = "Generador de paletas de colores", Likes = 45,  Comments = new int[2], Tag = new Tag { Label = "Diseño",     Icon = "brush"    }, date = DateTime.Now.AddDays(-2),  Content = "Eres un diseñador UI/UX experimentado. Genera 5 paletas de colores modernas y accesibles para una aplicación de finanzas, utilizando códigos HEX y justificando tu elección." },
        new Prompt { Id = 3,  AuthorId = 1, Title = "Campaña para redes sociales",  Likes = 60,  IsLikedByUser = true,  Comments = new int[3], Tag = new Tag { Label = "Marketing", Icon = "campaign" }, date = DateTime.Now.AddDays(-3),  Content = "Diseña una campaña de marketing de 7 días para Instagram enfocada en el lanzamiento de un nuevo producto de café ecológico, detallando el tipo de contenido diario." },
        new Prompt { Id = 4,  AuthorId = 1, Title = "Refactorización de código Python", Likes = 85,  Comments = new int[4], Tag = new Tag { Label = "Código",    Icon = "code"     }, date = DateTime.Now.AddDays(-4),  Content = "Optimiza mi siguiente script de Python asegurando las mejores prácticas, manejando excepciones correctamente y mejorando la legibilidad. Dame comentarios explicando cada cambio." },
        new Prompt { Id = 5,  AuthorId = 1, Title = "Plan de estudio para el TOEFL", Likes = 150, Comments = new int[0], Tag = new Tag { Label = "Educación",  Icon = "school"   }, date = DateTime.Now.AddDays(-5),  Content = "Crea un plan de estudio detallado de 4 semanas para prepararme para la certificación de inglés TOEFL. Incluye recursos recomendados para lectura, escucha, escritura y conversación." },
        new Prompt { Id = 6,  AuthorId = 2, Title = "Diseño de base de datos SQL",  Likes = 112, Comments = new int[1], Tag = new Tag { Label = "Código",     Icon = "code"     }, date = DateTime.Now.AddDays(-6),  Content = "Diseña la estructura de una base de datos relacional para una plataforma de e-commerce. Incluye las tablas de usuarios, productos, y órdenes, especificando llaves foráneas y tipos de datos." },
        new Prompt { Id = 7,  AuthorId = 2, Title = "Estrategia SEO local",  Likes = 74,  Comments = new int[2], Tag = new Tag { Label = "Marketing", Icon = "campaign" }, date = DateTime.Now.AddDays(-7),  Content = "Desarrolla una estrategia integral de SEO local para una floristería recién inaugurada en el centro de la ciudad, enfocada en mejorar la visibilidad en resultados de Google Maps." },
        new Prompt { Id = 8,  AuthorId = 2, Title = "Script automatizado para backups", Likes = 96, Comments = new int[3], Tag = new Tag { Label = "Código",    Icon = "code"     }, date = DateTime.Now.AddDays(-8),  Content = "Escribe un script en Bash o PowerShell de unos cuantos comandos que me permita automatizar el respaldo diario de los archivos de una carpeta específica y borrarlos si superan cierta antigüedad." },
        new Prompt { Id = 9,  AuthorId = 2, Title = "Tutor de matemáticas",  Likes = 210, Comments = new int[4], Tag = new Tag { Label = "Educación",  Icon = "school"   }, date = DateTime.Now.AddDays(-9),  Content = "Actúa como un profesor de cálculo comprensivo. Quiero que me expliques el concepto de las derivadas y su uso en la vida real de manera sencilla y atractiva para un adolescente." },
        new Prompt { Id = 10, AuthorId = 2, Title = "Guía de estilos CSS", Likes = 105, Comments = new int[0], Tag = new Tag { Label = "Diseño",     Icon = "brush"    }, date = DateTime.Now.AddDays(-10), Content = "Crea una guía concisa de convenciones de nombrado para BEM en CSS. Escribe ejemplos claros de bloques, elementos y modificadores aplicados a una tarjeta de producto." },
    };

    private List<Prompt> GenerarPromptsFalsos(int userId, string tab)
    {
        if (userId == 0) return _prompts;
        if (tab == "Saved") return _prompts.Where(p => p.AuthorId != userId).ToList();
        return _prompts.Where(p => p.AuthorId == userId).ToList();
    }

    private void ApplyToggleLike(int promptId)
    {
        var prompt = _prompts.FirstOrDefault(p => p.Id == promptId);
        if (prompt == null) return;
        prompt.IsLikedByUser = !prompt.IsLikedByUser;
        prompt.Likes += prompt.IsLikedByUser ? 1 : -1;
    }

    [HttpPost]
    public IActionResult ToggleLikeLibrary(int promptId, string searchTerm, string category, string sortOrder, int page)
    {
        ApplyToggleLike(promptId);
        return RedirectToAction("Library", "Home",
            new { searchTerm, category, sortOrder, page },
            fragment: $"prompt-{promptId}");
    }

    [HttpPost]
    public IActionResult ToggleLikeProfile(int promptId, string searchTerm, string tab, string sortOrder, int page)
    {
        ApplyToggleLike(promptId);
        return RedirectToAction("Profile", "Home",
            new { searchTerm, tab, sortOrder, page },
            fragment: $"prompt-{promptId}");
    }

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        User user = getUserFromId(1);

        HttpContext.Session.SetInt32("UserId", user.Id);
        HttpContext.Session.SetString("Name", user.Name);
        HttpContext.Session.SetString("Lastname", user.LastName);
        HttpContext.Session.SetString("PrifileAddr", GetProfileAddr(user.ProfilePhoto));
        HttpContext.Session.SetInt32("Coins", user.Coins);
        HttpContext.Session.SetInt32("LocalRanking", user.LocalRanking);
        HttpContext.Session.SetInt32("NationalRanking", user.NationalRanking);

        ViewData["Coins"] = user.Coins;
        ViewData["ProfilePhoto"] = GetProfileAddr(user.ProfilePhoto);

        return View(user);
    }

    public IActionResult Leaderboard(string league = "Nacional", int page = 1, string searchTerm = null)
    {
        if (!isSessionStarted())
            return RedirectToAction("Index");

        User user = getUserFromSession();
        ViewData["Coins"] = user.Coins;
        ViewData["ProfilePhoto"] = HttpContext.Session.GetString("PrifileAddr");

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



    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Profile(string searchTerm = null, string tab = "Created", string sortOrder = "date", int page = 1)
    {
        if (!isSessionStarted())
            return RedirectToAction("Index");

        User user = getUserFromSession();
        ViewData["Coins"] = user.Coins;
        ViewData["ProfilePhoto"] = HttpContext.Session.GetString("PrifileAddr");

        var prompts = GenerarPromptsFalsos(user.Id, tab);
        prompts = ApplySearch(prompts, searchTerm);
        prompts = ApplySort(prompts, sortOrder);
        (var pagePrompts, int totalPages) = ApplyPagination(prompts, page, pageSize: 4);

        var viewModel = new ProfileViewModel
        {
            User = user,
            Prompts = pagePrompts,
            SearchTerm = searchTerm,
            ActiveTab = tab,
            SortOrder = sortOrder,
            CurrentPage = page,
            TotalPages = totalPages
        };

        return View(viewModel);
    }

    public IActionResult Library(string searchTerm = null, string category = null, string sortOrder = "date", int page = 1)
    {
        User user = getUserFromSession();
        var prompts = GenerarPromptsFalsos(0, "");

        if (!string.IsNullOrEmpty(category))
            prompts = prompts.Where(p => p.Tag?.Label == category).ToList();

        prompts = ApplySearch(prompts, searchTerm);
        prompts = ApplySort(prompts, sortOrder);
        (var pagePrompts, int totalPages) = ApplyPagination(prompts, page, pageSize: 6);

        var viewModel = new LibraryViewModel
        {
            Prompts = pagePrompts,
            CurrentPage = page,
            TotalPages = totalPages,
            SearchTerm = searchTerm,
            SelectedCategory = category,
            SortOrder = sortOrder
        };

        ViewData["UserId"] = user.Id;
        ViewData["Coins"] = user.Coins;
        ViewData["ProfilePhoto"] = GetProfileAddr(user.ProfilePhoto);

        return View(viewModel);
    }

    public IActionResult Comments(int promptId)
    {
        return View(promptId);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
