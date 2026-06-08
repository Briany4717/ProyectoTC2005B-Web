using WhirlpoolPromptWeb.Models;

namespace WhirlpoolPromptWeb.Services;

public interface ILibraryServices
{
    Task<List<string>> GetCategories();
}