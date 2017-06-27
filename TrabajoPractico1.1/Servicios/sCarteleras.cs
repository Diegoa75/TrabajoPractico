using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrabajoPractico1._1.ModelViews;


namespace TrabajoPractico1._1.Servicios
{
    public class sCarteleras
    {
        ContextoPractico ctx = new ContextoPractico();

        public List<Carteleras> obtenerCarteleras()
        {
					return ctx.Carteleras.Include("Sedes").Include("Peliculas").ToList();
        }

        public void agregarCartelera(Carteleras nuevaCartelera)
        {
            ctx.Carteleras.Add(nuevaCartelera);
            ctx.SaveChanges();
        }

        public void eliminarCartelera(Carteleras aBorrar)
        {
            ctx.Carteleras.Remove(aBorrar);
            ctx.SaveChanges();
        }

        public Carteleras buscarPorId(int id)
        {
            Carteleras cartelera = new Carteleras();

            cartelera = (from c in ctx.Carteleras
                         where c.IdCartelera == id
                         select c).FirstOrDefault();

            return cartelera;
        }

        public Carteleras buscarPorSedePeliculaYVersion(Carteleras carteleraABuscar)
        {
            Carteleras cartelera = new Carteleras();

            cartelera= (from _c in ctx.Carteleras
                        where _c.IdSede     == carteleraABuscar.IdSede
                        && _c.IdPelicula    == carteleraABuscar.IdPelicula
                        && _c.IdVersion     == carteleraABuscar.IdVersion
                        select _c).FirstOrDefault();

            return cartelera;
        }

        public List<Carteleras> buscarPorFechaDiasYSalas(Carteleras c)
        {
            List<Carteleras> carteleras = new List<Carteleras>();

            carteleras = (from _c in ctx.Carteleras
                          where _c.IdSede == c.IdSede
                          && _c.NumeroSala == c.NumeroSala
                          && ((c.FechaInicio >= _c.FechaInicio && c.FechaInicio <= _c.FechaFin)
                          || (c.FechaFin >= _c.FechaInicio && c.FechaFin <= _c.FechaFin))
                          && (c.Lunes == _c.Lunes
                          || c.Martes == _c.Martes
                          || c.Miercoles == _c.Miercoles
                          || c.Jueves == _c.Jueves
                          || c.Viernes == _c.Viernes
                          || c.Sabado == _c.Sabado
                          || c.Domingo == _c.Domingo)
                          select _c).ToList();

            return carteleras;
        }

        public List<Carteleras> buscarPorSede(Carteleras carteleraABuscar)
        {
            List<Carteleras> carteleras = new List<Carteleras>();

            carteleras = (from _c in ctx.Carteleras
                          where _c.IdSede == carteleraABuscar.IdSede
                         select _c).ToList();

            return carteleras;
        }
    }
}