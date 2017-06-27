using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using TrabajoPractico1._1.Servicios;

namespace TrabajoPractico1._1.Models.Servicios
{
    public class sImagenes
    {
        public static string Guardar(HttpPostedFileBase archivoSubido, string nombrePelicula)
        {
            string carpetaImagenes = System.Configuration.ConfigurationManager.AppSettings["CarpetaImagenes"];

            if (string.IsNullOrEmpty(carpetaImagenes))
            {
                throw new Exception("En el archivo web.config debe agregar dentro de <appSettings> el elemento <add key=\"CarpetaImagenes\" value=\"/Imagenes/\" />");
            }

            //garantizamos que no importa si el valor en el web.config empieza/termina con /, nosotros le ponemos que empiece y termine con /
            carpetaImagenes = string.Format("/{0}/", carpetaImagenes.TrimStart('/').TrimEnd('/'));

            //Server.MapPath antepone a un string la ruta fisica donde actualmente esta corriendo la aplicacion (ej. c:\inetpub\misitio\)
            string pathDestino = System.Web.Hosting.HostingEnvironment.MapPath("~") + carpetaImagenes;

            //si no exise la carpeta, la creamos
            if (!Directory.Exists(pathDestino))
            {
                Directory.CreateDirectory(pathDestino);
            }

            string nombreArchivoFinal = GenerarNombreUnico(nombrePelicula);
            nombreArchivoFinal = string.Concat(nombreArchivoFinal, Path.GetExtension(archivoSubido.FileName));

            //para guardar en el disco rigido, se guarda con el path absoluto
            archivoSubido.SaveAs(string.Concat(pathDestino, nombreArchivoFinal));

            //retornamos el path relativo desde la raiz del sitio
            return string.Concat(carpetaImagenes, nombreArchivoFinal);
        }

        private static string GenerarNombreUnico(string nombrePelicula)
        {
            //Genero un string random de 20 caracteres para asegurar un nombre unico y que no se pisen archivos inesperadamente
            //System.Web.Security.Membership.GeneratePassword(int length, int numberOfNonAlphanumericCharacters)
            string randomString = System.Web.Security.Membership.GeneratePassword(20, 0);
            Random rnd = new Random();

            //removiendo espacios y caracteres raros del string random 
            randomString = Regex.Replace(randomString, @"[^a-zA-Z0-9]", m => "");

            //removiendo espacios y caracteres raros del nombre 
            nombrePelicula = Regex.Replace(nombrePelicula.Trim(), @"[^a-zA-Z0-9]", m => "").ToLower();

            //{Nombre,8 carac}-{Random,5 carac}
            return string.Format("{0}-{1}", sString.Truncar(nombrePelicula, 8), sString.Truncar(randomString, 5));
        }

        public static void Borrar(string pathGuardado)
        {
            //si el path es relativo, se le agrega el mapeo completo para que sea absoluto
            //y pasar de /temp/imagen.jpg a c:\inetpub\temp\imagen.jpg por ejemplo
            if (!Path.GetPathRoot(pathGuardado).Contains(":"))
            {
                //Alternativa a Server.MapPath(
                pathGuardado = System.Web.Hosting.HostingEnvironment.MapPath("~") + pathGuardado;
            }

            if (System.IO.File.Exists(pathGuardado))
            {
                System.IO.File.Delete(pathGuardado);
            }
        }
    }
}