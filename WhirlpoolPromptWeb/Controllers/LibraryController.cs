using Microsoft.AspNetCore.Mvc;
using WhirlpoolPromptWeb.Filters;
using WhirlpoolPromptWeb.Models;
using WhirlpoolPromptWeb.Services;

namespace WhirlpoolPromptWeb.Controllers;

[RequireSession]
public class LibraryController : Controller
{
    private readonly ILogger<LibraryController> _logger;
    private readonly ILibraryServices _libraryServices;
    private readonly IProfileService _profileService;
    public static List<Prompt> _prompts = new List<Prompt>();

    public LibraryController(ILogger<LibraryController> logger, ILibraryServices libraryServices, IProfileService profileService)
    {
        _logger = logger;
        _libraryServices = libraryServices;
        _profileService = profileService;
    }

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

    private User getUserFromSession()
    {
        User user = new User();
        user.Id = (int)HttpContext.Session.GetInt32("UserId");
        user.Name = HttpContext.Session.GetString("Name");
        user.Coins = (int)HttpContext.Session.GetInt32("Coins");
        // For profile photo address, we might need more session data or a default
        string profileType = HttpContext.Session.GetString("ProfilePhoto");
        if (Enum.TryParse(profileType, out ProfilePhoto photo))
        {
            user.ProfilePhoto = photo;
        }
        else
        {
            user.ProfilePhoto = ProfilePhoto.mario;
        }

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

    [HttpPost]
    public async Task<IActionResult> ToggleLike(int promptId, string searchTerm, string category, string sortOrder, int page)
    {
        User user = getUserFromSession();
        await _profileService.ToggleLike(promptId, user.Id);
        
        return RedirectToAction("Index", "Library",
            new { searchTerm, category, sortOrder, page },
            fragment: $"prompt-{promptId}");
    }

    public async Task<IActionResult> Index(string searchTerm = null, string category = null, string sortOrder = "date", int page = 1)
    {
        User user = getUserFromSession();
        var createdResponses = await _profileService.GetPromptsCreados(user.Id);
        
        var prompts = createdResponses.Select(p => new Prompt
        {
            Id = p.IdPromptCreado,
            Title = p.Titulo,
            Content = p.Contenido,
            date = p.FechaPublicacion,
            Tag = new Tag { Label = p.CategoriaLabel, Icon = p.CategoriaIcono },
            Likes = p.LikesCount,
            Comments = new int[p.CommentsCount],
            IsLikedByUser = p.IsLikedByUser > 0
        }).ToList();

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
            Categories = await _libraryServices.GetCategories(),
            SortOrder = sortOrder
        };

        ViewData["UserId"] = user.Id;
        ViewData["Coins"] = user.Coins;
        ViewData["ProfilePhoto"] = GetProfileAddr(user.ProfilePhoto);

        return View(viewModel);
    }

    public IActionResult Comments(int promptId)
    {
        User user = getUserFromSession();
        ViewData["Coins"] = user.Coins;
        ViewData["ProfilePhoto"] = GetProfileAddr(user.ProfilePhoto);
        
        return View(promptId);
    }
}
