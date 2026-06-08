using WhirlpoolPromptWeb.Models;
using System.Text.Json;

namespace WhirlpoolPromptWeb.Services;

public class ShopService : IShopService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "https://127.0.0.1:6747";

    public ShopService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Product>> GetProductsAsync(int userId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/usuarios/{userId}/canciones/compras");
            if (!response.IsSuccessStatusCode) return new List<Product>();

            var json = await response.Content.ReadAsStringAsync();
            var items = JsonSerializer.Deserialize<List<CancionApiResponse>>(json) ?? new List<CancionApiResponse>();

            return items.Select(c => new Product
            {
                Id = c.IdCancion,
                Name = c.NombreCancion,
                Description = c.Descripcion,
                Cost = c.Costo,
                ImageUrl = c.UrlImagen,
                IsOwned = c.Comprada > 0
            }).ToList();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"GetProductsAsync error: {ex.Message}");
            return new List<Product>();
        }
    }

    public async Task<PurchaseResult> PurchaseProductAsync(int productId, int userId, int currentCoins)
    {
        try
        {
            var response = await _httpClient.PostAsync(
                $"{_baseUrl}/usuarios/{userId}/canciones/{productId}/comprar", null);

            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return new PurchaseResult
                {
                    Success = false,
                    Message = "No fue posible completar la compra. Intenta de nuevo.",
                    NewCoinBalance = currentCoins
                };
            }

            var products = await GetProductsAsync(userId);
            var product = products.FirstOrDefault(p => p.Id == productId);
            int newBalance = currentCoins - (product?.Cost ?? 0);

            return new PurchaseResult
            {
                Success = true,
                Message = $"¡Compra exitosa! Ahora tienes acceso a \"{product?.Name ?? "producto"}\".",
                NewCoinBalance = newBalance
            };
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"PurchaseProductAsync error: {ex.Message}");
            return new PurchaseResult
            {
                Success = false,
                Message = "Error de conexión. Intenta de nuevo.",
                NewCoinBalance = currentCoins
            };
        }
    }
}
