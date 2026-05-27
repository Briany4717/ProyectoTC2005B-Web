using System.Text.Json.Serialization;

namespace WhirlpoolPromptWeb.Models;

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
    public DateTime FechaRegistro { get; set; }

    [JsonPropertyName("coins")]
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