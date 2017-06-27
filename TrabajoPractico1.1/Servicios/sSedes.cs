using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TrabajoPractico1._1.ModelViews;


namespace TrabajoPractico1._1.Servicios
{
    public class sSedes
    {
        ContextoPractico ctx = new ContextoPractico();

        public List<Sedes> obtenerSedes()
        {
            return ctx.Sedes.ToList();
        }

        public void guardarSede(Sedes nuevaSede)
        {
            ctx.Sedes.Add(nuevaSede);
            ctx.SaveChanges();
        }

        public Sedes buscarSedePorId(int id)
        {
            Sedes sede = new Sedes();

            sede = ctx.Sedes.Where(p => p.IdSede == id).SingleOrDefault();

            return sede;
        }

        public Sedes buscarSedePorNombreYDireccion(Sedes sedeABuscar)
        {
            Sedes sede = new Sedes();

            sede = ctx.Sedes.Where(s => s.Nombre == sedeABuscar.Nombre
                   && s.Direccion == sedeABuscar.Direccion).FirstOrDefault();

            return sede;
        }

        public Sedes buscarSedeIgualALaModificada(Sedes sedeModificada)
        {
            Sedes sede = new Sedes();

            sede = ctx.Sedes.Where(s => s.Nombre == sedeModificada.Nombre
                     && s.Direccion == sedeModificada.Direccion
                     && s.IdSede != sedeModificada.IdSede).FirstOrDefault();

            return sede;
        }

        public void guardarCambiosEnContexto() 
        {
            ctx.SaveChanges();
        }

    }
}