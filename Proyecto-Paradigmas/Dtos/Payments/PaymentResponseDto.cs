namespace Proyecto_Paradigmas.Dtos.Payments
{
    public class PaymentResponseDto
    {
        public string OrderId { get; set; } = string.Empty;
        public string ApprovalLink { get; set; } = string.Empty;
    }
}