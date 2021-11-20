using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Monedero.Migrations
{
    public partial class MonederoContextMonederoContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cuentas",
                columns: table => new
                {
                    CuentaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Saldo = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cuentas", x => x.CuentaId);
                });

            migrationBuilder.CreateTable(
                name: "Movimientos",
                columns: table => new
                {
                    MovimientoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Importe = table.Column<int>(nullable: false),
                    Descripcion = table.Column<string>(nullable: true),
                    Fecha = table.Column<DateTime>(nullable: false),
                    TipoMovimiento = table.Column<int>(nullable: false),
                    CuentaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movimientos", x => x.MovimientoId);
                    table.ForeignKey(
                        name: "FK_Movimientos_Cuentas_CuentaId",
                        column: x => x.CuentaId,
                        principalTable: "Cuentas",
                        principalColumn: "CuentaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(nullable: false),
                    Apellido = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    CuentaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.UsuarioId);
                    table.ForeignKey(
                        name: "FK_Usuarios_Cuentas_CuentaId",
                        column: x => x.CuentaId,
                        principalTable: "Cuentas",
                        principalColumn: "CuentaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movimientos_CuentaId",
                table: "Movimientos",
                column: "CuentaId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_CuentaId",
                table: "Usuarios",
                column: "CuentaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movimientos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Cuentas");
        }
    }
}
