using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrabajoPractico1._1.ModelViews
{
    public class vConfirmacion
    {
        public Peliculas pelicula = new Peliculas();

				public Carteleras cartelera = new Carteleras();

        public DateTime FechaHoraInicio { get; set; }
    }
}