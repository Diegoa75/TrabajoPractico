using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL.ModelViews;


namespace DAL.Servicios
{
    public class sGeneros
    {
        ContextoPractico ctx = new ContextoPractico();

        public List<Generos> obtenerGeneros()
        {
            return ctx.Generos.ToList();
        }

    }
}