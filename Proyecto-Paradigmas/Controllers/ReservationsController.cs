using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persons.API.Constants;
using Proyecto_Paradigmas.Dtos.Reservations;
using ProyectoParadigmas.Database;
using ProyectoParadigmas.Entities;
using System.Security.Claims;

namespace Proyecto_Paradigmas.Controllers
{
    [Route("api/reservations")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly HotelDbContext _context;

        public ReservationsController(HotelDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation([FromBody] ReservationCreateDto request)
        {
            try
            {
                var room = await _context.CatalogItems.FindAsync(request.CatalogItemId);

                if (room == null)
                    return StatusCode(HttpStatusCode.NOT_FOUND, new { message = "Habitación/Paquete no encontrado." });

                if (room.Estado != "Disponible")
                    return StatusCode(HttpStatusCode.BAD_REQUEST, new { message = "La habitación no está disponible." });

                // Cálculo de días y monto total
                var days = (request.FechaCheckOut.Date - request.FechaCheckIn.Date).Days;
                if (days <= 0) days = 1;

                var totalAmount = room.Precio * days;

                var newReservation = new ReservationEntity
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = request.UserId,
                    CatalogItemId = request.CatalogItemId,
                    FechaCheckIn = request.FechaCheckIn,
                    FechaCheckOut = request.FechaCheckOut,
                    MontoTotal = totalAmount,
                    EstadoTransaccion = "Pendiente",
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    UpdatedBy = "System",
                    UpdatedDate = DateTime.UtcNow
                };

                _context.Reservations.Add(newReservation);
                room.Estado = "Ocupada";

                await _context.SaveChangesAsync();

                return StatusCode(HttpStatusCode.CREATED, new
                {
                    message = "Reserva creada exitosamente.",
                    reservationId = newReservation.Id,
                    montoTotal = totalAmount
                });
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.INTERNAL_SERVER_ERROR, new { message = $"Error interno: {ex.Message}" });
            }
        }

        [HttpGet("my-reservations")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> GetMyReservations()
        {
            try
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Name);
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Correo == userEmail);

                if (user == null)
                    return StatusCode(HttpStatusCode.UNAUTHORIZED, new { message = "Usuario no válido." });

                var reservas = await _context.Reservations
                    .Where(r => r.UserId == user.Id)
                    .OrderByDescending(r => r.CreatedDate)
                    .ToListAsync();

                return StatusCode(HttpStatusCode.OK, reservas);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.INTERNAL_SERVER_ERROR, new { message = $"Error interno: {ex.Message}" });
            }
        }
    }
}