using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrabajoPractico1._1.ModelViews
{
    public class vReserva
    {

        public List<Versiones> Versiones = new List<Versiones>();
        public List<Sedes> Sedes= new List<Sedes>();
        public List<string> Dias = new List<string>();
        public List<string> Horarios = new List<string>();

        public int? idVersion { get; set; }

        public int? idSede { get; set; }

        public string Dia { get; set; }

         public string FechaHora { get; set; }
        public int IdPelicula {get; set; }

    }
}