using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogicaAccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class lolo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_TiposEmpleados_IdEmpleado",
                table: "Empleados");

            migrationBuilder.RenameColumn(
                name: "IdEmpleado",
                table: "Empleados",
                newName: "IdTipoEmpleado");

            migrationBuilder.RenameIndex(
                name: "IX_Empleados_IdEmpleado",
                table: "Empleados",
                newName: "IX_Empleados_IdTipoEmpleado");

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_TiposEmpleados_IdTipoEmpleado",
                table: "Empleados",
                column: "IdTipoEmpleado",
                principalTable: "TiposEmpleados",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Empleados_TiposEmpleados_IdTipoEmpleado",
                table: "Empleados");

            migrationBuilder.RenameColumn(
                name: "IdTipoEmpleado",
                table: "Empleados",
                newName: "IdEmpleado");

            migrationBuilder.RenameIndex(
                name: "IX_Empleados_IdTipoEmpleado",
                table: "Empleados",
                newName: "IX_Empleados_IdEmpleado");

            migrationBuilder.AddForeignKey(
                name: "FK_Empleados_TiposEmpleados_IdEmpleado",
                table: "Empleados",
                column: "IdEmpleado",
                principalTable: "TiposEmpleados",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
