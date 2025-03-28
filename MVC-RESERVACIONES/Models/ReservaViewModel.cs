using System.ComponentModel.DataAnnotations;

namespace MVC_RESERVACIONES.Models
{
    public class ReservaViewModel
    {
        public int IdReserva { get; set; }

        [RegularExpression("^[0-9]+$", ErrorMessage = "Solo se permiten números.")]
        public int? UsuarioId { get; set; }

        [RegularExpression("^[0-9]+$", ErrorMessage = "Solo se permiten números.")]
        public int? UnidadId { get; set; }

        public DateTime? FInicio { get; set; }

        public DateTime? FFin { get; set; }

        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Solo se permiten letras.")]
        public string? TipoEstancia { get; set; } = null!;

        public decimal? Descuento { get; set; }

        public decimal? Total { get; set; }

        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Solo se permiten letras.")]
        public string? Estado { get; set; } = null!;
    }
}
