using System.Text.Json;
using WhirlpoolPromptWeb.Models;

namespace WhirlpoolPromptWeb.Services;

public class LeaderboardService : ILeaderboardService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "https://10.14.255.43:6747";

    public LeaderboardService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<LeaderboardEntryResponse>> GetLeaderboard(string league)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/usuarios/resumen");
            if (!response.IsSuccessStatusCode) return new List<LeaderboardEntryResponse>();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<List<LeaderboardEntryResponse>>(jsonResponse, options)
                   ?? new List<LeaderboardEntryResponse>();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"GetLeaderboard error: {ex.Message}");
            return new List<LeaderboardEntryResponse>();
        }
    }
}
