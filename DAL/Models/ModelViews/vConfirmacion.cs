using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DAL.ModelViews
{
    public class vConfirmacion
    {
        public Peliculas pelicula = new Peliculas();

				public Carteleras cartelera = new Carteleras();

        public DateTime FechaHoraInicio { get; set; }
    }
}