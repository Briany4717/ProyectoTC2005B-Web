using WhirlpoolPromptWeb.Models;

namespace WhirlpoolPromptWeb.Services;

public interface IShopService
{
    Task<List<Product>> GetProductsAsync(int userId);
    Task<PurchaseResult> PurchaseProductAsync(int productId, int userId, int currentCoins);
}
