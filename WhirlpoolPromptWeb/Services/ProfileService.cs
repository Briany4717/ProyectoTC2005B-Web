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
}