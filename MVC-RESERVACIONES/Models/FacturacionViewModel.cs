using System.ComponentModel.DataAnnotations;

namespace MVC_RESERVACIONES.Models
{
    public class FacturacionViewModel
    {
        public int IdFact { get; set; }

        [RegularExpression("^[0-9]+$", ErrorMessage = "Solo se permiten números.")]
        public int? IdReserva { get; set; }

        public decimal? Total { get; set; }

        public DateTime? Fecha { get; set; }

        [MinLength(3), MaxLength(50)]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Solo se permiten letras.")]
        public string? Estado { get; set; } = null!;
    }
}
