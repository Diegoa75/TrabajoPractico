using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL.ModelViews;


namespace DAL.Servicios
{
    public class sCalificaciones
    {
        ContextoPractico ctx = new ContextoPractico();

        public List<Calificaciones> obtenerCalificaciones()
        {
            return ctx.Calificaciones.ToList();
        }

    }
}