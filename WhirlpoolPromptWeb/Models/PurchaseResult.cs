namespace WhirlpoolPromptWeb.Models;

public class PurchaseResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public int NewCoinBalance { get; set; }
}
