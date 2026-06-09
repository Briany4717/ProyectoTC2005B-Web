using System.Text.Json.Serialization;

namespace WhirlpoolPromptWeb.Models;

public class CancionApiResponse
{
    [JsonPropertyName("id_cancion")]
    public int IdCancion { get; set; }

    [JsonPropertyName("nombre_cancion")]
    public string NombreCancion { get; set; } = string.Empty;

    [JsonPropertyName("descripcion")]
    public string Descripcion { get; set; } = string.Empty;

    [JsonPropertyName("costo")]
    public int Costo { get; set; }

    [JsonPropertyName("url_imagen")]
    public string UrlImagen { get; set; } = string.Empty;

    [JsonPropertyName("comprada")]
    public int Comprada { get; set; }
}
