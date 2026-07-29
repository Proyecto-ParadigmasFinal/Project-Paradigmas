using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persons.API.Constants;
using Proyecto_Paradigmas.Dtos.Payments;
using Proyecto_Paradigmas.Services;
using Proyecto_Paradigmas.Services.Interfaces;
using ProyectoParadigmas.Database;
using ProyectoParadigmas.Entities;

namespace Proyecto_Paradigmas.Controllers
{
    [Route("api/payment")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaypalServices _paypalService;
        private readonly HotelDbContext _context;

        public PaymentsController(IPaypalServices paypalService, HotelDbContext context)
        {
            _paypalService = paypalService;
            _context = context;
        }

        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder([FromBody] PaymentCreateDto request)
        {
            try
            {
                // Consulta directa mediante el DbSet configurado en HotelDbContext
                var reservation = await _context.Reservations
                                                .FirstOrDefaultAsync(r => r.Id == request.ReservationId);

                if (reservation == null)
                    return StatusCode(HttpStatusCode.NOT_FOUND, new { message = "Reserva no encontrada." });

                var result = await _paypalService.CreateOrderAsync(request);

                reservation.PayPalOrderId = result.OrderId;
                await _context.SaveChangesAsync();

                return StatusCode(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.INTERNAL_SERVER_ERROR, new { message = $"Error proveedor pago: {ex.Message}" });
            }
        }

        [HttpPost("capture-order/{orderId}")]
        public async Task<IActionResult> CaptureOrder(string orderId)
        {
            try
            {
                var isCaptured = await _paypalService.CaptureOrderAsync(orderId);

                if (!isCaptured)
                    return StatusCode(HttpStatusCode.BAD_REQUEST, new { message = "Pago denegado por proveedor." });

                var reservation = await _context.Reservations
                                                .FirstOrDefaultAsync(r => r.PayPalOrderId == orderId);

                if (reservation != null)
                {
                    reservation.EstadoTransaccion = "Completado";
                    await _context.SaveChangesAsync();
                }

                return StatusCode(HttpStatusCode.OK, new { message = "Transacción procesada correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.INTERNAL_SERVER_ERROR, new { message = $"Error actualización pago: {ex.Message}" });
            }
        }
    }
}
