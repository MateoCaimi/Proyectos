using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogicaAccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class s : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SolicitudesMateriales_Materiales_IdMaterial",
                table: "SolicitudesMateriales");

            migrationBuilder.AddForeignKey(
                name: "FK_SolicitudesMateriales_Materiales_IdMaterial",
                table: "SolicitudesMateriales",
                column: "IdMaterial",
                principalTable: "Materiales",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SolicitudesMateriales_Materiales_IdMaterial",
                table: "SolicitudesMateriales");

            migrationBuilder.AddForeignKey(
                name: "FK_SolicitudesMateriales_Materiales_IdMaterial",
                table: "SolicitudesMateriales",
                column: "IdMaterial",
                principalTable: "Materiales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
