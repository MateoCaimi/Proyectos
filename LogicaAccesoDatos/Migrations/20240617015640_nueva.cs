using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogicaAccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class nueva : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solicitudes_Proveedores_IdProveedor",
                table: "Solicitudes");

            migrationBuilder.DropForeignKey(
                name: "FK_Solicitudes_Usuarios_AprovadorId",
                table: "Solicitudes");

            migrationBuilder.DropForeignKey(
                name: "FK_Solicitudes_Usuarios_SolicitanteId",
                table: "Solicitudes");

            migrationBuilder.DropIndex(
                name: "IX_Solicitudes_AprovadorId",
                table: "Solicitudes");

            migrationBuilder.DropIndex(
                name: "IX_Solicitudes_SolicitanteId",
                table: "Solicitudes");

            migrationBuilder.DropColumn(
                name: "AprovadorId",
                table: "Solicitudes");

            migrationBuilder.DropColumn(
                name: "SolicitanteId",
                table: "Solicitudes");

            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Materiales");

            migrationBuilder.AlterColumn<int>(
                name: "IdUDeOficina",
                table: "Solicitudes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "IdProveedor",
                table: "Solicitudes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitudes_IdUDeOficina",
                table: "Solicitudes",
                column: "IdUDeOficina");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitudes_IdUsuario",
                table: "Solicitudes",
                column: "IdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitudes_Proveedores_IdProveedor",
                table: "Solicitudes",
                column: "IdProveedor",
                principalTable: "Proveedores",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitudes_Usuarios_IdUDeOficina",
                table: "Solicitudes",
                column: "IdUDeOficina",
                principalTable: "Usuarios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitudes_Usuarios_IdUsuario",
                table: "Solicitudes",
                column: "IdUsuario",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solicitudes_Proveedores_IdProveedor",
                table: "Solicitudes");

            migrationBuilder.DropForeignKey(
                name: "FK_Solicitudes_Usuarios_IdUDeOficina",
                table: "Solicitudes");

            migrationBuilder.DropForeignKey(
                name: "FK_Solicitudes_Usuarios_IdUsuario",
                table: "Solicitudes");

            migrationBuilder.DropIndex(
                name: "IX_Solicitudes_IdUDeOficina",
                table: "Solicitudes");

            migrationBuilder.DropIndex(
                name: "IX_Solicitudes_IdUsuario",
                table: "Solicitudes");

            migrationBuilder.AlterColumn<int>(
                name: "IdUDeOficina",
                table: "Solicitudes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdProveedor",
                table: "Solicitudes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AprovadorId",
                table: "Solicitudes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SolicitanteId",
                table: "Solicitudes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "Materiales",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Solicitudes_AprovadorId",
                table: "Solicitudes",
                column: "AprovadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitudes_SolicitanteId",
                table: "Solicitudes",
                column: "SolicitanteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitudes_Proveedores_IdProveedor",
                table: "Solicitudes",
                column: "IdProveedor",
                principalTable: "Proveedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitudes_Usuarios_AprovadorId",
                table: "Solicitudes",
                column: "AprovadorId",
                principalTable: "Usuarios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitudes_Usuarios_SolicitanteId",
                table: "Solicitudes",
                column: "SolicitanteId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
