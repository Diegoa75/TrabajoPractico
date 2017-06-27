﻿using System;
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
        sPeliculas peliculaServiceImpl = new sPeliculas();
        sReportes reporteServiceImpl = new sReportes();
        sSedes sedeServiceImpl = new sSedes();
        sGeneros generoServiceImpl = new sGeneros();
        sCalificaciones calificacionServiceImpl = new sCalificaciones();
        sCarteleras carteleraServiceImpl = new sCarteleras();
        sVersiones versionServiceImpl = new sVersiones();

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
                //busca la sede a modificar
                Sedes sedeEncontrada = sedeServiceImpl.buscarSedePorId(sedeModificada.IdSede);

                if (sedeEncontrada != null)
                {
                    //modifica los datos por los actuales
                    sedeEncontrada.Nombre = sedeModificada.Nombre;
                    sedeEncontrada.Direccion = sedeModificada.Direccion;
                    sedeEncontrada.PrecioGeneral = sedeModificada.PrecioGeneral;

                    sedeServiceImpl.guardarCambiosEnContexto();
                }
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

                    //modifica los datos existentes por los nuevos
                    peliculaEncontrada.Nombre = peliculaModificada.Nombre;
                    peliculaEncontrada.Descripcion = peliculaModificada.Descripcion;
                    peliculaEncontrada.IdCalificacion = peliculaModificada.IdCalificacion;
                    peliculaEncontrada.IdGenero = peliculaModificada.IdGenero;
                    peliculaEncontrada.Duracion = peliculaModificada.Duracion;

                    peliculaServiceImpl.guardarContexto();
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
            Carteleras miCartelera = new Carteleras();

            c.FechaCarga = DateTime.Now;
            if (ModelState.IsValid)
            {
                // Si los tres parametros son iguales, no pasa la validacion.
                // Si al menos uno de los tres es distinto, guarda en BD.
                var registroRepetido = carteleraServiceImpl.buscarPorSedePeliculaYVersion(c);

                // Guarda todas las sedes repetidas
                var sedes = carteleraServiceImpl.buscarPorSede(c);

                Carteleras pisaDias = new Carteleras();
                Carteleras pisaFechas = new Carteleras();
                // Verifica que no se solapen dias en las salas de las sedes.
                foreach (Carteleras soloSedesIguales in sedes)
                {
                    if (soloSedesIguales.NumeroSala == c.NumeroSala)
                    {
                        if (soloSedesIguales.Lunes != c.Lunes &&
                                soloSedesIguales.Martes != c.Martes &&
                                soloSedesIguales.Miercoles != c.Miercoles &&
                                soloSedesIguales.Jueves != c.Jueves &&
                                soloSedesIguales.Viernes != c.Viernes &&
                                soloSedesIguales.Sabado != c.Sabado &&
                                soloSedesIguales.Domingo != c.Domingo)
                        {
                            pisaDias = soloSedesIguales;
                        }

                        if (soloSedesIguales.validacionFecha(soloSedesIguales.FechaInicio, soloSedesIguales.FechaFin, c.FechaInicio) ||
                                soloSedesIguales.validacionFecha(soloSedesIguales.FechaInicio, soloSedesIguales.FechaFin, c.FechaFin))
                        {
                            pisaFechas = soloSedesIguales;
                        }
                    }
                }

                if (registroRepetido.IdCartelera > 0 && pisaDias.IdCartelera > 0 && pisaFechas.IdCartelera > 0)
                {
                    carteleraServiceImpl.agregarCartelera(c);
                }
                else
                {
                    ViewBag.errorCarga = "No se puede cargar la cartelera.";
                }
            }

            var carteleras = carteleraServiceImpl.obtenerCarteleras();
            return View("carteleras", carteleras);
        }

        public ActionResult eliminarCartelera(int id)
        {
            if (comprobarUsuario("carteleras"))
            {
                List<Carteleras> misCarteleras = carteleraServiceImpl.obtenerCarteleras();

                Carteleras aBorrar = carteleraServiceImpl.buscarPorId(id);

                if (aBorrar != null)
                {
                    carteleraServiceImpl.eliminarCartelera(aBorrar);
                    ViewBag.mensajeBorrar = "El registro se ha borrado con éxito.";
                }

                List<Carteleras> carteleras = carteleraServiceImpl.obtenerCarteleras();
                return View("carteleras", carteleras);
            }
            else
                return RedirectToAction("Login", "home");
        }

        public ActionResult modificarCartelera(int id)
        {
            if (comprobarUsuario("carteleras"))
            {
                List<Sedes> sedes = sedeServiceImpl.obtenerSedes();
                ViewBag.sedes = sedes;

                var peliculas = peliculaServiceImpl.obtenerPeliculas();
                ViewBag.peliculas = peliculas;

                var versiones = versionServiceImpl.obtenerVersiones();
                ViewBag.versiones = versiones;

                Carteleras aModificar = carteleraServiceImpl.buscarPorId(id);

                return View("crearCartelera", aModificar);
            }
            else
                return RedirectToAction("Login", "home");
        }

    }
}