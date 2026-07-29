namespace Proyecto_Paradigmas.Dtos.Payments
{
    public class PaymentCreateDto
    {
        public int ReservationId { get; set; }
        public decimal TotalAmount { get; set; }
    }
}