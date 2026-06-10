namespace WhirlpoolPromptWeb.Models;

public class LeaderboardEntry
{
    public int    Rank          { get; set; }
    public string Name          { get; set; }
    public string Avatar        { get; set; }
    public int    Coins         { get; set; }
    public int    Prompts       { get; set; }
    public int    Useful        { get; set; }
    public bool   IsCurrentUser { get; set; } = false;
}
