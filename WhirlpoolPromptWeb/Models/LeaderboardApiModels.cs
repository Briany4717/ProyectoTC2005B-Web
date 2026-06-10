using System.Text.Json.Serialization;

namespace WhirlpoolPromptWeb.Models;

public class LeaderboardEntryResponse
{
    [JsonPropertyName("id_usuario")]
    public int IdUsuario { get; set; }

    [JsonPropertyName("nombre_usuario")]
    public string NombreUsuario { get; set; } = string.Empty;

    [JsonPropertyName("monedas_usuario")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int MonedasUsuario { get; set; }

    [JsonPropertyName("numero_prompts")]
    public int NumeroPrompts { get; set; }

    [JsonPropertyName("votos_acumulados")]
    public int VotosAcumulados { get; set; }
}
