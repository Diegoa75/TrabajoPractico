using System.ComponentModel.DataAnnotations;

namespace TrabajoPractico1._1
{
    [MetadataType(typeof(UsuariosMetadata))]
    public partial class Usuarios
    {
        public class UsuariosMetadata
        {
            [Required(ErrorMessage = "Debe ingresar un nombre de usuario")]
            public string NombreUsuario { get; set; }

            [Required(ErrorMessage = "Debe ingresar una contraseña")]
            public string Password { get; set; }

        }
    }
}