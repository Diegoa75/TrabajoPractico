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
        //
        // GET: /Administrativo/

        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VerificarUsuario([Bind(Include="IdUsuario,NombreUsuario,Password" )]Usuarios usuario)
        {
            if (ModelState.IsValid)
            {
                Usuarios admin = ctx.Usuarios.Where(us => us.NombreUsuario == usuario.NombreUsuario && 
                                                    us.Password == usuario.Password).SingleOrDefault();
                if (admin == null)
                    return View("Error");
                else
                    return View("GestionAdmin");
            }
            else return View("Login");

        }

        public ActionResult GestionAdmin()
        {
            return View();
        }

        public ActionResult GestionSedes()
        {
            List<Sedes> listaSedes = new List<Sedes>();

            listaSedes = (from p in ctx.Sedes
                          select p).ToList();

            return View("GestionSedes", listaSedes);
        }

        public ActionResult CrearNuevaSede()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NuevaSede(Sedes nuevaSede)
        {
            ctx.Sedes.Add(nuevaSede);
            ctx.SaveChanges();
            List<Sedes> listaSedes = new List<Sedes>();
            listaSedes = (from p in ctx.Sedes
                          select p).ToList();
            return View("GestionSedes", listaSedes);
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
            List<Sedes> listaSedes = new List<Sedes>();
            listaSedes = (from p in ctx.Sedes select p).ToList();
            return View("GestionSedes", listaSedes);
        }

        [HttpGet]
        public ActionResult ModificarSedeSeleccionada(int id)
        {
            Sedes sede = new Sedes();
            sede = (from p in ctx.Sedes
                    where (p.IdSede == id)
                    select p).FirstOrDefault();
            return View("ModificarSedeSeleccionada", sede);
        }

      /*  protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ctx.Dispose();
            }
            base.Dispose(disposing);
        }*/
    }
}