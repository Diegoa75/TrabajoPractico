using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrabajoPractico1._1.ModelViews;

namespace TrabajoPractico1._1.Servicios
{
    public class sReportes
    {
        ContextoPractico ctx = new ContextoPractico();

        public List<Reservas> buscarReservasEntreFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            List<Reservas> reservas = new List<Reservas>();

            reservas = (from r in ctx.Reservas
                        where (fechaFin >= r.FechaCarga && r.FechaCarga >= fechaInicio)
                        select r).ToList();

            return reservas;
        }

        public String validarReservas(TimeSpan ts)
        {
            if (ts.Days < 0)
            {
                return "La fecha de inicio no puede ser mayor a la de fin";
            }

            if (ts.Days > 30)
            {
                return "No puede haber una diferencia mayor a 30 dias entre las fechas";
            }

            return null;
        }

    }

}