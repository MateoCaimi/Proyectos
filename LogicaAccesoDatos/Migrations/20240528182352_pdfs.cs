using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogicaAccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class pdfs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TipoImagen",
                table: "Planos",
                newName: "TipoPdf");

            migrationBuilder.RenameColumn(
                name: "NombreImagen",
                table: "Planos",
                newName: "NombrePdf");

            migrationBuilder.RenameColumn(
                name: "Imagen",
                table: "Planos",
                newName: "Pdf");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TipoPdf",
                table: "Planos",
                newName: "TipoImagen");

            migrationBuilder.RenameColumn(
                name: "Pdf",
                table: "Planos",
                newName: "Imagen");

            migrationBuilder.RenameColumn(
                name: "NombrePdf",
                table: "Planos",
                newName: "NombreImagen");
        }
    }
}
