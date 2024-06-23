using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogicaAccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class ew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ObrasEmpleados",
                columns: table => new
                {
                    IdObra = table.Column<int>(type: "int", nullable: false),
                    IdEmpleado = table.Column<int>(type: "int", nullable: false),
                    FechaIngreso = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaEgreso = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObrasEmpleados", x => new { x.IdObra, x.IdEmpleado });
                });

            migrationBuilder.CreateTable(
                name: "Proveedores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mail = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proveedores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TiposEmpleados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Categoría = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValorHora = table.Column<double>(type: "float", nullable: false),
                    Presentismo = table.Column<double>(type: "float", nullable: false),
                    Compensacion = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposEmpleados", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TiposPlanos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Categoria = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposPlanos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreUsuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contrasenia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    CambioContrasenia = table.Column<bool>(type: "bit", nullable: false),
                    IntentosFallidos = table.Column<int>(type: "int", nullable: false),
                    UsuarioBloqueado = table.Column<bool>(type: "bit", nullable: false),
                    TiempoDeBloqueo = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dias",
                columns: table => new
                {
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Horas = table.Column<int>(type: "int", nullable: false),
                    ObraEmpleadoIdEmpleado = table.Column<int>(type: "int", nullable: true),
                    ObraEmpleadoIdObra = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dias", x => x.Fecha);
                    table.ForeignKey(
                        name: "FK_Dias_ObrasEmpleados_ObraEmpleadoIdObra_ObraEmpleadoIdEmpleado",
                        columns: x => new { x.ObraEmpleadoIdObra, x.ObraEmpleadoIdEmpleado },
                        principalTable: "ObrasEmpleados",
                        principalColumns: new[] { "IdObra", "IdEmpleado" });
                });

            migrationBuilder.CreateTable(
                name: "Empleados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaIngreso = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdEmpleado = table.Column<int>(type: "int", nullable: false),
                    CuentaBanco = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Banco = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Empleados_TiposEmpleados_IdEmpleado",
                        column: x => x.IdEmpleado,
                        principalTable: "TiposEmpleados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Obras",
                columns: table => new
                {
                    IdObra = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFinalizacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Finalizada = table.Column<bool>(type: "bit", nullable: false),
                    IdACargo = table.Column<int>(type: "int", nullable: false),
                    NombreCronograma = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoCronograma = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cronograma = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    QR = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Obras", x => x.IdObra);
                    table.ForeignKey(
                        name: "FK_Obras_Usuarios_IdACargo",
                        column: x => x.IdACargo,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Materiales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnidadDeMedida = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ObraIdObra = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materiales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Materiales_Obras_ObraIdObra",
                        column: x => x.ObraIdObra,
                        principalTable: "Obras",
                        principalColumn: "IdObra");
                });

            migrationBuilder.CreateTable(
                name: "Planos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdTipoPlano = table.Column<int>(type: "int", nullable: false),
                    FechaPublicado = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdObra = table.Column<int>(type: "int", nullable: false),
                    NombrePdf = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoPdf = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pdf = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Planos_Obras_IdObra",
                        column: x => x.IdObra,
                        principalTable: "Obras",
                        principalColumn: "IdObra",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Planos_TiposPlanos_IdTipoPlano",
                        column: x => x.IdTipoPlano,
                        principalTable: "TiposPlanos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Solicitudes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdProveedor = table.Column<int>(type: "int", nullable: true),
                    IdObra = table.Column<int>(type: "int", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdUDeOficina = table.Column<int>(type: "int", nullable: true),
                    Estado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solicitudes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Solicitudes_Obras_IdObra",
                        column: x => x.IdObra,
                        principalTable: "Obras",
                        principalColumn: "IdObra",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Solicitudes_Proveedores_IdProveedor",
                        column: x => x.IdProveedor,
                        principalTable: "Proveedores",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Solicitudes_Usuarios_IdUDeOficina",
                        column: x => x.IdUDeOficina,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Solicitudes_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ObrasMateriales",
                columns: table => new
                {
                    IdObra = table.Column<int>(type: "int", nullable: false),
                    IdMaterial = table.Column<int>(type: "int", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObrasMateriales", x => new { x.IdObra, x.IdMaterial });
                    table.ForeignKey(
                        name: "FK_ObrasMateriales_Materiales_IdMaterial",
                        column: x => x.IdMaterial,
                        principalTable: "Materiales",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ObrasMateriales_Obras_IdObra",
                        column: x => x.IdObra,
                        principalTable: "Obras",
                        principalColumn: "IdObra",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SolicitudesMateriales",
                columns: table => new
                {
                    IdSolicitud = table.Column<int>(type: "int", nullable: false),
                    IdMaterial = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudesMateriales", x => new { x.IdSolicitud, x.IdMaterial });
                    table.ForeignKey(
                        name: "FK_SolicitudesMateriales_Materiales_IdMaterial",
                        column: x => x.IdMaterial,
                        principalTable: "Materiales",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SolicitudesMateriales_Solicitudes_IdSolicitud",
                        column: x => x.IdSolicitud,
                        principalTable: "Solicitudes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dias_ObraEmpleadoIdObra_ObraEmpleadoIdEmpleado",
                table: "Dias",
                columns: new[] { "ObraEmpleadoIdObra", "ObraEmpleadoIdEmpleado" });

            migrationBuilder.CreateIndex(
                name: "IX_Empleados_IdEmpleado",
                table: "Empleados",
                column: "IdEmpleado");

            migrationBuilder.CreateIndex(
                name: "IX_Materiales_ObraIdObra",
                table: "Materiales",
                column: "ObraIdObra");

            migrationBuilder.CreateIndex(
                name: "IX_Obras_IdACargo",
                table: "Obras",
                column: "IdACargo");

            migrationBuilder.CreateIndex(
                name: "IX_ObrasMateriales_IdMaterial",
                table: "ObrasMateriales",
                column: "IdMaterial");

            migrationBuilder.CreateIndex(
                name: "IX_Planos_IdObra",
                table: "Planos",
                column: "IdObra");

            migrationBuilder.CreateIndex(
                name: "IX_Planos_IdTipoPlano",
                table: "Planos",
                column: "IdTipoPlano");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitudes_IdObra",
                table: "Solicitudes",
                column: "IdObra");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitudes_IdProveedor",
                table: "Solicitudes",
                column: "IdProveedor");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitudes_IdUDeOficina",
                table: "Solicitudes",
                column: "IdUDeOficina");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitudes_IdUsuario",
                table: "Solicitudes",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesMateriales_IdMaterial",
                table: "SolicitudesMateriales",
                column: "IdMaterial");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dias");

            migrationBuilder.DropTable(
                name: "Empleados");

            migrationBuilder.DropTable(
                name: "ObrasMateriales");

            migrationBuilder.DropTable(
                name: "Planos");

            migrationBuilder.DropTable(
                name: "SolicitudesMateriales");

            migrationBuilder.DropTable(
                name: "ObrasEmpleados");

            migrationBuilder.DropTable(
                name: "TiposEmpleados");

            migrationBuilder.DropTable(
                name: "TiposPlanos");

            migrationBuilder.DropTable(
                name: "Materiales");

            migrationBuilder.DropTable(
                name: "Solicitudes");

            migrationBuilder.DropTable(
                name: "Obras");

            migrationBuilder.DropTable(
                name: "Proveedores");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
