namespace WhirlpoolPromptWeb.Models;

public class LeaderboardEntry
{
    public int Rank         { get; set; }
    public string Name      { get; set; }
    public string Avatar    { get; set; } 
    public int Coins        { get; set; }
    public int Prompts      { get; set; }
    public int Useful       { get; set; }
    public bool IsCurrentUser { get; set; } = false;
}

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