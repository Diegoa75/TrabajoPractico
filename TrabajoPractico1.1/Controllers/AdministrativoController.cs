﻿using System.Linq;
using System.Web.Mvc;
using TrabajoPractico1._1.Models.Servicios;

namespace TrabajoPractico1._1.Controllers
{
    public class administrativoController : Controller
    {
        ContextoPractico ctx = new ContextoPractico();
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
                Usuarios admin = ctx.Usuarios.Where(us => us.NombreUsuario == usuario.NombreUsuario &&
                                                    us.Password == usuario.Password).SingleOrDefault();
                if (admin != null)
                    return RedirectToAction("Inicio");
                else
                    TempData["Error"] = "Error de usuario y/o contraseña";
            }
            return RedirectToAction("Login", "home");

        }
    }
}