using System.Text.Json;
using WhirlpoolPromptWeb.Models;

namespace WhirlpoolPromptWeb.Services;

public class CreatePromptService : ICreatePromptService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "https://10.14.255.43:6747";

    public CreatePromptService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<InsertarPromptResponse?> InsertarPrompt(string titulo, string contenido, int idCategoria, int idUsuario)
    {
        try
        {
            var payload = new
            {
                titulo = titulo,
                contenido = contenido,
                id_categoria = idCategoria,
                id_usuario = idUsuario
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/insertarPrompt", content);
            if (!response.IsSuccessStatusCode) return null;

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var lista = JsonSerializer.Deserialize<List<InsertarPromptResponse>>(jsonResponse);
            return lista?.FirstOrDefault();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"InsertarPrompt error: {ex.Message}");
            return null;
        }
    }
}