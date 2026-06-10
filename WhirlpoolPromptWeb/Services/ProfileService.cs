using System.Text;
using System.Text.Json;
using WhirlpoolPromptWeb.Models;

namespace WhirlpoolPromptWeb.Services;

public class ProfileService : IProfileService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "https://10.14.255.43:6747";

    public ProfileService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PerfilUsuarioResponse?> GetPerfilUsuario(int idUsuario)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/usuarios/{idUsuario}/perfil");
            if (!response.IsSuccessStatusCode) return null;

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<PerfilUsuarioResponse>(jsonResponse);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"GetPerfilUsuario error: {ex.Message}");
            return null;
        }
    }

    public async Task<List<PromptCreadoResponse>> GetPromptsCreados(int idUsuario)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/usuarios/{idUsuario}/prompts/creados");
            if (!response.IsSuccessStatusCode) return new List<PromptCreadoResponse>();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<PromptCreadoResponse>>(jsonResponse) ?? new List<PromptCreadoResponse>();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"GetPromptsCreados error: {ex.Message}");
            return new List<PromptCreadoResponse>();
        }
    }

    public async Task<List<PromptGuardadoResponse>> GetPromptsGuardados(int idUsuario)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/usuarios/{idUsuario}/prompts/guardados");
            if (!response.IsSuccessStatusCode) return new List<PromptGuardadoResponse>();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<PromptGuardadoResponse>>(jsonResponse) ?? new List<PromptGuardadoResponse>();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"GetPromptsGuardados error: {ex.Message}");
            return new List<PromptGuardadoResponse>();
        }
    }

    public async Task<ToggleLikeResponse?> ToggleLike(int idPrompt, int idUsuario)
    {
        try
        {
            var response = await _httpClient.PostAsync($"{_baseUrl}/prompts/{idPrompt}/like?id_usuario={idUsuario}", null);
            if (!response.IsSuccessStatusCode) return null;

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ToggleLikeResponse>(jsonResponse);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ToggleLike error: {ex.Message}");
            return null;
        }
    }

    public async Task<Prompt?> GetPromptDetailAsync(int promptId, int userId, string currentUserName)
    {
        // Check saved first — gives us author info + isSaved = true
        var saved = await GetPromptsGuardados(userId);
        var s = saved.FirstOrDefault(x => x.IdPromptCreado == promptId);
        if (s != null)
        {
            return new Prompt
            {
                Id = s.IdPromptCreado,
                Title = s.Titulo,
                Content = s.Contenido,
                Description = s.Descripcion,
                date = s.FechaPublicacion,
                Tag = new Tag { Label = s.CategoriaLabel, Icon = s.CategoriaIcono },
                Likes = s.LikesCount,
                Comments = new int[s.CommentsCount],
                IsLikedByUser = s.IsLikedByUser > 0,
                IsSavedByUser = true,
                AuthorId = s.AutorId,
                AuthorName = $"{s.AutorNombre} {s.AutorApellido}".Trim()
            };
        }

        // Check created — author is the current user, isSaved = false
        var created = await GetPromptsCreados(userId);
        var c = created.FirstOrDefault(x => x.IdPromptCreado == promptId);
        if (c != null)
        {
            return new Prompt
            {
                Id = c.IdPromptCreado,
                Title = c.Titulo,
                Content = c.Contenido,
                Description = c.Descripcion,
                date = c.FechaPublicacion,
                Tag = new Tag { Label = c.CategoriaLabel, Icon = c.CategoriaIcono },
                Likes = c.LikesCount,
                Comments = new int[c.CommentsCount],
                IsLikedByUser = c.IsLikedByUser > 0,
                IsSavedByUser = false,
                AuthorId = userId,
                AuthorName = currentUserName
            };
        }

        return null;
    }

    public async Task ToggleSave(int promptId, int userId, bool currentlySaved)
    {
        try
        {
            if (currentlySaved)
            {
                await _httpClient.DeleteAsync($"{_baseUrl}/savedPrompts/{userId}/{promptId}");
            }
            else
            {
                var body = JsonSerializer.Serialize(new { id_usuario = userId, id_prompt = promptId });
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                await _httpClient.PostAsync($"{_baseUrl}/savePrompt", content);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ToggleSave error: {ex.Message}");
        }
    }
}