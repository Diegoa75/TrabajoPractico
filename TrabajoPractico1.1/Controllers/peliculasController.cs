using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAL;
using DAL.ModelViews;
using DAL.Servicios;

namespace TrabajoPractico.Controllers
{
    public class peliculasController : Controller
    {
        ContextoPractico ctx = new ContextoPractico();
        // GET: /peliculas/

        public ActionResult reserva(int id)
        {
            sReservas reservaServ = new sReservas();
            vReserva reserva = new vReserva();

            reserva.IdPelicula = id;
            reserva = reservaServ.cargarVersiones(reserva);

            return View(reserva);
        }

        [HttpPost]
        public ActionResult reserva(FormCollection form, vReserva myReserva)
        {
            sReservas reservaServ = new sReservas();

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
            sPeliculas sConfirma = new sPeliculas();
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
                sPeliculas confirmarServ = new sPeliculas();

                myReserva = confirmarServ.completarReserva(Session["reserva"] as Reservas, myReserva);

                Session.Remove("reserva");

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
