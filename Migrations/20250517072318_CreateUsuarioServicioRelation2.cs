using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jham.Migrations
{
    /// <inheritdoc />
    public partial class CreateUsuarioServicioRelation2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbogadoServicio_servicio_ServicioId",
                table: "AbogadoServicio");

            migrationBuilder.DropForeignKey(
                name: "FK_AbogadoServicio_usuario_UsuarioId",
                table: "AbogadoServicio");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AbogadoServicio",
                table: "AbogadoServicio");

            migrationBuilder.RenameTable(
                name: "AbogadoServicio",
                newName: "abogado_servicio");

            migrationBuilder.RenameIndex(
                name: "IX_AbogadoServicio_ServicioId",
                table: "abogado_servicio",
                newName: "IX_abogado_servicio_ServicioId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_abogado_servicio",
                table: "abogado_servicio",
                columns: new[] { "UsuarioId", "ServicioId" });

            migrationBuilder.AddForeignKey(
                name: "FK_abogado_servicio_servicio_ServicioId",
                table: "abogado_servicio",
                column: "ServicioId",
                principalTable: "servicio",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_abogado_servicio_usuario_UsuarioId",
                table: "abogado_servicio",
                column: "UsuarioId",
                principalTable: "usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_abogado_servicio_servicio_ServicioId",
                table: "abogado_servicio");

            migrationBuilder.DropForeignKey(
                name: "FK_abogado_servicio_usuario_UsuarioId",
                table: "abogado_servicio");

            migrationBuilder.DropPrimaryKey(
                name: "PK_abogado_servicio",
                table: "abogado_servicio");

            migrationBuilder.RenameTable(
                name: "abogado_servicio",
                newName: "AbogadoServicio");

            migrationBuilder.RenameIndex(
                name: "IX_abogado_servicio_ServicioId",
                table: "AbogadoServicio",
                newName: "IX_AbogadoServicio_ServicioId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AbogadoServicio",
                table: "AbogadoServicio",
                columns: new[] { "UsuarioId", "ServicioId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AbogadoServicio_servicio_ServicioId",
                table: "AbogadoServicio",
                column: "ServicioId",
                principalTable: "servicio",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AbogadoServicio_usuario_UsuarioId",
                table: "AbogadoServicio",
                column: "UsuarioId",
                principalTable: "usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
