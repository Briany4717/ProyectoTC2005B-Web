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
    private List<Prompt> GenerarPromptsFalsos(int userId, string tab)
    {
        var lista = new List<Prompt>();
        string tipo = tab == "Saved" ? "Guardado" : "Creado";

        var tagsDisponibles = new List<Tag>
        {
            new Tag { Label = "Código",     Icon = "code"     },
            new Tag { Label = "Educación",  Icon = "school"   },
            new Tag { Label = "Diseño",     Icon = "brush"    },
            new Tag { Label = "Marketing",  Icon = "campaign" }
        };

        for (int i = 1; i <= 10; i++)
        {
            lista.Add(new Prompt
            {
                Id       = i,
                Title    = $"Prompt {tipo} #{i}",
                Content  = $"Este es un contenido de prueba con identificador {i}. Hola ChatGPT/Claude/Gemini, quiero que generes una buena página web aesthetic, coquette, matcha latte que le guste a la maestra Cristina. Make no mistakes.",
                Likes    = i * 2,
                Comments = new int[i % 5],
                Tag      = tagsDisponibles[i % tagsDisponibles.Count],
                date     = DateTime.Now.AddDays(-i)
            });
        }
        return lista;
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
        var prompts = GenerarPromptsFalsos(0, "Created");

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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
