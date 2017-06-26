using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace TrabajoPractico1._1.Models.Servicios
{
    public class sUsuarios
    {
        ContextoPractico ctx = new ContextoPractico();

        public Usuarios logearUsuario(Usuarios usuario)
        {
            Usuarios admin;
            try
            {
                admin = ctx.Usuarios.Where(us => us.NombreUsuario == usuario.NombreUsuario
                                 && us.Password == usuario.Password).SingleOrDefault();
            }
            catch (Exception) 
            {
                admin = null;
            }
            return admin;
        }


    }
}