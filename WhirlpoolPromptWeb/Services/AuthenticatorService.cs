using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json;
using WhirlpoolPromptWeb.Models;

public class AuthenticatorService : IAuthenticatorService
{

    private readonly HttpClient _httpClient;
    public AuthenticatorService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<UserSession>> AuthenticateUserAPI(string email, string password)
    {
        var url = "https://127.0.0.1:8081/authenticate";

        var payload = new
        {
            email = email,
            password = password
        };

        var json = JsonSerializer.Serialize(payload);

        var content = new StringContent(json, System.Text.Encoding.UTF8,
        "application/json");

        var response = await _httpClient.PostAsync(url, content);

        if (!response.IsSuccessStatusCode)
            return new List<UserSession>();

        var jsonResponse = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<List<UserSession>>(jsonResponse) ?? new List<UserSession>();

    }
}