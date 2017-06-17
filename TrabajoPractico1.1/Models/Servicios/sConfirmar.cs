using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrabajoPractico1._1.ModelViews;

namespace TrabajoPractico1._1.Servicios
{
    public class sConfirmar
    {
        ContextoPractico ctx = new ContextoPractico();

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

    }
}