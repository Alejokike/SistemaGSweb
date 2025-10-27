﻿using System.ComponentModel.DataAnnotations;

namespace SistemaGS.DTO
{
    public class InventarioDTO
    {
        public int IdTransaccion { get; set; }
        [Required(ErrorMessage = "Seleccione un tipo de operación")]
        public string? TipoOperacion { get; set; }
        [Required(ErrorMessage = "Ingrese un item")]
        public int? Item { get; set; }
        [Required(ErrorMessage = "Ingrese la unidad de medida")]
        public string? Unidad { get; set; }
        [Required(ErrorMessage = "Ingrese una cantidad")]
        [Range(0, double.MaxValue, ErrorMessage = "El valor no puede ser negativo")]
        public decimal? Cantidad { get; set; }
        [Required(ErrorMessage = "Ingrese concepto")]
        public string? Concepto { get; set; }
        public DateTime? Fecha { get; set; }
    }
}
