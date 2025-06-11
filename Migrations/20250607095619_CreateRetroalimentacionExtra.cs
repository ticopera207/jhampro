using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jham.Migrations
{
    /// <inheritdoc />
    public partial class CreateRetroalimentacionExtra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Publico",
                table: "retroalimentacion",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Publico",
                table: "retroalimentacion");
        }
    }
}
