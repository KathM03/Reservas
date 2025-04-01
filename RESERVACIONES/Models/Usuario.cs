using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RESERVACIONES.Models;

public partial class Usuario
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
    [MinLength(8, ErrorMessage = "La clave debe tener al menos 8 caracteres.")]
    public string Psswd { get; set; } = null!;

    [Required]
    [MinLength(3), MaxLength(50)]
    [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Solo se permiten letras.")]
    public string Rol { get; set; } = null!;

    [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Solo se permiten letras.")]
    public string? Estado { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}