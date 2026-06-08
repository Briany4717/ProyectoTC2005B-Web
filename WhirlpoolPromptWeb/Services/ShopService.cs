using WhirlpoolPromptWeb.Models;

namespace WhirlpoolPromptWeb.Services;

public class ShopService : IShopService
{
    private static readonly List<Product> _catalog = new()
    {
        new Product
        {
            Id = 1,
            Name = "Avatar: Princesa Peach",
            Cost = 80,
            ImageUrl = "https://picsum.photos/seed/shop1/600/400",
            Description = "Desbloquea el avatar exclusivo de la Princesa Peach para tu perfil. Destaca en el ranking y muestra tu personalidad única ante la comunidad."
        },
        new Product
        {
            Id = 2,
            Name = "Boost de Visibilidad",
            Cost = 50,
            ImageUrl = "https://picsum.photos/seed/shop2/600/400",
            Description = "Coloca uno de tus prompts en la sección destacada durante 24 horas. Aumenta tus likes y llega a más usuarios de la plataforma."
        },
        new Product
        {
            Id = 3,
            Name = "Pack Premium: IA & Negocios",
            Cost = 120,
            ImageUrl = "https://picsum.photos/seed/shop3/600/400",
            Description = "Accede a una colección curada de 15 prompts premium para casos de uso empresarial: reuniones, correos, estrategia y más."
        },
        new Product
        {
            Id = 4,
            Name = "Badge: Experto Verificado",
            Cost = 200,
            ImageUrl = "https://picsum.photos/seed/shop4/600/400",
            Description = "Obtén la insignia de Experto Verificado en tu perfil. Un reconocimiento permanente que señala la calidad de tus contribuciones a la comunidad."
        },
        new Product
        {
            Id = 5,
            Name = "Categoría Exclusiva: Arte",
            Cost = 75,
            ImageUrl = "https://picsum.photos/seed/shop5/600/400",
            Description = "Desbloquea la categoría 'Arte & Creatividad' y accede a prompts especializados en diseño gráfico, música, escritura creativa y más."
        },
        new Product
        {
            Id = 6,
            Name = "Pack del Mes: Productividad",
            Cost = 90,
            ImageUrl = "https://picsum.photos/seed/shop6/600/400",
            Description = "El pack curado de este mes incluye los 10 prompts más valorados de la categoría Productividad. Selección especial por el equipo editorial."
        },
    };

    public Task<List<Product>> GetProductsAsync()
    {
        // Simula latencia de red
        return Task.FromResult(_catalog.ToList());
    }

    public async Task<PurchaseResult> PurchaseProductAsync(int productId, int userId, int currentCoins)
    {
        // Simula latencia de API
        await Task.Delay(400);

        var product = _catalog.FirstOrDefault(p => p.Id == productId);

        if (product == null)
            return new PurchaseResult
            {
                Success = false,
                Message = "El producto no existe.",
                NewCoinBalance = currentCoins
            };

        if (currentCoins < product.Cost)
            return new PurchaseResult
            {
                Success = false,
                Message = $"Monedas insuficientes. Necesitas {product.Cost - currentCoins} monedas más.",
                NewCoinBalance = currentCoins
            };

        return new PurchaseResult
        {
            Success = true,
            Message = $"¡Compra exitosa! Ahora tienes acceso a \"{product.Name}\".",
            NewCoinBalance = currentCoins - product.Cost
        };
    }
}
