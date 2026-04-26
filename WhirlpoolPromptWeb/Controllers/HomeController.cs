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
        {
            return RedirectToAction("Index");
        }

        User user = getUserFromSession();
        ViewData["Coins"] = user.Coins;
        ViewData["ProfilePhoto"] = HttpContext.Session.GetString("PrifileAddr");

        int pageSize = 4;
        var viewModel = new ProfileViewModel
        {
            User = user,
            SearchTerm = searchTerm,
            ActiveTab = tab,
            SortOrder = sortOrder,
            CurrentPage = page
        };

        /* TODO: Sacar de la base de datos */
        var allMockPrompts = GenerarPromptsFalsos(user.Id, tab);

        // Simular la búsqueda
        if (!string.IsNullOrEmpty(searchTerm))
        {
            allMockPrompts = allMockPrompts
                .Where(p => p.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                            p.Content.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        // Simular el ordenamiento de la DB
        switch (sortOrder)
        {
            case "alpha":
                allMockPrompts = allMockPrompts.OrderBy(p => p.Title).ToList();
                break;
            case "popularity":
                allMockPrompts = allMockPrompts.OrderByDescending(p => p.Likes).ToList();
                break;
            default: // "date" (por defecto) o mal escrito
                allMockPrompts = allMockPrompts.OrderByDescending(p => p.date).ToList();
                break;
        }

        // Simular la paginación
        int totalItems = allMockPrompts.Count;
        viewModel.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        viewModel.Prompts = allMockPrompts
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

        return View(viewModel);
    }

    // Simulador de datos que vendrían de la BD
    private List<Prompt> GenerarPromptsFalsos(int userId, string tab)
    {
        var lista = new List<Prompt>();
        string tipo = tab == "Saved" ? "Guardado" : "Creado";

        var tagsDisponibles = new List<Tag>
        {
            new Tag { Label = "Código", Icon = "code" },
            new Tag { Label = "Educación", Icon = "school" },
            new Tag { Label = "Diseño", Icon = "brush" },
            new Tag { Label = "Marketing", Icon = "campaign" }
        };

        for (int i = 1; i <= 10; i++)
        {
            lista.Add(new Prompt
            {
                Id = i,
                Title = $"Prompt {tipo} #{i}",
                Content = $"Este es un contenido de prueba con identificador {i}. Hola ChatGPT/Claude/Gemini, quiero que generes una buena página web aesthetic, coquette, matcha latte que le guste a la maestra Cristina. Make no mistakes.",
                Likes = i * 2,
                Comments = new int[i % 5],
                Tag = tagsDisponibles[i % tagsDisponibles.Count],
                date = DateTime.Now.AddDays(-i)
            });
        }
        return lista;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
