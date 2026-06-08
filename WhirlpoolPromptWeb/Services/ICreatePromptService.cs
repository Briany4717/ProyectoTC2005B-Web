using WhirlpoolPromptWeb.Models;

namespace WhirlpoolPromptWeb.Services;

public interface ICreatePromptService
{
    Task<InsertarPromptResponse?> InsertarPrompt(string titulo, string contenido, int idCategoria, int idUsuario);
}