﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrabajoPractico1._1.ModelViews;


namespace TrabajoPractico1._1.Servicios
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