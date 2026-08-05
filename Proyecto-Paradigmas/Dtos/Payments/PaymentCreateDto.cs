namespace Proyecto_Paradigmas.Dtos.Payments
{
    public class PaymentCreateDto
    {
        public string ReservationId { get; set; }
        public decimal PrecioTotal { get; set; }
    }
}