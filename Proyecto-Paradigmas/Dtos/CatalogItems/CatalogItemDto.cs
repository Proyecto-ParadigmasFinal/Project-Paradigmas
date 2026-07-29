namespace ProyectoParadigmas.Dtos.CatalogItems
{
    public class CatalogItemDto
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public string ImagenUrl { get; set; }
        public string Tipo { get; set; }
        public int CapacidadPersonas { get; set; }
        public string NumeroHabitacion { get; set; }
        public string Estado { get; set; }
    }
}