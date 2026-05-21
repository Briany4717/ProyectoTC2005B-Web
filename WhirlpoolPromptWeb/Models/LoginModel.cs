using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;


namespace WhirlpoolPromptWeb.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "El usuario es obligatorio.")]
        [MaxLength(20, ErrorMessage = "El usuario no puede exceder {1} caracteres.")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "El usuario solo puede contener letras, números y guiones bajos.")]
        [MinLength(5, ErrorMessage = "El usuario debe tener al menos {1} caracteres.")]
        public string User { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MaxLength(100, ErrorMessage = "La contraseña no puede exceder {1} caracteres.")]
        [MinLength(5, ErrorMessage = "La contraseña debe tener al menos {1} caracteres.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


    }
}
