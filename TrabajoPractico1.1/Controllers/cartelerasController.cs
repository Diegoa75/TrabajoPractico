using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrabajoPractico1._1.Controllers
{
  public class cartelerasController : Controller
  {
		ContextoPractico ctx = new ContextoPractico();
		public ActionResult crearCartelera()
    {
			List<Sedes> sedes = ctx.Sedes.ToList();
			ViewBag.sedes = sedes;

			var peliculas = ctx.Peliculas.ToList();
			ViewBag.peliculas = peliculas;

			Carteleras miCartelera = new Carteleras();
			ViewBag.horas = miCartelera.horas();

			var versiones = ctx.Versiones.ToList();
			ViewBag.versiones = versiones;
	
			return View();
    }

  }
}
