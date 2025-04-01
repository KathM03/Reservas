using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RESERVACIONES.Models;

public partial class Unidade
{
    public int IdUnidad { get; set; }

    [Required]
    [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Solo se permiten letras.")]
    public string NombreUnidad { get; set; }

    [Required]
    [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Solo se permiten letras.")]
    public string Tipo { get; set; } = null!;

    [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Solo se permiten letras.")]
    public string? Estado { get; set; } = null!;

    [Required]
    public decimal PrecioHora { get; set; }

    [JsonIgnore]
    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}