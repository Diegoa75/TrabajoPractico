using System.Collections.Generic;
using System.Web.Mvc;
using TrabajoPractico1._1;
using TrabajoPractico1._1.Servicios;



namespace TrabajoPractico.Controllers
{
    public class homeController : Controller
    {

        public ActionResult Inicio()
        {
            sInicio servicioInicio = new sInicio();
            List<Peliculas> peliculas = servicioInicio.listarPeliculas();

            return View(peliculas);
        }

        public ActionResult Login()
        {
            if (TempData["Error"] != null)
            { 
                ViewBag.Error = TempData["Error"] as string;
            }
            return View();
        }

        public ActionResult cerrarSesion()
        {
            Session.Remove("usuarioEnSesion");
            return RedirectToAction("Inicio");
        }

    }
}