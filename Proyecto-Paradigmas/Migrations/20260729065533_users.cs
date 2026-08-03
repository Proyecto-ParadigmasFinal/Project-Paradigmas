using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto_Paradigmas.Migrations
{
    /// <inheritdoc />
    public partial class users : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "catalog_items",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", nullable: false),
                    nombre = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "text", nullable: true),
                    precio = table.Column<decimal>(type: "TEXT", nullable: false),
                    imagen_url = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    tipo = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    capacidad_personas = table.Column<int>(type: "INTEGER", nullable: false),
                    numero_habitacion = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    estado = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    created_by = table.Column<string>(type: "TEXT", nullable: true),
                    created_date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_by = table.Column<string>(type: "TEXT", nullable: true),
                    updated_date = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_catalog_items", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", nullable: false),
                    correo = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    password_hash = table.Column<string>(type: "TEXT", nullable: false),
                    rol = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    created_by = table.Column<string>(type: "TEXT", nullable: true),
                    created_date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_by = table.Column<string>(type: "TEXT", nullable: true),
                    updated_date = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "reservations",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", nullable: false),
                    user_id = table.Column<string>(type: "TEXT", nullable: false),
                    catalog_item_id = table.Column<string>(type: "TEXT", nullable: false),
                    fecha_check_in = table.Column<DateTime>(type: "TEXT", nullable: false),
                    fecha_check_out = table.Column<DateTime>(type: "TEXT", nullable: false),
                    monto_total = table.Column<decimal>(type: "TEXT", nullable: false),
                    fecha_transaccion = table.Column<DateTime>(type: "TEXT", nullable: true),
                    estado_transaccion = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    paypal_order_id = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    created_by = table.Column<string>(type: "TEXT", nullable: true),
                    created_date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_by = table.Column<string>(type: "TEXT", nullable: true),
                    updated_date = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservations", x => x.id);
                    table.ForeignKey(
                        name: "FK_reservations_catalog_items_catalog_item_id",
                        column: x => x.catalog_item_id,
                        principalTable: "catalog_items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_reservations_catalog_item_id",
                table: "reservations",
                column: "catalog_item_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reservations");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "catalog_items");
        }
    }
}
