using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrabajoPractico.Models;
using TrabajoPractico1._1;

namespace TrabajoPractico1._1.Controllers
{
    public class administrativoController : Controller
    {
        //
        // GET: /Administrativo/

        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult VerificarUsuario(Usuarios usuario)
        {
            ContextoPractico ctx = new ContextoPractico();
            var admin = (from p in ctx.Usuarios
                         where (p.NombreUsuario == usuario.NombreUsuario) && (p.Password == usuario.Password)
                        select p).FirstOrDefault();
            if (admin == null)
                return View("Error");
            else
                return View("GestionAdmin");
        }
    }
}