namespace WhirlpoolPromptWeb.Models;

public class ShopViewModel
{
    public List<Product> Products { get; set; } = new();
    public PurchaseResult? LastPurchaseResult { get; set; }
}
