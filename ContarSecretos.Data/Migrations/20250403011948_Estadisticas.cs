using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ContarSecretos.Data.Migrations
{
    /// <inheritdoc />
    public partial class Estadisticas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "Tamanio",
                table: "AudioLibro",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateTable(
                name: "Estadistica",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AudioLibroId = table.Column<int>(type: "integer", nullable: true),
                    LibroId = table.Column<int>(type: "integer", nullable: true),
                    CountDescargas = table.Column<int>(type: "integer", nullable: false),
                    CountEscuchado = table.Column<int>(type: "integer", nullable: false),
                    CountLeido = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estadistica", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Estadistica_AudioLibro_AudioLibroId",
                        column: x => x.AudioLibroId,
                        principalTable: "AudioLibro",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Estadistica_Libro_LibroId",
                        column: x => x.LibroId,
                        principalTable: "Libro",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Estadistica_AudioLibroId",
                table: "Estadistica",
                column: "AudioLibroId");

            migrationBuilder.CreateIndex(
                name: "IX_Estadistica_LibroId",
                table: "Estadistica",
                column: "LibroId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Estadistica");

            migrationBuilder.AlterColumn<int>(
                name: "Tamanio",
                table: "AudioLibro",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
