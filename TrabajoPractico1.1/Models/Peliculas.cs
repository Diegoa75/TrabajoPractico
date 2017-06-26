using System;
using System.ComponentModel.DataAnnotations;

namespace TrabajoPractico1._1
{
    [MetadataType(typeof(PeliculasMetadata))]
    public partial class Peliculas
    {
        public class PeliculasMetadata
        {
            [Required(ErrorMessage = "El nombre es requerido")]
            public string Nombre { get; set; }

            [Required(ErrorMessage = "La Descripcion es requerida")]
            public string Descripcion { get; set; }

            [Required(ErrorMessage = "La imagene es requerida")]
            public string Imagen { get; set; }

            [Required(ErrorMessage = "La Clasificacion es requerida")]
            [RegularExpression("^\\d+$", ErrorMessage = "El documento debe contener sólo números.")]
            public int IdCalificacion { get; set; }

            [Required(ErrorMessage = "El Genero es requerido")]
            [RegularExpression("^\\d+$", ErrorMessage = "El documento debe contener sólo números.")]
            public int IdGenero { get; set; }

            [Required(ErrorMessage = "La duracion de la pelicula es un dato requerido")]
            [RegularExpression("^\\d+$", ErrorMessage = "El documento debe contener sólo números.")]
            public int Duracion { get; set; }

            [Required]
            public DateTime FechaCarga { get; set; }
        }
    }
}