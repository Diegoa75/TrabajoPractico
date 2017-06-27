using System.ComponentModel.DataAnnotations;

namespace DAL
{
    [MetadataType(typeof(UsuariosMetadata))]
    public partial class Usuarios
    {
        public class UsuariosMetadata
        {
            [Required(ErrorMessage = "Debe ingresar un nombre de usuario.")]
            [StringLength(50, ErrorMessage = "El nombre excede el limite de caracteres.")]
            public string NombreUsuario { get; set; }

            [Required(ErrorMessage = "Debe ingresar una contraseña.")]
            [StringLength(50, ErrorMessage = "La contraseña ingresada es demasiado larga")]
            public string Password { get; set; }

        }
    }
}