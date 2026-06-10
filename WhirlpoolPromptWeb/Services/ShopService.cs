using WhirlpoolPromptWeb.Models;
using System.Text.Json;

namespace WhirlpoolPromptWeb.Services;

public class ShopService : IShopService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "https://10.14.255.43:6747";

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
            var data = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            var resultado = data?.GetValueOrDefault("resultado") ?? "";

            return resultado switch
            {
                "Compra exitosa" => new PurchaseResult
                {
                    Success = true,
                    Message = "¡Compra exitosa!",
                    NewCoinBalance = await GetUpdatedCoinBalance(userId, currentCoins)
                },
                "Saldo Insuficiente" => new PurchaseResult
                {
                    Success = false,
                    Message = "Saldo insuficiente para realizar esta compra.",
                    NewCoinBalance = currentCoins
                },
                _ => new PurchaseResult
                {
                    Success = false,
                    Message = resultado,
                    NewCoinBalance = currentCoins
                }
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

    private async Task<int> GetUpdatedCoinBalance(int userId, int fallback)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/usuarios/{userId}/perfil");
            if (!response.IsSuccessStatusCode) return fallback;

            var json = await response.Content.ReadAsStringAsync();
            var perfil = JsonSerializer.Deserialize<PerfilUsuarioResponse>(json);
            return perfil?.Coins ?? fallback;
        }
        catch
        {
            return fallback;
        }
    }
}
