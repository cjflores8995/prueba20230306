using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Persona",
                columns: table => new
                {
                    PersonaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombres = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Genero = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    Edad = table.Column<int>(type: "int", nullable: false),
                    Identificacion = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persona", x => x.PersonaId);
                });

            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    ClienteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    PersonaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.ClienteId);
                    table.ForeignKey(
                        name: "FK_Cliente_Persona_PersonaId",
                        column: x => x.PersonaId,
                        principalTable: "Persona",
                        principalColumn: "PersonaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cuenta",
                columns: table => new
                {
                    CuentaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumeroCuenta = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    TipoCuenta = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    SaldoInicial = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    ClienteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cuenta", x => x.CuentaId);
                    table.ForeignKey(
                        name: "FK_Cuenta_Cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "ClienteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Movimiento",
                columns: table => new
                {
                    MovimientoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TipoMovimiento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Saldo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CuentaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movimiento", x => x.MovimientoId);
                    table.ForeignKey(
                        name: "FK_Movimiento_Cuenta_CuentaId",
                        column: x => x.CuentaId,
                        principalTable: "Cuenta",
                        principalColumn: "CuentaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_PersonaId",
                table: "Cliente",
                column: "PersonaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cuenta_ClienteId",
                table: "Cuenta",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Cuenta_NumeroCuenta",
                table: "Cuenta",
                column: "NumeroCuenta",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Movimiento_CuentaId",
                table: "Movimiento",
                column: "CuentaId");

            migrationBuilder.CreateIndex(
                name: "IX_Persona_Identificacion",
                table: "Persona",
                column: "Identificacion",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movimiento");

            migrationBuilder.DropTable(
                name: "Cuenta");

            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "Persona");
        }
    }
}
