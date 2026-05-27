using System.ComponentModel.DataAnnotations;

namespace WhirlpoolPromptWeb.Models;

public class CreatePromptViewModel
{
    [Required(ErrorMessage = "El título es obligatorio.")]
    [MaxLength(100, ErrorMessage = "El título no puede exceder {1} caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El contenido es obligatorio.")]
    [MaxLength(2000, ErrorMessage = "El contenido no puede exceder {1} caracteres.")]
    public string Contenido { get; set; } = string.Empty;

    [Required(ErrorMessage = "La categoría es obligatoria.")]
    public int IdCategoria { get; set; }
}