using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MVC_RESERVACIONES.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [PasswordPropertyText]
        public string Contraseña { get; set; }
    }
}
