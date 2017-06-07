using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrabajoPractico.Models;

namespace TrabajoPractico.Controllers
{
    public class homeController : Controller
    {
        //
        // GET: /home/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Inicio()
        {
            Pelicula peli = new Pelicula();
            peli.nombre = "The Avengers";
            return View(peli);
        }

        public ActionResult clienteincorrecto()
        {
            return View();
        }

       /* [HttpPost]
        public ActionResult inicio(int dni, string email)
        {
            Cliente cli = new Cliente();
            if ((cli.dni != dni) || (cli.email != email))
                return View("clienteincorrecto");
            else
                return View("inicio");
        }*/

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult registrar()
        {
            Pelicula peli = new Pelicula();
            peli.nombre = "The Avengers";
            return View(peli);
        }

    }
}
