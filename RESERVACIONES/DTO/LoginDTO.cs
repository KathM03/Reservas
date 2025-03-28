using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace RESERVACIONES.DTO
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [PasswordPropertyText]
        public string Contraseña { get; set; }
    }
}
