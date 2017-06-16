using System.Collections.Generic;
using System.Web.Mvc;
using TrabajoPractico1._1;
using TrabajoPractico1._1.Servicios;



namespace TrabajoPractico.Controllers
{
    public class homeController : Controller
    {
        ContextoPractico ctx = new ContextoPractico();
        // GET: /home/

        public ActionResult Inicio()
        {
            sInicio inicio = new sInicio();
            List<TrabajoPractico1._1.Peliculas> peliculas = inicio.listarPeliculas();

            return View(peliculas);
        }

        public ActionResult clienteincorrecto()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

    }
}