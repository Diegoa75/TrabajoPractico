using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrabajoPractico1._1.ModelViews;

namespace TrabajoPractico1._1.Servicios
{
    public class sConfirmarPelicula
    {
        ContextoPractico ctx = new ContextoPractico();

        public Reservas completarReserva(Reservas sesion, Reservas reserva )
        {
            reserva.IdPelicula = sesion.IdPelicula;
            reserva.IdSede = sesion.IdSede;
            reserva.IdVersion = sesion.IdVersion;
            reserva.FechaHoraInicio = sesion.FechaHoraInicio;
            reserva.FechaCarga = DateTime.Now;

            ctx.Reservas.Add(reserva);
            ctx.SaveChanges();

            return reserva;
        }
    }
}