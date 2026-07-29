using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Paradigmas.Database;
using Proyecto_Paradigmas.Dtos.Payments;
using Proyecto_Paradigmas.Services.Interfaces;
using ProyectoParadigmas.Database;

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
                //Validar que la reserva existe en la BD
                var reservation = await _context.Set<Proyecto_Paradigmas.Database.Entities.ReservationEntity>()
                                                .FirstOrDefaultAsync(r => r.Id == request.ReservationId);

                if (reservation == null) return NotFound("Reserva no encontrada.");
                var result = await _payPalService.CreateOrderAsync(request);
                reservation.PayPalOrderId = result.OrderId;
                await _context.SaveChangesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno al conectar con PayPal: {ex.Message}");
            }
        }

        [HttpPost("capture-order/{orderId}")]
        public async Task<IActionResult> CaptureOrder(string orderId)
        {
            try
            {
                var isCaptured = await _payPalService.CaptureOrderAsync(orderId);

                if (!isCaptured) return BadRequest("El pago no pudo ser procesado o fue rechazado.");

                var reservation = await _context.Set<Proyecto_Paradigmas.Database.Entities.ReservationEntity>()
                                                .FirstOrDefaultAsync(r => r.PayPalOrderId == orderId);

                if (reservation != null)
                {
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
