using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrabajoPractico1._1.Servicios
{
    public class sInicio
    {
        ContextoPractico ctx = new ContextoPractico();

        public List<Peliculas> listarPeliculas()
        {
            DateTime fechaDesde = DateTime.Now.Date.AddDays(30);
            DateTime fechaHasta = DateTime.Now.Date;
            List<Peliculas> peliculas = (   from ca in ctx.Carteleras
                                            join pe in ctx.Peliculas on ca.IdPelicula equals pe.IdPelicula
                                            where ca.FechaFin >= fechaHasta
                                            && ca.FechaInicio <= fechaDesde
                                            select pe).Distinct().ToList();
            return peliculas;
        }
    }
}