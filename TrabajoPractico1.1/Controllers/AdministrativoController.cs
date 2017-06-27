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
        sPeliculas peliculaServiceImpl = new sPeliculas();
        sReportes reporteServiceImpl = new sReportes();
        sSedes sedeServiceImpl = new sSedes();
        sGeneros generoServiceImpl = new sGeneros();
        sCalificaciones calificacionServiceImpl = new sCalificaciones();
        sCarteleras carteleraServiceImpl = new sCarteleras();
        sVersiones versionServiceImpl = new sVersiones();
        sUsuarios usuarioServiceImpl = new sUsuarios();

        //Administrativo/
        //LOGIN
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VerificarUsuario(Usuarios usuario)
        {
            if (ModelState.IsValid)
            {
                Usuarios admin = usuarioServiceImpl.logearUsuario(usuario);
                if (admin != null)
                {
                    //verifica si necesita redirigir a una pagina
                    Session["usuarioEnSesion"] = admin.NombreUsuario;
                    if (Session["Action"] == null)
                        return RedirectToAction("Inicio");
                    else
                    {
                        string action = Session["Action"] as string;
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
                var usuarioExistente = usuarioServiceImpl.buscarUsuarioPorNombre(usuario);
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
                ViewBag.Listado = sedeServiceImpl.obtenerSedes();
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
                ViewBag.Listado = sedeServiceImpl.obtenerSedes();

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
                var sedeExistente = sedeServiceImpl.buscarSedePorNombreYDireccion(nuevaSede);

                if (sedeExistente != null)
                {
                    ViewBag.Error = "Ya existe una Sede con ese nombre y esa dirección";
                }
                else
                {
                    sedeServiceImpl.guardarSede(nuevaSede);
                }
            }

            ViewBag.Listado = sedeServiceImpl.obtenerSedes();

            return View("Sedes");
        }

        public ActionResult ModificarSede(int id)
        {
            if (comprobarUsuario("Sedes"))
            {
                Sedes sede = sedeServiceImpl.buscarSedePorId(id);
                ViewBag.Listado = sedeServiceImpl.obtenerSedes();

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
            var sedeExistente = sedeServiceImpl.buscarSedeIgualALaModificada(sedeModificada);

            if (sedeExistente != null)
            {
                ViewBag.Error = "Ya existe una Sede con ese nombre y esa dirección";
            }
            else
            {
                sedeServiceImpl.modificarSede(sedeModificada);
            }

            ViewBag.Listado = sedeServiceImpl.obtenerSedes();

            return View("Sedes");
        }


        //PELICULAS
        public ActionResult Peliculas()
        {
            if (comprobarUsuario("Peliculas"))
            {
                ViewBag.Listado = peliculaServiceImpl.obtenerPeliculas();
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
                ViewBag.GeneroId = generoServiceImpl.obtenerGeneros();
                ViewBag.CalificacionId = calificacionServiceImpl.obtenerCalificaciones();
                ViewBag.Listado = peliculaServiceImpl.obtenerPeliculasYGeneros();

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
                var peliculaExistente = peliculaServiceImpl.buscarPeliculaConMismoNombre(nuevaPelicula);

                if (peliculaExistente != null)
                    ViewBag.Error = "Ya existe una Pelicula con ese nombre";
                else
                {
                    //Revisa si existe una imagen cargada para la pelicula
                    if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                    {
                        //uso el nombre de la pelicula  para crear un nombre significativo
                        string nombrePelicula = nuevaPelicula.Nombre;
                        string pathRelativoImagen = sImagenes.Guardar(Request.Files[0], nombrePelicula);
                        nuevaPelicula.Imagen = pathRelativoImagen;
                    }
                    nuevaPelicula.FechaCarga = System.DateTime.Now;
                    peliculaServiceImpl.guardarPelicula(nuevaPelicula);
                }
            }
            ViewBag.Listado = peliculaServiceImpl.obtenerPeliculasYGeneros();

            return View("Peliculas");
        }

        public ActionResult ModificarPelicula(int id)
        {
            if (comprobarUsuario("Peliculas"))
            {
                Peliculas Pelicula = peliculaServiceImpl.obtenerPeliculaPorId(id);
                
                ViewBag.Listado = peliculaServiceImpl.obtenerPeliculas();
                ViewBag.GeneroId = generoServiceImpl.obtenerGeneros();
                ViewBag.CalificacionId = calificacionServiceImpl.obtenerCalificaciones();

                return View("Peliculas", Pelicula);
            }
            else
                return RedirectToAction("Login", "home");
        }

        [HttpPost]
        public ActionResult ModificarPelicula(Peliculas peliculaModificada, string myImagen)
        {
            Peliculas peliculaEncontrada = peliculaServiceImpl.obtenerPeliculaPorId(peliculaModificada.IdPelicula);

            if (peliculaEncontrada != null)
            {
                var peliculaExistente = peliculaServiceImpl.buscarPeliculaConMismoNombre(peliculaModificada);

                if (peliculaExistente != null)
                    ViewBag.Error = "Ya existe una Pelicula con ese nombre";
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
                            string pathRelativoImagen = sImagenes.Guardar(Request.Files[0], nombreSignificativo);
                            peliculaEncontrada.Imagen = pathRelativoImagen;
                        }
                    }

                    peliculaServiceImpl.modificarPelicula(peliculaEncontrada, peliculaModificada);

                }
            }
            ViewBag.Listado = peliculaServiceImpl.obtenerPeliculas();

            return View("Peliculas");
        }

        //REPORTES
        public ActionResult Reportes()
        {
            if (comprobarUsuario("Reportes"))
            {
                ViewBag.Peliculas = peliculaServiceImpl.obtenerPeliculas();
                return View();
            }
            else
                return RedirectToAction("Login", "home");
        }

        [HttpPost]
        public ActionResult Reporte(FormCollection formulario)
        {
            DateTime fechaInicio;
            DateTime fechaFin;
            TimeSpan ts;
            int idPelicula;
            ViewBag.Peliculas = peliculaServiceImpl.obtenerPeliculas();

            try
            {
                fechaInicio = Convert.ToDateTime(formulario["fechaInicio"]);
                fechaFin = Convert.ToDateTime(formulario["fechaFin"]);
                idPelicula = Convert.ToInt16(formulario["idPelicula"]);
                ts = fechaFin - fechaInicio;
            }
            catch (Exception)
            {
                ViewBag.error = "No se cargaron todos los datos";
                return View("Reportes");
            }

            List<Reservas> reservas = new List<Reservas>();
            ViewBag.error = reporteServiceImpl.validarReservas(ts);

            if (ViewBag.error != null)
            {
                return View("Reportes");
            }

            reservas = reporteServiceImpl.buscarReservasEntreFechas(fechaInicio, fechaFin, idPelicula);
            ViewBag.IntervaloFechas = "El intervalo de fechas buscado es del " + fechaInicio.ToShortDateString() + " hasta " + fechaFin.ToShortDateString() + " para la pelicula ";

            return View("Reportes", reservas);
        }


        // CARTELERAS
        public ActionResult carteleras()
        {
            if (comprobarUsuario("carteleras"))
            {
                List<Carteleras> listado = carteleraServiceImpl.obtenerCarteleras();
                return View(listado);
            }
            else
                return RedirectToAction("Login", "home");
        }

        public ActionResult crearCartelera()
        {
            if (comprobarUsuario("carteleras"))
            {
                List<Sedes> sedes = sedeServiceImpl.obtenerSedes();
                ViewBag.sedes = sedes;

                var peliculas = peliculaServiceImpl.obtenerPeliculas();
                ViewBag.peliculas = peliculas;

                var versiones = versionServiceImpl.obtenerVersiones();
                ViewBag.versiones = versiones;

                return View();
            }
            else
                return RedirectToAction("Login", "home");
        }

		[HttpPost]
		public ActionResult cargarCartelera(Carteleras c)
		{
            if (ModelState.IsValid)
            {
                //validacion no repetir sede/pelicula/version
                var registroRepetido = carteleraServiceImpl.buscarPorSedePeliculaYVersion(c);
                //validacion solapamiento de fechas
                var _carteleras = carteleraServiceImpl.buscarPorFechaDiasYSalas(c);

                if (registroRepetido == null && _carteleras.Count == 0)
                {
                    //si hay que modificar trae el registro a editar
			        var registroAmodificar = carteleraServiceImpl.buscarPorId(c.IdCartelera);
                    
			        if (registroAmodificar != null)
			        {
                        carteleraServiceImpl.modificarCartelera(registroAmodificar, c);

                        return RedirectToAction("carteleras");
			        }
                   
                    //si no hay que modificar crea el registro nuevo
			        c.FechaCarga = DateTime.Now;
				
					carteleraServiceImpl.agregarCartelera(c);
				}
				else
				{
					ViewBag.errorCarga = "No se puede cargar la cartelera.";
				}
			}
            List<Carteleras> listado = carteleraServiceImpl.obtenerCarteleras();
			return View("carteleras",listado);
		}

		public ActionResult eliminarCartelera(int id)
        {
            if (comprobarUsuario("carteleras"))
            {
                Carteleras aBorrar = carteleraServiceImpl.buscarPorId(id);

                if (aBorrar != null)
                {
                    carteleraServiceImpl.eliminarCartelera(aBorrar);
                    ViewBag.mensajeBorrar = "El registro se ha borrado con éxito.";
                }

                List<Carteleras> listado = carteleraServiceImpl.obtenerCarteleras();
                return View("carteleras", listado);
            }
            else
                return RedirectToAction("Login", "home");
        }

        public ActionResult modificarCartelera(int id)
        {
            if (comprobarUsuario("carteleras"))
            {
                ViewBag.sedes = sedeServiceImpl.obtenerSedes();

                ViewBag.peliculas = peliculaServiceImpl.obtenerPeliculas();

                ViewBag.versiones = versionServiceImpl.obtenerVersiones();

                Carteleras aModificar = carteleraServiceImpl.buscarPorId(id);

                return View("crearCartelera", aModificar);
            }
            else
                return RedirectToAction("Login", "home");
        }

    }
}