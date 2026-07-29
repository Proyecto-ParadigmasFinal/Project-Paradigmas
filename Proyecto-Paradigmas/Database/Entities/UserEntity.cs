using Persons.API.Database.Entities.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Paradigmas.Database.Entities
{
    [Table("users")]
    public class UserEntity : BaseEntity
    {
        [Required]
        [StringLength(100)]
        [Column("correo")]
        public string Correo { get; set; }

        [Required]
        [Column("password_hash")]
        public string PasswordHash { get; set; }

        [Required]
        [StringLength(20)]
        [Column("rol")]
        public string Rol { get; set; } // Valores esperados: "Cliente" o "Administrador"
    }
}