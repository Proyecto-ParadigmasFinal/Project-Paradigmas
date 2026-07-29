using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persons.API.Constants;
using Proyecto_Paradigmas.Database.Entities;
using Proyecto_Paradigmas.Dtos.Users;
using ProyectoParadigmas.Database;

namespace Proyecto_Paradigmas.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly HotelDbContext _context;

        public UsersController(HotelDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserCreateDto request)
        {
            try
            {
                // Refactorizado a FirstOrDefaultAsync para evitar conflictos de IAsyncEnumerable
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Correo == request.Correo);
                if (existingUser != null)
                    return StatusCode(HttpStatusCode.CONFLICT, new { message = "El correo ya se encuentra registrado." });

                var newUser = new UserEntity
                {
                    Id = Guid.NewGuid().ToString(),
                    Correo = request.Correo,
                    PasswordHash = request.Password,
                    Rol = string.IsNullOrWhiteSpace(request.Rol) ? "Cliente" : request.Rol,
                    CreatedBy = "System",
                    CreatedDate = DateTime.UtcNow,
                    UpdatedBy = "System",
                    UpdatedDate = DateTime.UtcNow
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                return StatusCode(HttpStatusCode.CREATED, new { message = "Usuario registrado exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.INTERNAL_SERVER_ERROR, new { message = $"Error interno: {ex.Message}" });
            }
        }
    }
}
