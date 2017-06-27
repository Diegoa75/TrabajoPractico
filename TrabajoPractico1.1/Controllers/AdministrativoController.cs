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
        //Administrativo/
                                            //LOGIN
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VerificarUsuario(Usuarios usuario)
        {
            if (ModelState.IsValid)
            {
                sUsuarios servicioUsuarios = new sUsuarios();
                Usuarios admin = servicioUsuarios.logearUsuario(usuario);
                if (admin != null)
                {
                    //verifica si necesita redirigir a una pagina
                    Session["usuarioEnSesion"] = admin.NombreUsuario;
                    if (Session["Action"] == null)
                        return RedirectToAction("Inicio");
                    else
                    { 
                        string action = Session["Action"] as string ;
                        Session.Remove("Action");
                        return RedirectToAction(action);
                    }
                }
                else
                {
                    TempData["Error"] = "Error de usuario y/o contraseña";
                }
            }
            return RedirectToAction("Login", "home");

        }
                                            //VERIFICAR SESION
        public Boolean comprobarUsuario(String action)
        {
            //si el usuario existe devuelve true, sino crea la variable de sesion con el action al que tiene que volver
            if (Session["usuarioEnSesion"] != null)
            {
                string usuario = Session["usuarioEnSesion"] as string;
                var usuarioExistente = ctx.Usuarios.Where(u => u.NombreUsuario == usuario
                                                          ).SingleOrDefault();
                if (usuarioExistente != null)
                {
                    return true;
                }
            }

						Session["Action"] = action;
            return false;
        }

                                            //INICIO
        public ActionResult Inicio()
        {
            if (comprobarUsuario("Inicio"))
                return View();
            else
                return RedirectToAction("Login", "home");
        }

                                            //SEDES
        public ActionResult Sedes()
        {
            if (comprobarUsuario("Sedes"))
            { 
                ViewBag.Listado = ctx.Sedes.ToList();
                return View();
            }
            else
                return RedirectToAction("Login", "home");
        }

        public ActionResult NuevaSede()
        {
            if (comprobarUsuario("Sedes"))
            {
                ViewBag.Nuevo = true;
                ViewBag.Listado = ctx.Sedes.ToList();

                return View("Sedes");
            }
            else
                return RedirectToAction("Login", "home");
        }

        [HttpPost]
        public ActionResult NuevaSede(Sedes nuevaSede)
        {
            if (ModelState.IsValid)
            {
                //verifica que no se este cargando una sede con mismo nombre y direccion a una existente
                var sedeExistente = ctx.Sedes.Where(s => s.Nombre == nuevaSede.Nombre 
                                                    && s.Direccion == nuevaSede.Direccion).FirstOrDefault();

                if (sedeExistente != null)
                {
                    ViewBag.Error = "Ya existe una Sede con ese nombre y esa dirección";
                }
                else
                {   
                    //guarda la nueva sede
                    ctx.Sedes.Add(nuevaSede);
                    ctx.SaveChanges();
                }
            }
            ViewBag.Listado = ctx.Sedes.ToList();

            return View("Sedes");
        }

        public ActionResult ModificarSede(int id)
        {
            if (comprobarUsuario("Sedes"))
            {
                Sedes sede = ctx.Sedes.Where(p => p.IdSede == id).SingleOrDefault();
                ViewBag.Listado = ctx.Sedes.ToList();

                //si la sede es encontrada la devuelve para modificarla
                if (sede != null)
                    return View("Sedes", sede);
                else
                    return View("Sedes");
            }
            else
                return RedirectToAction("Login", "home");
        }

        [HttpPost]
        public ActionResult ModificarSede(Sedes sedeModificada)
        {
            //verifica que no se este cargando una sede con mismo nombre y direccion a una existente
            var sedeExistente = ctx.Sedes.Where(s => s.Nombre == sedeModificada.Nombre
                                                    && s.Direccion == sedeModificada.Direccion
                                                    && s.IdSede != sedeModificada.IdSede).FirstOrDefault();

            if (sedeExistente != null)
            {
                ViewBag.Error = "Ya existe una Sede con ese nombre y esa dirección";
            }
            else
            {
                //busca la sede a modificar
                Sedes sedeEncontrada = ctx.Sedes.Find(sedeModificada.IdSede);

                if (sedeEncontrada != null)
                {
                    //modifica los datos por los actuales
                    sedeEncontrada.Nombre = sedeModificada.Nombre;
                    sedeEncontrada.Direccion = sedeModificada.Direccion;
                    sedeEncontrada.PrecioGeneral = sedeModificada.PrecioGeneral;

                    ctx.SaveChanges();
                }
            }
            ViewBag.Listado = ctx.Sedes.ToList();

            return View("Sedes");
        }


                                            //PELICULAS
        public ActionResult Peliculas()
        {
            if (comprobarUsuario("Peliculas"))
            { 
                ViewBag.Listado = ctx.Peliculas.ToList();
                return View();
            }
            else
                return RedirectToAction("Login", "home");
    }
        public ActionResult NuevaPelicula()
        {
            if (comprobarUsuario("Peliculas"))
            {
                ViewBag.Nuevo = true;
                ViewBag.GeneroId = ctx.Generos.ToList();
                ViewBag.CalificacionId = ctx.Calificaciones.ToList();
                ViewBag.Listado = ctx.Peliculas.Include("Generos").ToList();

                return View("Peliculas");
            }
            else
                return RedirectToAction("Login", "home");
        }

        [HttpPost]
        public ActionResult NuevaPelicula(Peliculas nuevaPelicula)
        {
            if (ModelState.IsValid)
            {
                //Revisa que no exista otra pelicula con el mismo nombre
                var peliculaExistente = ctx.Peliculas.Where(p => p.Nombre == nuevaPelicula.Nombre
                                                            && p.IdPelicula != nuevaPelicula.IdPelicula)
                                                            .FirstOrDefault(); 
                if (peliculaExistente != null)
                {
                    ViewBag.Error = "Ya existe una Pelicula con ese nombre";
                }
                else
                {
                    //Revisa si existe una imagen cargada para la pelicula
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

        public ActionResult ModificarPelicula(int id)
        {
            if (comprobarUsuario("Peliculas"))
            {
                Peliculas Pelicula = ctx.Peliculas.Where(p => p.IdPelicula == id).FirstOrDefault();
                ViewBag.Listado = ctx.Peliculas.ToList();
                ViewBag.GeneroId = ctx.Generos.ToList();
                ViewBag.CalificacionId = ctx.Calificaciones.ToList();

                return View("Peliculas", Pelicula);
            }
            else
                return RedirectToAction("Login", "home");
        }

        [HttpPost]
        public ActionResult ModificarPelicula(Peliculas peliculaModificada, string myImagen)
        {
            //busca la pelicula a modificar
            Peliculas peliculaEncontrada = ctx.Peliculas.Find(peliculaModificada.IdPelicula);

            if (peliculaEncontrada != null)
            {
                //consulta si existe otra pelicula con el mismo nombre
                var peliculaExistente = ctx.Peliculas.Where(p => p.Nombre == peliculaModificada.Nombre 
                                                            && p.IdPelicula != peliculaModificada.IdPelicula)
                                                            .FirstOrDefault();

                if (peliculaExistente != null)
                {
                    ViewBag.Error = "Ya existe una Pelicula con ese nombre";
                }
                else
                { 
                    //Comprueba si se agrego una foto nueva para modificar la actual
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

                    //modifica los datos existentes por los nuevos
                    peliculaEncontrada.Nombre = peliculaModificada.Nombre;
                    peliculaEncontrada.Descripcion = peliculaModificada.Descripcion;
                    peliculaEncontrada.IdCalificacion = peliculaModificada.IdCalificacion;
                    peliculaEncontrada.IdGenero = peliculaModificada.IdGenero;
                    peliculaEncontrada.Duracion = peliculaModificada.Duracion;
                
                    ctx.SaveChanges();
                }
            }
            ViewBag.Listado = ctx.Peliculas.ToList();

            return View("Peliculas");
        }

                                            //REPORTES
        public ActionResult Reportes()
        {
            if (comprobarUsuario("Reportes"))
            {
                return View();
            }
            else
                return RedirectToAction("Login", "home");
        }

        [HttpPost]
        public ActionResult Reporte(FormCollection formulario)
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

																			// CARTELERAS
		public ActionResult carteleras()
		{
			List<Carteleras> listado = ctx.Carteleras.ToList();
			return View(listado);
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

			c.FechaCarga = DateTime.Now;
			if (ModelState.IsValid)
			{
				ctx.Carteleras.Add(c);
				ctx.SaveChanges();
			}

			var carteleras = ctx.Carteleras.ToList();
			return View("carteleras", carteleras);
		}

		public ActionResult eliminarCartelera(int id)
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

		public ActionResult modificarCartelera(int id)
		{
			Carteleras aModificar = (from c in ctx.Carteleras
															 where c.IdCartelera == id
															 select c).FirstOrDefault();

			return View("crearCartelera", aModificar);
		}

	}
}