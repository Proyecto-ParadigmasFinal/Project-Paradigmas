using Microsoft.EntityFrameworkCore;
using Proyecto_Paradigmas.Database.Entities;
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
        public DbSet<UserEntity> Users { get; set; }
    }
}