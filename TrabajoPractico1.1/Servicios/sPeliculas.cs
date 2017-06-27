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

        public List<Peliculas> obtenerPeliculas()
        {
            List<Peliculas> peliculas = new List<Peliculas>();

            peliculas = ctx.Peliculas.ToList();

            return peliculas;
        }

        public List<Peliculas> obtenerPeliculasYGeneros()
        {
            List<Peliculas> peliculas = new List<Peliculas>();

            peliculas = ctx.Peliculas.Include("Generos").ToList();

            return peliculas;
        }

        public Peliculas obtenerPeliculaPorId(int id)
        {
            Peliculas pelicula = new Peliculas();

            pelicula = (from peli in ctx.Peliculas
                         where id == peli.IdPelicula
                         select peli).FirstOrDefault();

            return pelicula;
        }

        public Peliculas buscarPeliculaConMismoNombre(Peliculas pelicula)
        {
            Peliculas peliculaEncontrada = new Peliculas();

            peliculaEncontrada = ctx.Peliculas.Where(p => p.Nombre == pelicula.Nombre
                                                && p.IdPelicula != pelicula.IdPelicula).FirstOrDefault();

            return peliculaEncontrada;
        }

        public void guardarPelicula(Peliculas nuevaPelicula)
        {
            ctx.Peliculas.Add(nuevaPelicula);
            ctx.SaveChanges();
        }

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

        public void guardarContexto()
        {
            ctx.SaveChanges();
        }
    }
}