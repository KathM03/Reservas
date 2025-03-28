using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RESERVACIONES.Models;

public partial class Reserva
{
    public int IdReserva { get; set; }

    [Required]
    [RegularExpression("^[0-9]+$", ErrorMessage = "Solo se permiten números.")]
    public int UsuarioId { get; set; }

    [Required]
    [RegularExpression("^[0-9]+$", ErrorMessage = "Solo se permiten números.")]
    public int UnidadId { get; set; }

    [Required]
    public DateTime FInicio { get; set; }

    [Required]
    public DateTime FFin { get; set; }

    [Required]
    [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Solo se permiten letras.")]
    public string TipoEstancia { get; set; } = null!;

    public decimal? Descuento { get; set; }

    public decimal? Total { get; set; }

    [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Solo se permiten letras.")]
    public string? Estado { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Facturacion> Facturacions { get; set; } = new List<Facturacion>();

    [JsonIgnore]
    public virtual Unidade? Unidad { get; set; }

    [JsonIgnore]
    public virtual Usuario? Usuario { get; set; }
}
