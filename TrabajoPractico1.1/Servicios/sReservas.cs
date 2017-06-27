using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrabajoPractico1._1.ModelViews;

namespace TrabajoPractico1._1.Servicios
{
    
    public class sReservas
    {
        ContextoPractico ctx = new ContextoPractico();
        private DateTime fechaDesde = DateTime.Now.Date.AddDays(30);
        private DateTime fechaHasta = DateTime.Now.Date;

        public vReserva cargarVersiones(vReserva reserva)
        {//Busca las posibles versiones de la pelicula seleccionada
            reserva.Versiones = (from ve in ctx.Versiones
                                 join ca in ctx.Carteleras on ve.IdVersion equals ca.IdVersion
                                 where ca.IdPelicula == reserva.IdPelicula
                                    && ca.FechaFin >= fechaHasta
                                     && ca.FechaInicio <= fechaDesde
                                 select ve).Distinct().ToList();
            return reserva;
        }

        public vReserva cargarSedes(vReserva reserva)
        {//Busca las Sedes que tienen la pelicula elegida en la version seleccionada
            if (reserva.idVersion > 0)
            {   
                /*reserva.Sedes = ctx.Sedes
                                    .Join(ctx.Carteleras,
                                        se => se.IdSede,
                                        ca => ca.IdSede,
                                        (se, ca) => new { Sedes = se, Carteleras = ca })
                                    .Where(seca => seca.Carteleras.IdPelicula == reserva.IdPelicula
                                        && seca.Carteleras.IdVersion == reserva.idVersion)
                                    .Select(se=>se.Sedes)
                                    .Distinct()
                                    .ToList();*/

                reserva.Sedes = (from se in ctx.Sedes
                                 join ca in ctx.Carteleras on se.IdSede equals ca.IdSede
                                 where ca.IdPelicula == reserva.IdPelicula
                                    && ca.IdVersion == reserva.idVersion
                                    && ca.FechaFin >= fechaHasta
                                    && ca.FechaInicio <= fechaDesde
                                 select se).Distinct().ToList();
            }
            return reserva;
        }
        public vReserva cargarDias(vReserva reserva)
        {//Busca los dias disponibles para la pelicula elegida en la version y sede seleccionada
            DateTime Inicio;
            int dia;
            if (reserva.idSede > 0)
            {
                var Datos = (from ca in ctx.Carteleras
                             where ca.IdPelicula == reserva.IdPelicula
                                     && ca.IdVersion == reserva.idVersion
                                     && ca.IdSede == reserva.idSede
                             select new
                             {
                                 Inicio = ca.FechaInicio,
                                 Fin = ca.FechaFin,
                                 Lunes = ca.Lunes,
                                 Martes = ca.Martes,
                                 Miercoles = ca.Miercoles,
                                 Jueves = ca.Jueves,
                                 Viernes = ca.Viernes,
                                 Sabado = ca.Sabado,
                                 Domingo = ca.Domingo
                             }).FirstOrDefault();
                if (Datos != null)
                {//crea un array con True en los dias que se presenta la pelicula
                    bool[] Dias = { Datos.Domingo,Datos.Lunes,Datos.Martes,Datos.Miercoles
                                   ,Datos.Jueves,Datos.Viernes,Datos.Sabado };
                    //si la fecha de inicio es menor a la actual, se utiliza la actual
                    if (Datos.Inicio >= DateTime.Now.Date)
                    {
                        Inicio = Datos.Inicio;
                    }
                    else
                    {
                        Inicio = DateTime.Now.Date;
                    }
                    //recorre los dias restantes en cartelera y lista en caso de que ese dia se presente
                    while (Inicio <= Datos.Fin)
                    {
                        dia = (int)Inicio.DayOfWeek;
                        if (Dias[dia] == true)
                        {
                            reserva.Dias.Add(Inicio.ToString("dddd") + " " + Inicio.ToString("dd-MM-yyyy"));
                        }
                        Inicio = Inicio.AddDays(1);
                    }
                }
                else
                {
                    reserva.idSede = 0;
                    reserva.Dia = null;
                }
            }
            return reserva;
        }
        public vReserva cargarHorarios(vReserva reserva)
        {
            if (reserva.Dia != null)
            {
                DateTime hora;
                var Datos = (from pe in ctx.Peliculas
                             join ca in ctx.Carteleras on pe.IdPelicula equals ca.IdPelicula
                             where pe.IdPelicula == reserva.IdPelicula
                                && ca.IdVersion == reserva.idVersion
                                && ca.IdSede == reserva.idSede
                             select new
                             {
                                 HoraInicio = ca.HoraInicio,
                                 Duracion = pe.Duracion
                             }).FirstOrDefault();
                if (Datos != null)
                {
                    int dia = Convert.ToInt32(reserva.Dia.Substring(reserva.Dia.Length - 10, 2));
                    int mes = Convert.ToInt32(reserva.Dia.Substring(reserva.Dia.Length - 7, 2));
                    int anio = Convert.ToInt32(reserva.Dia.Substring(reserva.Dia.Length - 4, 4));
                    hora = new DateTime(anio, mes, dia, Datos.HoraInicio, 0, 0);
                    for (int i = 0; i < 7; i++)
                    {
                        reserva.Horarios.Add(hora.ToString("hh:mm"));
                        hora = hora.AddMinutes(Datos.Duracion + 30);
                    }
                }
                else
                {
                    reserva.Dia = null;
                }
            }
            return reserva;
        }
        public Reservas crearSesionReserva(vReserva reserva)
        {
            Reservas reservaConfirmacion = new Reservas();
            reservaConfirmacion.IdPelicula = reserva.IdPelicula;
            reservaConfirmacion.IdSede = Convert.ToInt32(reserva.idSede);
            reservaConfirmacion.IdVersion = Convert.ToInt32(reserva.idVersion);
            reservaConfirmacion.FechaHoraInicio = Convert.ToDateTime(reserva.FechaHora);

            return reservaConfirmacion;
        }
    }
}