using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrabajoPractico1._1
{
	[MetadataType(typeof(CartelerasMetadata))]
	public partial class Carteleras
	{
		public class CartelerasMetadata
		{
			[Required(ErrorMessage = "El campo es requerido.")]
			public int IdSede { get; set; }

			[Required(ErrorMessage = "El campo es requerido.")]
			public int IdPelicula { get; set; }

			[Range(15, 23, ErrorMessage = "La hora de inicio debe ser mayor a las 15hs y menor a las 00hs.")]
			[Required(ErrorMessage = "El campo es requerido.")]
			public int HoraInicio { get; set; }

            [Required(ErrorMessage = "El campo es requerido.")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
            public DateTime FechaInicio { get; set; }

            [Required(ErrorMessage = "El campo es requerido.")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
            public DateTime FechaFin { get; set; }

            [RegularExpression("^\\d+$", ErrorMessage = "La sala debe ser sólo números.")]
			[Required(ErrorMessage = "El campo es requerido.")]
			public int NumeroSala { get; set; }

			[Required(ErrorMessage = "El campo es requerido.")]
			public int IdVersion { get; set; }

			[Required(ErrorMessage = "El campo es requerido.")]
			public DateTime FechaCarga { get; set; }
		}

		public bool validacionFecha (DateTime inicio, DateTime fin, DateTime _inicio, DateTime _fin)
		{
			if ((_inicio < inicio && _fin < inicio) || (_inicio > fin && _fin > fin))
			{
				return true;
			}

			return false;
		}

	}
}