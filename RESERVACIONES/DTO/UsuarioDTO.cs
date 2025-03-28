using System.ComponentModel.DataAnnotations;

namespace RESERVACIONES.DTO
{
    public class UsuarioDTO
    {
        public int Id { get; set; }

        [MinLength(3), MaxLength(50)]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Solo se permiten letras.")]
        public string? Nombre { get; set; } = null!;

        [MinLength(3), MaxLength(50)]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Solo se permiten letras.")]
        public string? Apellido { get; set; } = null!;

        [EmailAddress(ErrorMessage = "Ingrese un formato de Email Válido.")]
        public string? Email { get; set; } = null!;

        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        public string? Contraseña { get; set; } = null!;

        [MinLength(3), MaxLength(50)]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Solo se permiten letras.")]
        public string? Rol { get; set; } = null!;

        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Solo se permiten letras.")]
        public string? Estado { get; set; } = null!;
    }
}
