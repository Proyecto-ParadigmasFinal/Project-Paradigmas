using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Persons.API.Database.Entities.Common;

namespace ProyectoParadigmas.Entities
{
    [Table("catalog_items")]
    public class CatalogItemEntity : BaseEntity
    {
        [Required]
        [StringLength(100)]
        [Column("nombre")]
        public string Nombre { get; set; }

        [Column("descripcion", TypeName = "text")]
        public string Descripcion { get; set; }

        [Required]
        [Column("precio")]
        public decimal Precio { get; set; }

        [StringLength(255)]
        [Column("imagen_url")]
        public string ImagenUrl { get; set; }

        [Required]
        [StringLength(30)]
        [Column("tipo")]
        public string Tipo { get; set; } 

        [Column("capacidad_personas")]
        public int CapacidadPersonas { get; set; }

        [StringLength(20)]
        [Column("numero_habitacion")]
        public string NumeroHabitacion { get; set; }

        [Required]
        [StringLength(20)]
        [Column("estado")]
        public string Estado { get; set; } 
    }
}