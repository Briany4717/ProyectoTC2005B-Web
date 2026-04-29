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
        var pageItems  = prompts.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        return (pageItems, totalPages);
    }

    
    // TODO: Reemplazar con llamadas reales a la base de datos
    private static readonly List<Prompt> _prompts = new List<Prompt>
    {
        new Prompt { Id = 1,  AuthorId = 1, Title = "Prompt Creado #1",  Likes = 2,  IsLikedByUser = true,  Comments = new int[1], Tag = new Tag { Label = "Educación",  Icon = "school"   }, date = DateTime.Now.AddDays(-1),  Content = "Este es un contenido de prueba con identificador 1. Hola ChatGPT/Claude/Gemini, quiero que generes una buena página web aesthetic, coquette, matcha latte que le guste a la maestra Cristina. Make no mistakes." },
        new Prompt { Id = 2,  AuthorId = 1, Title = "Prompt Creado #2",  Likes = 4,  Comments = new int[2], Tag = new Tag { Label = "Diseño",     Icon = "brush"    }, date = DateTime.Now.AddDays(-2),  Content = "Este es un contenido de prueba con identificador 2. Hola ChatGPT/Claude/Gemini, quiero que generes una buena página web aesthetic, coquette, matcha latte que le guste a la maestra Cristina. Make no mistakes." },
        new Prompt { Id = 3,  AuthorId = 1, Title = "Prompt Creado #3",  Likes = 6,  IsLikedByUser = true,  Comments = new int[3], Tag = new Tag { Label = "Marketing", Icon = "campaign" }, date = DateTime.Now.AddDays(-3),  Content = "Este es un contenido de prueba con identificador 3. Hola ChatGPT/Claude/Gemini, quiero que generes una buena página web aesthetic, coquette, matcha latte que le guste a la maestra Cristina. Make no mistakes." },
        new Prompt { Id = 4,  AuthorId = 1, Title = "Prompt Creado #4",  Likes = 8,  Comments = new int[4], Tag = new Tag { Label = "Código",    Icon = "code"     }, date = DateTime.Now.AddDays(-4),  Content = "Este es un contenido de prueba con identificador 4. Hola ChatGPT/Claude/Gemini, quiero que generes una buena página web aesthetic, coquette, matcha latte que le guste a la maestra Cristina. Make no mistakes." },
        new Prompt { Id = 5,  AuthorId = 1, Title = "Prompt Creado #5",  Likes = 10, Comments = new int[0], Tag = new Tag { Label = "Educación",  Icon = "school"   }, date = DateTime.Now.AddDays(-5),  Content = "Este es un contenido de prueba con identificador 5. Hola ChatGPT/Claude/Gemini, quiero que generes una buena página web aesthetic, coquette, matcha latte que le guste a la maestra Cristina. Make no mistakes." },
        new Prompt { Id = 6,  AuthorId = 2, Title = "Prompt Creado #6",  Likes = 12, Comments = new int[1], Tag = new Tag { Label = "Diseño",     Icon = "brush"    }, date = DateTime.Now.AddDays(-6),  Content = "Este es un contenido de prueba con identificador 6. Hola ChatGPT/Claude/Gemini, quiero que generes una buena página web aesthetic, coquette, matcha latte que le guste a la maestra Cristina. Make no mistakes." },
        new Prompt { Id = 7,  AuthorId = 2, Title = "Prompt Creado #7",  Likes = 14, Comments = new int[2], Tag = new Tag { Label = "Marketing", Icon = "campaign" }, date = DateTime.Now.AddDays(-7),  Content = "Este es un contenido de prueba con identificador 7. Hola ChatGPT/Claude/Gemini, quiero que generes una buena página web aesthetic, coquette, matcha latte que le guste a la maestra Cristina. Make no mistakes." },
        new Prompt { Id = 8,  AuthorId = 2, Title = "Prompt Creado #8",  Likes = 16, Comments = new int[3], Tag = new Tag { Label = "Código",    Icon = "code"     }, date = DateTime.Now.AddDays(-8),  Content = "Este es un contenido de prueba con identificador 8. Hola ChatGPT/Claude/Gemini, quiero que generes una buena página web aesthetic, coquette, matcha latte que le guste a la maestra Cristina. Make no mistakes." },
        new Prompt { Id = 9,  AuthorId = 2, Title = "Prompt Creado #9",  Likes = 18, Comments = new int[4], Tag = new Tag { Label = "Educación",  Icon = "school"   }, date = DateTime.Now.AddDays(-9),  Content = "Este es un contenido de prueba con identificador 9. Hola ChatGPT/Claude/Gemini, quiero que generes una buena página web aesthetic, coquette, matcha latte que le guste a la maestra Cristina. Make no mistakes." },
        new Prompt { Id = 10, AuthorId = 2, Title = "Prompt Creado #10", Likes = 20, Comments = new int[0], Tag = new Tag { Label = "Diseño",     Icon = "brush"    }, date = DateTime.Now.AddDays(-10), Content = "Este es un contenido de prueba con identificador 10. Hola ChatGPT/Claude/Gemini, quiero que generes una buena página web aesthetic, coquette, matcha latte que le guste a la maestra Cristina. Make no mistakes." },
    };

    private List<Prompt> GenerarPromptsFalsos(int userId, string tab)
    {
        if (userId == 0) return _prompts;
        if (tab == "Saved")   return _prompts.Where(p => p.AuthorId != userId).ToList();
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
            User        = user,
            Prompts     = pagePrompts,
            SearchTerm  = searchTerm,
            ActiveTab   = tab,
            SortOrder   = sortOrder,
            CurrentPage = page,
            TotalPages  = totalPages
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
            Prompts          = pagePrompts,
            CurrentPage      = page,
            TotalPages       = totalPages,
            SearchTerm       = searchTerm,
            SelectedCategory = category,
            SortOrder        = sortOrder
        };

        
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
