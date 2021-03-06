﻿using System.ComponentModel.DataAnnotations;

namespace DAL
{
    [MetadataType(typeof(SedesMetadata))]
    public partial class Sedes
    {
        public class SedesMetadata
        {
            [StringLength(100, ErrorMessage = "El nombre excede el limite de caracteres.")]
            [Required(ErrorMessage = "Debe ingresar el nombre.")]
            public string Nombre { get; set; }

            [StringLength(300, ErrorMessage = "El domicilio excede el límite de caracteres.")]
            [Required(ErrorMessage = "Debe ingresar el domicilio.")]
            public string Direccion { get; set; }

            [Required(ErrorMessage = "Debe ingresar la cantidad de entradas deseadas.")]
            [Range(0, 20000,  ErrorMessage = "El importe ingresado es incorrecto.")]
            public decimal PrecioGeneral { get; set; }
        }
    }
}