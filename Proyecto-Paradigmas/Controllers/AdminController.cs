using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persons.API.Constants;
using Proyecto_Paradigmas.Dtos.Admin;
using ProyectoParadigmas.Database;
using ProyectoParadigmas.Entities;

namespace Proyecto_Paradigmas.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly HotelDbContext _context;

        public AdminController(HotelDbContext context)
        {
            _context = context;
        }

        [HttpPatch("rooms/{id}/status")]
        public async Task<IActionResult> UpdateRoomStatus(string id, [FromBody] UpdateRoomStatusDto request)
        {
            try
            {
                var room = await _context.CatalogItems.FindAsync(id);

                if (room == null)
                    return StatusCode(HttpStatusCode.NOT_FOUND, new { message = "Habitación/Paquete no encontrado." });

                room.Estado = request.Estado;

                room.UpdatedDate = DateTime.UtcNow;
                room.UpdatedBy = "Admin";

                await _context.SaveChangesAsync();

                return StatusCode(HttpStatusCode.OK, new { message = "Estado actualizado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.INTERNAL_SERVER_ERROR, new { message = $"Error interno: {ex.Message}" });
            }
        }

        [HttpGet("reports/sales")]
        public async Task<IActionResult> GetSalesReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                IQueryable<ReservationEntity> query = _context.Reservations
                    .Where(r => r.EstadoTransaccion == "Completado"
                             && r.FechaTransaccion >= startDate
                             && r.FechaTransaccion <= endDate);
                var sales = await EntityFrameworkQueryableExtensions.ToListAsync(query);

                var totalSales = sales.Sum(r => r.MontoTotal);
                var totalTransactions = sales.Count;

                return StatusCode(HttpStatusCode.OK, new
                {
                    TotalMonto = totalSales,
                    TotalTransacciones = totalTransactions,
                    Detalle = sales
                });
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.INTERNAL_SERVER_ERROR, new { message = $"Error interno: {ex.Message}" });
            }
        }

        [HttpGet("reservations")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> GetAllReservations()
        {
            try
            {
                var reservas = await _context.Reservations
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
