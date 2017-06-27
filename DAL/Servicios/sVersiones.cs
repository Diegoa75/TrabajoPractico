using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL.ModelViews;


namespace DAL.Servicios
{
    public class sVersiones
    {
        ContextoPractico ctx = new ContextoPractico();

        public List<Versiones> obtenerVersiones()
        {
            return ctx.Versiones.ToList();
        }

    }
}