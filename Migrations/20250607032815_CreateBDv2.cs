using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Jham.Migrations
{
    /// <inheritdoc />
    public partial class CreateBDv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "documento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NombreArchivo = table.Column<string>(type: "text", nullable: false),
                    RutaArchivo = table.Column<string>(type: "text", nullable: false),
                    FechaSubida = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Observacion = table.Column<string>(type: "text", nullable: true),
                    ServicioId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_documento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_documento_servicio_ServicioId",
                        column: x => x.ServicioId,
                        principalTable: "servicio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "retroalimentacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Comentario = table.Column<string>(type: "text", nullable: false),
                    Calificacion = table.Column<int>(type: "integer", nullable: false),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ServicioId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_retroalimentacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_retroalimentacion_servicio_ServicioId",
                        column: x => x.ServicioId,
                        principalTable: "servicio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_documento_ServicioId",
                table: "documento",
                column: "ServicioId");

            migrationBuilder.CreateIndex(
                name: "IX_retroalimentacion_ServicioId",
                table: "retroalimentacion",
                column: "ServicioId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "documento");

            migrationBuilder.DropTable(
                name: "retroalimentacion");
        }
    }
}
