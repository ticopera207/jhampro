using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Jham.Migrations
{
    /// <inheritdoc />
    public partial class RelacionAbogadoCasoEspecialidad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Citas_Usuarios_UsuarioId",
                table: "Citas");

            migrationBuilder.AddColumn<int>(
                name: "AbogadoId",
                table: "Citas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioId2",
                table: "Citas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Abogados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abogados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Abogados_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Especialidades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NombreEspecialidad = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Especialidades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rel_AbogadoCasos",
                columns: table => new
                {
                    AbogadoId = table.Column<int>(type: "integer", nullable: false),
                    CasoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rel_AbogadoCasos", x => new { x.AbogadoId, x.CasoId });
                    table.ForeignKey(
                        name: "FK_Rel_AbogadoCasos_Abogados_AbogadoId",
                        column: x => x.AbogadoId,
                        principalTable: "Abogados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rel_AbogadoCasos_Casos_CasoId",
                        column: x => x.CasoId,
                        principalTable: "Casos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rel_AbogadoEspecialidades",
                columns: table => new
                {
                    AbogadoId = table.Column<int>(type: "integer", nullable: false),
                    EspecialidadId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rel_AbogadoEspecialidades", x => new { x.AbogadoId, x.EspecialidadId });
                    table.ForeignKey(
                        name: "FK_Rel_AbogadoEspecialidades_Abogados_AbogadoId",
                        column: x => x.AbogadoId,
                        principalTable: "Abogados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rel_AbogadoEspecialidades_Especialidades_EspecialidadId",
                        column: x => x.EspecialidadId,
                        principalTable: "Especialidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Citas_AbogadoId",
                table: "Citas",
                column: "AbogadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Citas_UsuarioId2",
                table: "Citas",
                column: "UsuarioId2");

            migrationBuilder.CreateIndex(
                name: "IX_Abogados_UsuarioId",
                table: "Abogados",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Rel_AbogadoCasos_CasoId",
                table: "Rel_AbogadoCasos",
                column: "CasoId");

            migrationBuilder.CreateIndex(
                name: "IX_Rel_AbogadoEspecialidades_EspecialidadId",
                table: "Rel_AbogadoEspecialidades",
                column: "EspecialidadId");

            migrationBuilder.AddForeignKey(
                name: "FK_Citas_Abogados_AbogadoId",
                table: "Citas",
                column: "AbogadoId",
                principalTable: "Abogados",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Citas_Usuarios_UsuarioId",
                table: "Citas",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Citas_Usuarios_UsuarioId2",
                table: "Citas",
                column: "UsuarioId2",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Citas_Abogados_AbogadoId",
                table: "Citas");

            migrationBuilder.DropForeignKey(
                name: "FK_Citas_Usuarios_UsuarioId",
                table: "Citas");

            migrationBuilder.DropForeignKey(
                name: "FK_Citas_Usuarios_UsuarioId2",
                table: "Citas");

            migrationBuilder.DropTable(
                name: "Rel_AbogadoCasos");

            migrationBuilder.DropTable(
                name: "Rel_AbogadoEspecialidades");

            migrationBuilder.DropTable(
                name: "Abogados");

            migrationBuilder.DropTable(
                name: "Especialidades");

            migrationBuilder.DropIndex(
                name: "IX_Citas_AbogadoId",
                table: "Citas");

            migrationBuilder.DropIndex(
                name: "IX_Citas_UsuarioId2",
                table: "Citas");

            migrationBuilder.DropColumn(
                name: "AbogadoId",
                table: "Citas");

            migrationBuilder.DropColumn(
                name: "UsuarioId2",
                table: "Citas");

            migrationBuilder.AddForeignKey(
                name: "FK_Citas_Usuarios_UsuarioId",
                table: "Citas",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
