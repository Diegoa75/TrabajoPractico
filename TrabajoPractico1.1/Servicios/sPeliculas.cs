using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrabajoPractico1._1.ModelViews;

namespace TrabajoPractico1._1.Servicios
{
    public class sPeliculas
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

        public vConfirmacion llenarVista(Reservas reserva)
        {
            vConfirmacion confirmacion = new vConfirmacion();

            confirmacion.FechaHoraInicio = reserva.FechaHoraInicio;
            confirmacion.cartelera = ctx.Carteleras.Where(x => x.IdPelicula == reserva.IdPelicula
                                                          && x.IdSede == reserva.IdSede
                                                          && x.IdVersion == reserva.IdVersion).FirstOrDefault();

            confirmacion.pelicula = ctx.Peliculas.Where(x => x.IdPelicula
                                                            == reserva.IdPelicula).FirstOrDefault();

            return confirmacion;
        }

        public List<Peliculas> obtenerPeliculas()
        {
            List<Peliculas> peliculas = new List<Peliculas>();

            peliculas = ctx.Peliculas.ToList();

            return peliculas;
        }

    }
}