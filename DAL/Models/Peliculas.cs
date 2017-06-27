using System;
using System.ComponentModel.DataAnnotations;

namespace DAL
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
            public int IdCalificacion { get; set; }

            [Required(ErrorMessage = "El Genero es requerido")]
            public int IdGenero { get; set; }

            [Required(ErrorMessage = "La duracion de la pelicula es un dato requerido")]
            [Range(1, 90, ErrorMessage = "La duración de la Pelicula no es válida")]
            public int Duracion { get; set; }

            [Required]
            public DateTime FechaCarga { get; set; }
        }
    }
}