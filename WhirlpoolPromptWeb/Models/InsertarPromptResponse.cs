
using System.Text.Json.Serialization;

namespace WhirlpoolPromptWeb.Models;

public class InsertarPromptResponse
{
    [JsonPropertyName("mensaje")]
    public string? Mensaje { get; set; }

    [JsonPropertyName("id_prompt")]
    public int? IdPrompt { get; set; }
}