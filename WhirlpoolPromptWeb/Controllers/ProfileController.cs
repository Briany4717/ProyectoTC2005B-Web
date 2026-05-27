using Microsoft.AspNetCore.Mvc;
using WhirlpoolPromptWeb.Filters;
using WhirlpoolPromptWeb.Models;
using WhirlpoolPromptWeb.Services;

namespace WhirlpoolPromptWeb.Controllers;

[RequireSession]
public class ProfileController : Controller
{
    private readonly ILogger<ProfileController> _logger;
    private readonly IProfileService _profileService;

    public ProfileController(ILogger<ProfileController> logger, IProfileService profileService)
    {
        _logger = logger;
        _profileService = profileService;
    }

    private User getUserFromSession()
    {
        User user = new User();

        user.Id = (int)HttpContext.Session.GetInt32("UserId");
        user.Name = HttpContext.Session.GetString("Name");
        user.Coins = (int)HttpContext.Session.GetInt32("Coins");

        return user;
    }

    private async Task<User> GetUserFromDbAsync(int userId)
    {
        var perfil = await _profileService.GetPerfilUsuario(userId);
        if (perfil == null) return getUserFromSession(); 

        var user = new User
        {
            Id = perfil.IdUsuario,
            Name = perfil.Nombre,
            LastName = perfil.Apellido,
            Coins = perfil.Coins,
            LocalRanking = perfil.LocalRanking,
            NationalRanking = perfil.NationalRanking,
            Birthday = perfil.FechaRegistro
        };

        if (Enum.TryParse<ProfilePhoto>(perfil.ImagenPerfil, true, out var photo))
        {
            user.ProfilePhoto = photo;
        }

        return user;
    }

    private async Task<List<Prompt>> ObtenerPromptsAsync(int userId, string tab)
    {
        if (tab == "Saved")
        {
            var saved = await _profileService.GetPromptsGuardados(userId);
            return saved.Select(s => new Prompt
            {
                Id = s.IdPromptCreado,
                Title = s.Titulo,
                Content = s.Contenido,
                date = s.FechaPublicacion,
                Tag = new Tag { Label = s.CategoriaLabel, Icon = s.CategoriaIcono },
                Likes = s.LikesCount,
                Comments = new int[s.CommentsCount],
                IsLikedByUser = s.IsLikedByUser > 0,
                AuthorId = s.AutorId
            }).ToList();
        }
        else
        {
            var created = await _profileService.GetPromptsCreados(userId);
            return created.Select(c => new Prompt
            {
                Id = c.IdPromptCreado,
                Title = c.Titulo,
                Content = c.Contenido,
                date = c.FechaPublicacion,
                Tag = new Tag { Label = c.CategoriaLabel, Icon = c.CategoriaIcono },
                Likes = c.LikesCount,
                Comments = new int[c.CommentsCount],
                IsLikedByUser = c.IsLikedByUser > 0,
                AuthorId = userId
            }).ToList();
        }
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
        int totalPages = prompts.Count > 0 ? (int)Math.Ceiling(prompts.Count / (double)pageSize) : 1;
        var pageItems = prompts.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        return (pageItems, totalPages);
    }

    [HttpPost]
    public async Task<IActionResult> ToggleLikeProfile(int promptId, string searchTerm, string tab, string sortOrder, int page)
    {
        int? userIdOrNull = HttpContext.Session.GetInt32("UserId");
        if (userIdOrNull == null) return RedirectToAction("Login", "Login");
        
        int userId = userIdOrNull.Value;
        await _profileService.ToggleLike(promptId, userId);
        
        return RedirectToAction("Profile", "Profile",
            new { searchTerm, tab, sortOrder, page },
            fragment: $"prompt-{promptId}");
    }

    public async Task<IActionResult> Profile(string searchTerm = null, string tab = "Created", string sortOrder = "date", int page = 1)
    {
        int? userIdOrNull = HttpContext.Session.GetInt32("UserId");
        if (userIdOrNull == null) return RedirectToAction("Login", "Login");
        
        int userId = userIdOrNull.Value;
        
        User user = await GetUserFromDbAsync(userId);
        
        ViewData["Coins"] = user.Coins;
        ViewData["ProfilePhoto"] = HttpContext.Session.GetString("ProfileAddr");

        var prompts = await ObtenerPromptsAsync(user.Id, tab);
        
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
