using Microsoft.AspNetCore.Mvc;
using WhirlpoolPromptWeb.Filters;
using WhirlpoolPromptWeb.Models;

namespace WhirlpoolPromptWeb.Controllers;

[RequireSession]
public class ProfileController : Controller
{
    private readonly ILogger<ProfileController> _logger;

    public ProfileController(ILogger<ProfileController> logger)
    {
        _logger = logger;
    }

    private User getUserFromSession()
    {
        User user = new User();

        user.Id = (int)HttpContext.Session.GetInt32("UserId");
        user.Name = HttpContext.Session.GetString("Name");
        user.Coins = (int)HttpContext.Session.GetInt32("Coins");

        return user;
    }

    private List<Prompt> GenerarPromptsFalsos(int userId, string tab)
    {
        if (userId == 0) return HomeController._prompts;
        if (tab == "Saved") return HomeController._prompts.Where(p => p.AuthorId != userId).ToList();
        return HomeController._prompts.Where(p => p.AuthorId == userId).ToList();
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

    private void ApplyToggleLike(int promptId)
    {
        var prompt = HomeController._prompts.FirstOrDefault(p => p.Id == promptId);
        if (prompt == null) return;
        prompt.IsLikedByUser = !prompt.IsLikedByUser;
        prompt.Likes += prompt.IsLikedByUser ? 1 : -1;
    }

    [HttpPost]
    public IActionResult ToggleLikeProfile(int promptId, string searchTerm, string tab, string sortOrder, int page)
    {
        ApplyToggleLike(promptId);
        return RedirectToAction("Profile", "Profile",
            new { searchTerm, tab, sortOrder, page },
            fragment: $"prompt-{promptId}");
    }

    public IActionResult Profile(string searchTerm = null, string tab = "Created", string sortOrder = "date", int page = 1)
    {
        User user = getUserFromSession();
        ViewData["Coins"] = user.Coins;
        ViewData["ProfilePhoto"] = HttpContext.Session.GetString("ProfileAddr");

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
}