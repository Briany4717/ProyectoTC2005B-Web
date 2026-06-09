namespace WhirlpoolPromptWeb.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Cost { get; set; }
    public string ImageUrl { get; set; }
    public string Description { get; set; }
    public bool IsOwned { get; set; }
}
