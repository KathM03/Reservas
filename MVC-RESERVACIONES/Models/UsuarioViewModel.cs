﻿using System.ComponentModel.DataAnnotations;

namespace MVC_RESERVACIONES.Models
{
    public class UsuarioViewModel
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3), MaxLength(50)]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Solo se permiten letras.")]
        public string Nombre { get; set; } = null!;

        [Required]
        [MinLength(3), MaxLength(50)]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Solo se permiten letras.")]
        public string Apellido { get; set; } = null!;

        [Required]
        [EmailAddress(ErrorMessage = "Ingrese un formato de Email Válido.")]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        public string Contraseña { get; set; } = null!;

        [Required]
        [MinLength(3), MaxLength(50)]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Solo se permiten letras.")]
        public string Rol { get; set; } = null!;

        [Required]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Solo se permiten letras.")]
        public string Estado { get; set; } = null!;
    }
}
