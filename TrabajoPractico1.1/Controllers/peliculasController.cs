using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrabajoPractico1._1;
using TrabajoPractico1._1.ModelViews;
using TrabajoPractico1._1.Servicios;

namespace TrabajoPractico.Controllers
{
    public class peliculasController : Controller
    {
        ContextoPractico ctx = new ContextoPractico();
        // GET: /peliculas/

        public ActionResult reserva(int id)
        {
            sRegistrar reservaServ = new sRegistrar();
            vReserva reserva = new vReserva();

            reserva.IdPelicula = id;
            reserva = reservaServ.cargarVersiones(reserva);

            return View(reserva);
        }

        [HttpPost]
        public ActionResult reserva(FormCollection form, vReserva myReserva)
        {
            sRegistrar reservaServ = new sRegistrar();

            myReserva = reservaServ.cargarVersiones(myReserva);
            myReserva = reservaServ.cargarSedes(myReserva);
            myReserva = reservaServ.cargarDias(myReserva);
            myReserva = reservaServ.cargarHorarios(myReserva);
            //verifica que este cargada la funcion y se haya apretado el submit
            if (Request.Form["btnComprar"] != null && myReserva.FechaHora != null)
            {
                Reservas confirmacion = reservaServ.crearSesionReserva(myReserva);
                Session["reserva"] = confirmacion;
                return RedirectToAction("confirmar");
            }
            return View(myReserva);
        }

        public ActionResult confirmar()
        {
            sConfirmar sConfirma = new sConfirmar();
            Reservas sesion = Session["reserva"] as Reservas;

            ViewBag.confirmacion = sConfirma.llenarVista(sesion);
            ViewBag.TiposDocumentos = ctx.TiposDocumentos.ToList();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult confirmarPelicula(Reservas myReserva)
        {
            if (ModelState.IsValid)
            {
                sConfirmarPelicula confirmarServ = new sConfirmarPelicula();

                myReserva = confirmarServ.completarReserva(Session["reserva"] as Reservas, myReserva);

                Session["products"] = null;

                var Datos = ctx.Sedes.Where(x => x.IdSede == myReserva.IdSede).FirstOrDefault();

                ViewBag.CodigoReserva = myReserva.IdReserva;
                ViewBag.Precio = "$" + (Datos.PrecioGeneral * myReserva.CantidadEntradas).ToString();
                ViewBag.CantidadEntradas = myReserva.CantidadEntradas;

                return View("confirmar");
            }
            return RedirectToAction("Inicio", "home"); ;
        }

    }
}
