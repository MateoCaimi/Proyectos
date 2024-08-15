using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogicaAccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class uno : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Planos_Carpetas_CarpetaContenedoraName_CarpetaContenedoraIdObra",
                table: "Planos");

            migrationBuilder.DropForeignKey(
                name: "FK_Solicitudes_Proveedores_IdProveedor",
                table: "Solicitudes");

            migrationBuilder.DropIndex(
                name: "IX_Planos_CarpetaContenedoraName_CarpetaContenedoraIdObra",
                table: "Planos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Proveedores",
                table: "Proveedores");

            migrationBuilder.DropColumn(
                name: "CarpetaContenedoraIdObra",
                table: "Planos");

            migrationBuilder.DropColumn(
                name: "CarpetaContenedoraName",
                table: "Planos");

            migrationBuilder.RenameTable(
                name: "Proveedores",
                newName: "Proveedor");

            migrationBuilder.AddColumn<DateTime>(
                name: "UltimaActualizacion",
                table: "Obras",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Proveedor",
                table: "Proveedor",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitudes_Proveedor_IdProveedor",
                table: "Solicitudes",
                column: "IdProveedor",
                principalTable: "Proveedor",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solicitudes_Proveedor_IdProveedor",
                table: "Solicitudes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Proveedor",
                table: "Proveedor");

            migrationBuilder.DropColumn(
                name: "UltimaActualizacion",
                table: "Obras");

            migrationBuilder.RenameTable(
                name: "Proveedor",
                newName: "Proveedores");

            migrationBuilder.AddColumn<int>(
                name: "CarpetaContenedoraIdObra",
                table: "Planos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CarpetaContenedoraName",
                table: "Planos",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Proveedores",
                table: "Proveedores",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Planos_CarpetaContenedoraName_CarpetaContenedoraIdObra",
                table: "Planos",
                columns: new[] { "CarpetaContenedoraName", "CarpetaContenedoraIdObra" });

            migrationBuilder.AddForeignKey(
                name: "FK_Planos_Carpetas_CarpetaContenedoraName_CarpetaContenedoraIdObra",
                table: "Planos",
                columns: new[] { "CarpetaContenedoraName", "CarpetaContenedoraIdObra" },
                principalTable: "Carpetas",
                principalColumns: new[] { "Name", "IdObra" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitudes_Proveedores_IdProveedor",
                table: "Solicitudes",
                column: "IdProveedor",
                principalTable: "Proveedores",
                principalColumn: "Id");
        }
    }
}
