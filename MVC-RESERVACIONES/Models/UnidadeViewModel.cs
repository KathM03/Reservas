﻿using System.ComponentModel.DataAnnotations;

namespace MVC_RESERVACIONES.Models
{
    public class UnidadeViewModel
    {
        public int IdUnidad { get; set; }

        public string? NombreUnidad { get; set; }

        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Solo se permiten letras.")]
        public string? Tipo { get; set; } = null!;

        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Solo se permiten letras.")]
        public string? Estado { get; set; } = null!;

        public decimal? PrecioHora { get; set; }
    }
}
