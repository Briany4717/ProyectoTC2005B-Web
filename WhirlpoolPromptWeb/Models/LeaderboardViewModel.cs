namespace WhirlpoolPromptWeb.Models;

public class LeaderboardViewModel
{
    public List<LeaderboardEntry> Entries      { get; set; }
    public LeaderboardEntry       MyPosition   { get; set; }
    public string                 ActiveLeague { get; set; } = "Nacional";
    public int                    CurrentPage  { get; set; } = 1;
    public int                    TotalPages   { get; set; }
    public int                    PageSize     { get; set; } = 5;
    public string                 SearchTerm   { get; set; }

    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage     => CurrentPage < TotalPages;
}
