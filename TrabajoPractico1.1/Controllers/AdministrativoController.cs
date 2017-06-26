using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrabajoPractico1._1.Models.Servicios;
using TrabajoPractico1._1.Servicios;


namespace TrabajoPractico1._1.Controllers
{
    public class administrativoController : Controller
    {
        ContextoPractico ctx = new ContextoPractico();
        sUsuarios usuariosServiceImpl = new sUsuarios();
        // GET: /Administrativo/

        public ActionResult Inicio()
        {
            return View();
        }

        public ActionResult Sedes()
        {
            ViewBag.Listado = ctx.Sedes.ToList();
            return View();
        }

        public ActionResult crearNuevaSede()
        {
            ViewBag.Nuevo = true;
            ViewBag.Listado = ctx.Sedes.ToList();

            return View("Sedes");
        }

        [HttpPost]
        public ActionResult NuevaSede(Sedes nuevaSede)
        {
            if (ModelState.IsValid)
            {
                Sedes sede = new Sedes();
                sede = (from p in ctx.Sedes
                        where (p.Nombre == nuevaSede.Nombre || p.Direccion == nuevaSede.Direccion)
                        select p).FirstOrDefault();
                if (sede != null)
                    return View("Sede Repetida");
                ctx.Sedes.Add(nuevaSede);
                ctx.SaveChanges();
            }
            ViewBag.Listado = ctx.Sedes.ToList();

            return View("Sedes");
        }

        [HttpPost]
        public ActionResult ModificarSede(Sedes sedeModificada)
        {
            Sedes sedeEncontrada = ctx.Sedes.Find(sedeModificada.IdSede);
            if (sedeEncontrada != null)
            {
                sedeEncontrada.Nombre = sedeModificada.Nombre;
                sedeEncontrada.Direccion = sedeModificada.Direccion;
                sedeEncontrada.PrecioGeneral = sedeModificada.PrecioGeneral;
                Sedes sede = new Sedes();
                sede = (from p in ctx.Sedes
                        where (p.Nombre == sedeModificada.Nombre || p.Direccion == sedeModificada.Direccion)
                        select p).FirstOrDefault();
                if (sede != null)
                    return View("Sede Repetida");
                ctx.SaveChanges();
            }

            ViewBag.Listado = ctx.Sedes.ToList();

            return View("Sedes");
        }

        [HttpGet]
        public ActionResult ModificarSedeSeleccionada(int id)
        {
            Sedes sede = new Sedes();
            sede = (from p in ctx.Sedes
                    where (p.IdSede == id)
                    select p).FirstOrDefault();
            ViewBag.Listado = ctx.Sedes.ToList();

            return View("Sedes",sede);
        }

        // [HttpPost]
        public ActionResult Peliculas()
        {
            ViewBag.Listado = ctx.Peliculas.Include("Generos").ToList();
            return View();
        }
        public ActionResult crearNuevaPelicula()
        {
            ViewBag.Nuevo = true;
            ViewBag.GeneroId = ctx.Generos.ToList();
            ViewBag.CalificacionId = ctx.Calificaciones.ToList();
            ViewBag.Listado = ctx.Peliculas.Include("Generos").ToList();

            return View("Peliculas");
        }
        [HttpPost]
        public ActionResult NuevaPelicula(Peliculas nuevaPelicula)
        {
            if (ModelState.IsValid)
            {
                Peliculas peli = new Peliculas();
                peli = (from p in ctx.Peliculas
                        where (p.Nombre == nuevaPelicula.Nombre && p.IdGenero == nuevaPelicula.IdGenero)
                        select p).FirstOrDefault();
                if (peli != null)
                    return View("Pelicula Repetida");
                else
                {

                    if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                    {
                        //uso el nombre de la pelicula  para crear un nombre significativo
                        string nombrePelicula = nuevaPelicula.Nombre;
                        //Guardar Imagen
                        string pathRelativoImagen = sImagenes.Guardar(Request.Files[0], nombrePelicula);
                        nuevaPelicula.Imagen = pathRelativoImagen;
                    }
                    nuevaPelicula.FechaCarga = System.DateTime.Now;

                    ctx.Peliculas.Add(nuevaPelicula);
                    ctx.SaveChanges();

                }
            }
                ViewBag.Listado = ctx.Peliculas.Include("Generos").ToList();

                return View("Peliculas");
            
        }

        [HttpPost]
        public ActionResult ModificarPelicula(Peliculas peliculaModificada, string myImagen)
        {
            Peliculas peliculaEncontrada = ctx.Peliculas.Find(peliculaModificada.IdPelicula);
            Peliculas peli = new Peliculas();
            if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
            {
                //TODO: Agregar validacion para confirmar que el archivo es una imagen
                if (!string.IsNullOrEmpty(myImagen))
                {
                    //elimina la foto anterior si tenia
                    if (!string.IsNullOrEmpty(peliculaEncontrada.Imagen))
                    {
                        sImagenes.Borrar(peliculaEncontrada.Imagen);
                    }

                    //creo un nombre "nombre" 
                    string nombreSignificativo = peliculaModificada.Nombre;
                    //Guardar Imagen
                    string pathRelativoImagen = sImagenes.Guardar(Request.Files[0], nombreSignificativo);
                    peliculaEncontrada.Imagen = pathRelativoImagen;
                }
            }

            if (peliculaEncontrada != null)
            {
                peliculaEncontrada.Nombre = peliculaModificada.Nombre;
                peliculaEncontrada.Descripcion = peliculaModificada.Descripcion;
                peliculaEncontrada.IdCalificacion = peliculaModificada.IdCalificacion;
                peliculaEncontrada.IdGenero = peliculaModificada.IdGenero;
                peliculaEncontrada.Duracion = peliculaModificada.Duracion;

                
                peli = (from p in ctx.Peliculas
                        where (p.Nombre == peliculaModificada.Nombre && p.IdGenero == peliculaModificada.IdGenero)
                        select p).FirstOrDefault();
                if (peli != null)
                    return View("Pelicula Repetida");

                ctx.SaveChanges();
            }

            ViewBag.Listado = ctx.Peliculas.Include("Generos").ToList();

            return View("Peliculas");
        }

        [HttpGet]
        public ActionResult ModificarPeliculaSeleccionada(int id)
        {
            Peliculas Pelicula = new Peliculas();
            Pelicula = (from p in ctx.Peliculas
                    where (p.IdPelicula == id)
                    select p).FirstOrDefault();
            ViewBag.Listado = ctx.Peliculas.ToList();
            ViewBag.GeneroId = ctx.Generos.ToList();
            ViewBag.CalificacionId = ctx.Calificaciones.ToList();

            return View("Peliculas", Pelicula);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VerificarUsuario(Usuarios usuario)
        {
            if (ModelState.IsValid)
            {
                Usuarios admin = usuariosServiceImpl.logearUsuario(usuario);
                if (admin != null)
                {
                    Session["usuarioEnSesion"] = admin.NombreUsuario;
                    return RedirectToAction("Inicio");
                }
                else
                {
                    TempData["Error"] = "Error de usuario y/o contraseña";
                }
            }
            return RedirectToAction("Login", "home");

        }

        public ActionResult Reportes()
        {
        //  comprobarUsuario(usuarioEnSesion(), "Reportes", "Administrativo");
            if (usuarioEnSesion() != null)
            {
                ViewBag.Listado = ctx.Sedes.ToList();
                return View();
            }
            else 
            {
                return RedirectToAction("Login", "home");
            }
        }

        [HttpPost]
        public ActionResult ArmarReporte(FormCollection formulario)
        {
            sReportes reservaServ = new sReportes();
            DateTime fechaInicio;
            DateTime fechaFin;
            TimeSpan ts;

            try
            {
                fechaInicio = Convert.ToDateTime(formulario["fechaInicio"]);
                fechaFin = Convert.ToDateTime(formulario["fechaFin"]);
                ts = fechaFin - fechaInicio;
            }
            catch (Exception)
            {
                ViewBag.error = "No se cargaron todos los datos";
                return View("Reportes");
            }

            List<Reservas> reservas = new List<Reservas>();

            ViewBag.error = reservaServ.validarReservas(ts);

            if (ViewBag.error != null)
            {
                return View("Reportes");
            }

            reservas = reservaServ.buscarReservasEntreFechas(fechaInicio, fechaFin);

            ViewBag.IntervaloFechas = "El intervalo de fechas buscado es del " + fechaInicio.ToShortDateString() + " hasta " + fechaFin.ToShortDateString() + "";

            return View("Reportes", reservas);
        }

		public ActionResult carteleras ()
		{
			List<Carteleras> listado = ctx.Carteleras.ToList();
			ViewBag.carteleras = listado;
			return View();
		}

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

		[HttpPost]
		public ActionResult cargarCartelera(Carteleras c)
		{
			Carteleras miCartelera = new Carteleras();

			if (ModelState.IsValid)
			{
				c.FechaCarga = System.DateTime.Now;
				ctx.Carteleras.Add(c);
				ctx.SaveChanges();
			}

			var carteleras = ctx.Carteleras.ToList();
			return View("crearCartelera", carteleras);
		}

		public ActionResult eliminarCartelera (int id)
		{
			List<Carteleras> misCarteleras = ctx.Carteleras.ToList();

			Carteleras aBorrar = (from c in ctx.Carteleras
														where c.IdCartelera == id
														select c).FirstOrDefault();
			if (aBorrar != null)
			{
				ctx.Carteleras.Remove(aBorrar);
				ctx.SaveChanges();

				ViewBag.mensajeBorrar = "El registro se ha borrado con éxito.";
			}

			ViewBag.carteleras = ctx.Carteleras.ToList();
			return View("carteleras");
		}

		public ActionResult modificarCartelera (Carteleras c)
		{

			return View("crearCartelera");
		}

		private String usuarioEnSesion()
        {
            if (Session["usuarioEnSesion"] != null)
            {
                String usuario = (string)Session["usuarioEnSesion"];
                return usuario;
            }
            else
            {
                return null;
            }
        }
        /*
        public ActionResult redireccionarLogueo(String action, String controller)
        {
            TempData["Error"] = "Debe logearse para acceder a esta sección";
            TempData["Action"] = action;
            TempData["Controller"] = controller;
            return RedirectToAction("Login", "home");
        }

        private void comprobarUsuario(String usuario, String action, String controller)
        {
            if (usuario == null)
            {
                redireccionarLogueo(action, controller);
            }
        } */
 }
}

