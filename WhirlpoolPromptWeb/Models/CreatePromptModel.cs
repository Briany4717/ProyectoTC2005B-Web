using System.ComponentModel.DataAnnotations;

namespace WhirlpoolPromptWeb.Models
{
    public class CreatePromptModel
    {
        [Required(ErrorMessage = "El título es obligatorio.")]
        [MinLength(5, ErrorMessage = "El título debe tener al menos {1} caracteres.")]
        [MaxLength(100, ErrorMessage = "El título no puede exceder {1} caracteres.")]
        [Display(Name = "Título")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Debes seleccionar una categoría.")]
        [Display(Name = "Categoría")]
        public string Category { get; set; }

        [Required(ErrorMessage = "El contenido del prompt es obligatorio.")]
        [MinLength(20, ErrorMessage = "El prompt debe tener al menos {1} caracteres.")]
        [MaxLength(2000, ErrorMessage = "El prompt no puede exceder {1} caracteres.")]
        [Display(Name = "Prompt")]
        public string Content { get; set; }
    }
}