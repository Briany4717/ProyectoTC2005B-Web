using System.Text.Json.Serialization;

namespace WhirlpoolPromptWeb.Models;

public class PromptCategoryConId
{
    [JsonPropertyName("id_categoria")]
    public int IdCategoria { get; set; }

    [JsonPropertyName("nombre")]
    public string Nombre { get; set; }
}