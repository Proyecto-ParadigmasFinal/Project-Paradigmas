using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Paradigmas.Dtos.Payments;
using Proyecto_Paradigmas.Services;
using Proyecto_Paradigmas.Services.Interfaces;
using ProyectoParadigmas.Database;
using ProyectoParadigmas.Entities;

namespace Proyecto_Paradigmas.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaypalServices _payPalService;
        private readonly HotelDbContext _context;

        public PaymentsController(IPaypalServices payPalService, HotelDbContext context)
        {
            _payPalService = payPalService;
            _context = context;
        }

        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder([FromBody] PaymentCreateDto request)
        {
            try
            {
                // Resolución de tipo mediante using directo, evitando rutas absolutas erróneas
                var reservation = await _context.Set<ReservationEntity>()
                                                .FirstOrDefaultAsync(r => r.Id == request.ReservationId);

                if (reservation == null) return NotFound("Reserva no encontrada.");

                var result = await _paypalService.CreateOrderAsync(request);

                reservation.PayPalOrderId = result.OrderId;
                await _context.SaveChangesAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error proveedor pago: {ex.Message}");
            }
        }

        [HttpPost("capture-order/{orderId}")]
        public async Task<IActionResult> CaptureOrder(string orderId)
        {
            try
            {
                var isCaptured = await _payPalService.CaptureOrderAsync(orderId);

                if (!isCaptured) return BadRequest("El pago no pudo ser procesado o fue rechazado.");

                var reservation = await _context.Set<ProyectoParadigmas.Database.Entities.ReservationEntity>()
                                                .FirstOrDefaultAsync(r => r.PayPalOrderId == orderId);

                if (reservation != null)
                {
                    reservation.IsPaid = true; reservation.OrderId = orderId;
                    await _context.SaveChangesAsync();
                }

                return Ok(new { message = "Pago completado exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al capturar el pago: {ex.Message}");
            }
        }
    }
}
