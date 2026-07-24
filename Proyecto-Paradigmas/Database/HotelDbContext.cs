using Microsoft.EntityFrameworkCore;
using ProyectoParadigmas.Entities;

namespace ProyectoParadigmas.Database
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<CatalogItemEntity> CatalogItems { get; set; }
        public DbSet<ReservationEntity> Reservations { get; set; }
    }
}