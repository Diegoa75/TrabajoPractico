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
			public DateTime FechaInicio { get; set; }

			[Required(ErrorMessage = "El campo es requerido.")]
			public DateTime FechaFin { get; set; }

			[Required(ErrorMessage = "El campo es requerido.")]
			public int NumeroSala { get; set; }

			[Required(ErrorMessage = "El campo es requerido.")]
			public int IdVersion { get; set; }

			[Required(ErrorMessage = "El campo es requerido.")]
			public DateTime FechaCarga { get; set; }
		}

		//public List<string> horariosDelDia(int horarioInicio, int duracionPelicula)
		//{
		//	List<string> listado = new List<string>();
		//	float _horarioInicio = (float)horarioInicio;

		//	listado.Add(horarioInicio.ToString());
		//	int horarioInicioEnMinutos = 0;
		//	for (int i = 1; i < 7; i++)
		//	{
		//		horarioInicioEnMinutos += duracionPelicula + 30;

		//		int horas = horarioInicioEnMinutos / 60;
		//		int minutos = horarioInicioEnMinutos % 60;

		//		int horarioDeinicioMasHoras = horarioInicio + horas;
		//		decimal minutosADoble = (decimal)(minutos / 100m);

		//		Decimal nuevaFuncion = Decimal.Round((horarioDeinicioMasHoras + minutosADoble), 2);
		//		if(nuevaFuncion > 24)
		//		{
		//			nuevaFuncion = nuevaFuncion - 24;
		//		}
		//		listado.Add(nuevaFuncion.ToString());
		//	}

		//	return listado;
		//}
	}
}