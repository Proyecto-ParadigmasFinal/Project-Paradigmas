using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Persons.API.Database.Entities.Common;

namespace ProyectoParadigmas.Entities
{
    [Table("reservations")]
    public class ReservationEntity : BaseEntity
    {
        [Required]
        [Column("user_id")]
        public string UserId { get; set; }

        // [ForeignKey(nameof(UserId))]
        // public virtual UserEntity User { get; set; }

        [Required]
        [Column("catalog_item_id")]
        public string CatalogItemId { get; set; } 

        [ForeignKey(nameof(CatalogItemId))]
        public virtual CatalogItemEntity CatalogItem { get; set; }

        [Required]
        [Column("fecha_check_in")]
        public DateTime FechaCheckIn { get; set; }

        [Required]
        [Column("fecha_check_out")]
        public DateTime FechaCheckOut { get; set; }

        [Required]
        [Column("monto_total")]
        public decimal MontoTotal { get; set; }

        [Column("fecha_transaccion")]
        public DateTime? FechaTransaccion { get; set; }

        [Required]
        [StringLength(20)]
        [Column("estado_transaccion")]
        public string EstadoTransaccion { get; set; } 

        [StringLength(100)]
        [Column("paypal_order_id")]
        public string PayPalOrderId { get; set; }
    }
}