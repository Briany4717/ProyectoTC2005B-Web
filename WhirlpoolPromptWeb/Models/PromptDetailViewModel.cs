namespace WhirlpoolPromptWeb.Models;

public class PromptDetailViewModel
{
    public Prompt Prompt { get; set; } = null!;
    public string? ReturnUrl { get; set; }
}
