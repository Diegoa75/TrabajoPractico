using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Models.Servicios
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

        public Usuarios buscarUsuarioPorNombre(string usuario)
        {
            Usuarios usuarioEncontrado = new Usuarios();

            usuarioEncontrado = ctx.Usuarios.Where(u => u.NombreUsuario == usuario).SingleOrDefault();

            return usuarioEncontrado;
        }

    }
}