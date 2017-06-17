using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrabajoPractico.Models;
using TrabajoPractico1._1;
using System.Data.Entity;
using System.Net;

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
            ViewBag.GeneroId = new SelectList(ctx.Generos, "IdGenero", "Nombre");
            ViewBag.CalificacionId = new SelectList(ctx.Calificaciones, "IdCalificacion", "Nombre");
            return View();

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