using System.ComponentModel.DataAnnotations;

namespace TrabajoPractico1._1
{
    [MetadataType(typeof(ReservasMetadata))]
    public partial class Reservas
    {
        public class ReservasMetadata
        {
            [Required]
            public int IdSede { get; set; }

            [Required]
            public int IdVersion { get; set; }

            [Required]
            public int IdPelicula { get; set; }

            [Required]
            public System.DateTime FechaHoraInicio { get; set; }

            [EmailAddress(ErrorMessage = "El Email ingresado no es válido")]
            [Required(ErrorMessage = "El Email es un dato requerido")]
            public string Email { get; set; }

            [Required(ErrorMessage = "El Tipo de documento es un dato requerido")]
            public int IdTipoDocumento { get; set; }

            [RegularExpression("^\\d+$", ErrorMessage = "El documento debe contener sólo números.")]
            [StringLength(10, ErrorMessage = "El número de documento no puede exceder los 10 caracteres")]
            [Required(ErrorMessage = "Debe ingresar su número de documento")]
            public string NumeroDocumento { get; set; }
            
            [RegularExpression("^\\d+$", ErrorMessage = "Las entradas deben contener sólo números.")]
            [Required(ErrorMessage = "Debe ingresar la cantidad de entradas deseadas")]
            [Range(1, 20, ErrorMessage = "puede reservar hasta 20 entradas")]
            public int CantidadEntradas { get; set; }

            [Required]
            public System.DateTime FechaCarga { get; set; }
        }
    }
}