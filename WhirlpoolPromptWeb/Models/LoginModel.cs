using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;


namespace WhirlpoolPromptWeb.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "El usuario es obligatorio.")]
        // [MinLength(5, ErrorMessage = "El usuario debe tener al menos {1} caracteres.")]
        public string User { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        // [MinLength(5, ErrorMessage = "La contraseña debe tener al menos {1} caracteres.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


    }
}
