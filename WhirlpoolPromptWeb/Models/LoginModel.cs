using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;


namespace WhirlpoolPromptWeb.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        [MaxLength(100, ErrorMessage = "El correo electrónico no puede exceder {1} caracteres.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [MaxLength(100, ErrorMessage = "La contraseña no puede exceder {1} caracteres.")]
        [MinLength(5, ErrorMessage = "La contraseña debe tener al menos {1} caracteres.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


    }
}
