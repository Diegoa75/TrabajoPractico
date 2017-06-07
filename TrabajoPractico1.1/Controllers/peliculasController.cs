
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrabajoPractico.Models;
using TrabajoPractico1._1;


namespace TrabajoPractico.Controllers
{
    public class peliculasController : Controller
    {
        //
        // GET: /peliculas/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult reserva(FormCollection f)
        {
            ContextoPractico ctx = new ContextoPractico();
            Usuarios myUsuario = new Usuarios();
           myUsuario.NombreUsuario = f["NombreUsuario"];
           myUsuario.Password = f["Password"];
            ctx.Usuarios.Add(myUsuario);
            ctx.SaveChanges();
            return View();
        }

    }
}
