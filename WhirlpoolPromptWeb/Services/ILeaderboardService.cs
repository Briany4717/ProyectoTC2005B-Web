using WhirlpoolPromptWeb.Models;

namespace WhirlpoolPromptWeb.Services;

public interface ILeaderboardService
{
    Task<List<LeaderboardEntryResponse>> GetLeaderboard(string league);
}
