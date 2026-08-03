using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persons.API.Constants;
using Proyecto_Paradigmas.Dtos.Admin;
using Proyecto_Paradigmas.Dtos.Reservations;
using ProyectoParadigmas.Database;
using ProyectoParadigmas.Entities;

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

        [HttpPatch("rooms/{id}/status")]
        public async Task<IActionResult> UpdateRoomStatus(string id, [FromBody] UpdateRoomStatusDto request)
        {
            try
            {
                // Corrección: Usar FindAsync evade el conflicto de LINQ para llaves primarias
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
                // Corrección: Declarar IQueryable explícitamente
                IQueryable<ReservationEntity> query = _context.Reservations
                    .Where(r => r.EstadoTransaccion == "Completado"
                             && r.FechaTransaccion >= startDate
                             && r.FechaTransaccion <= endDate);

                // Corrección: Llamar a la extensión estática de EF Core directamente para evitar System.Linq.Async
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
    }
}
