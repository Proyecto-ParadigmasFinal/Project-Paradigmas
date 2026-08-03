using Proyecto_Paradigmas.Dtos.Payments;

namespace Proyecto_Paradigmas.Services.Interfaces
{
    public interface IPaypalServices
    {
        Task<PaymentResponseDto> CreateOrderAsync(PaymentCreateDto request);
        Task<bool> CaptureOrderAsync(string orderId);
    }
}
