using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogicaAccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class f : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Carpeta",
                table: "Planos");

            migrationBuilder.AddColumn<int>(
                name: "ObraIdObra",
                table: "Materiales",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Materiales_ObraIdObra",
                table: "Materiales",
                column: "ObraIdObra");

            migrationBuilder.AddForeignKey(
                name: "FK_Materiales_Obras_ObraIdObra",
                table: "Materiales",
                column: "ObraIdObra",
                principalTable: "Obras",
                principalColumn: "IdObra");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materiales_Obras_ObraIdObra",
                table: "Materiales");

            migrationBuilder.DropIndex(
                name: "IX_Materiales_ObraIdObra",
                table: "Materiales");

            migrationBuilder.DropColumn(
                name: "ObraIdObra",
                table: "Materiales");

            migrationBuilder.AddColumn<string>(
                name: "Carpeta",
                table: "Planos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
