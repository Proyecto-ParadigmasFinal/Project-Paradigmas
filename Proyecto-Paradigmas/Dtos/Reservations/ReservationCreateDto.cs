namespace Proyecto_Paradigmas.Dtos.Reservations
{
    public class ReservationCreateDto
    {
        public string UserId { get; set; }
        public string CatalogItemId { get; set; }
        public DateTime FechaCheckIn { get; set; }
        public DateTime FechaCheckOut { get; set; }
    }
}
