using System.ComponentModel.DataAnnotations;

namespace ProyectoParadigmas.Dtos.CatalogItems
{
    public class CatalogItemCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        [Required]
        public decimal Precio { get; set; }

        [StringLength(255)]
        public string ImagenUrl { get; set; }

        [Required]
        [StringLength(30)]
        public string Tipo { get; set; }

        public int CapacidadPersonas { get; set; }

        [StringLength(20)]
        public string NumeroHabitacion { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; }
    }
}