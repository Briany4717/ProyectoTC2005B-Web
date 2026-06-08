using System.Text.Json;
using System.Text.Json.Serialization;

namespace WhirlpoolPromptWeb.Models;

// Flask returns dates as RFC 2822 ("Thu, 16 Apr 2026 21:32:28 GMT"), not ISO 8601.
public class Rfc2822DateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var raw = reader.GetString();
        return DateTime.Parse(raw!, System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.AdjustToUniversal);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString("R"));
}

public class PerfilUsuarioResponse
{
    [JsonPropertyName("id_usuario")]
    public int IdUsuario { get; set; }

    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [JsonPropertyName("apellido")]
    public string Apellido { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("imagen_perfil")]
    public string? ImagenPerfil { get; set; }

    [JsonPropertyName("descripcion")]
    public string? Descripcion { get; set; }

    [JsonPropertyName("fecha_registro")]
    [JsonConverter(typeof(Rfc2822DateTimeConverter))]
    public DateTime FechaRegistro { get; set; }

    [JsonPropertyName("coins")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int Coins { get; set; }

    [JsonPropertyName("LocalRanking")]
    public int LocalRanking { get; set; }

    [JsonPropertyName("NationalRanking")]
    public int NationalRanking { get; set; }
}

public class PromptCreadoResponse
{
    [JsonPropertyName("id_promptcreado")]
    public int IdPromptCreado { get; set; }

    [JsonPropertyName("titulo")]
    public string Titulo { get; set; } = string.Empty;

    [JsonPropertyName("contenido")]
    public string Contenido { get; set; } = string.Empty;

    [JsonPropertyName("descripcion")]
    public string? Descripcion { get; set; }

    [JsonPropertyName("fecha_publicacion")]
    [JsonConverter(typeof(Rfc2822DateTimeConverter))]
    public DateTime FechaPublicacion { get; set; }

    [JsonPropertyName("categoria_label")]
    public string CategoriaLabel { get; set; } = string.Empty;

    [JsonPropertyName("categoria_icono")]
    public string CategoriaIcono { get; set; } = string.Empty;

    [JsonPropertyName("likes_count")]
    public int LikesCount { get; set; }

    [JsonPropertyName("comments_count")]
    public int CommentsCount { get; set; }

    [JsonPropertyName("is_liked_by_user")]
    public int IsLikedByUser { get; set; }
}

public class PromptGuardadoResponse
{
    [JsonPropertyName("id_promptcreado")]
    public int IdPromptCreado { get; set; }

    [JsonPropertyName("titulo")]
    public string Titulo { get; set; } = string.Empty;

    [JsonPropertyName("contenido")]
    public string Contenido { get; set; } = string.Empty;

    [JsonPropertyName("descripcion")]
    public string? Descripcion { get; set; }

    [JsonPropertyName("fecha_publicacion")]
    [JsonConverter(typeof(Rfc2822DateTimeConverter))]
    public DateTime FechaPublicacion { get; set; }

    [JsonPropertyName("autor_id")]
    public int AutorId { get; set; }

    [JsonPropertyName("autor_nombre")]
    public string AutorNombre { get; set; } = string.Empty;

    [JsonPropertyName("autor_apellido")]
    public string AutorApellido { get; set; } = string.Empty;

    [JsonPropertyName("categoria_label")]
    public string CategoriaLabel { get; set; } = string.Empty;

    [JsonPropertyName("categoria_icono")]
    public string CategoriaIcono { get; set; } = string.Empty;

    [JsonPropertyName("likes_count")]
    public int LikesCount { get; set; }

    [JsonPropertyName("comments_count")]
    public int CommentsCount { get; set; }

    [JsonPropertyName("is_liked_by_user")]
    public int IsLikedByUser { get; set; }
}

public class ToggleLikeResponse
{
    [JsonPropertyName("is_liked")]
    public int IsLiked { get; set; }
}