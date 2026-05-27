using WhirlpoolPromptWeb.Models;

namespace WhirlpoolPromptWeb.Services;

public interface IProfileService
{
    Task<PerfilUsuarioResponse?> GetPerfilUsuario(int idUsuario);
    Task<List<PromptCreadoResponse>> GetPromptsCreados(int idUsuario);
    Task<List<PromptGuardadoResponse>> GetPromptsGuardados(int idUsuario);
    Task<ToggleLikeResponse?> ToggleLike(int idPrompt, int idUsuario);
}