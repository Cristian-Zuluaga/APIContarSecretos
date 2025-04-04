using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContarSecretos.Data.Migrations
{
    /// <inheritdoc />
    public partial class AjusteLibroPath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Libro",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "Libro",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Libro");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "Libro");
        }
    }
}
