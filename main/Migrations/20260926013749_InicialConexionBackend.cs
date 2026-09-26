using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace main.Migrations
{
    /// <inheritdoc />
    public partial class InicialConexionBackend : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Alertas",
                columns: table => new
                {
                    Id_alerta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    mensaje = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_horario = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    nivel_urgencia = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alertas", x => x.Id_alerta);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Barrios",
                columns: table => new
                {
                    Id_barrio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Barrios", x => x.Id_barrio);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AlertasBarrios",
                columns: table => new
                {
                    Id_barrio_alerta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_barrio = table.Column<int>(type: "int", nullable: false),
                    Id_alerta = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertasBarrios", x => x.Id_barrio_alerta);
                    table.ForeignKey(
                        name: "FK_AlertasBarrios_Alertas_Id_alerta",
                        column: x => x.Id_alerta,
                        principalTable: "Alertas",
                        principalColumn: "Id_alerta",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlertasBarrios_Barrios_Id_barrio",
                        column: x => x.Id_barrio,
                        principalTable: "Barrios",
                        principalColumn: "Id_barrio",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Ciudadanos",
                columns: table => new
                {
                    Id_ciudadano = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_barrio = table.Column<int>(type: "int", nullable: false),
                    dni = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    apellido = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ciudadanos", x => x.Id_ciudadano);
                    table.ForeignKey(
                        name: "FK_Ciudadanos_Barrios_Id_barrio",
                        column: x => x.Id_barrio,
                        principalTable: "Barrios",
                        principalColumn: "Id_barrio",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id_usuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_ciudadano = table.Column<int>(type: "int", nullable: false),
                    username = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    constrasenia = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    activo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    es_admin = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id_usuario);
                    table.ForeignKey(
                        name: "FK_Usuarios_Ciudadanos_Id_ciudadano",
                        column: x => x.Id_ciudadano,
                        principalTable: "Ciudadanos",
                        principalColumn: "Id_ciudadano",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Reportes",
                columns: table => new
                {
                    Id_reporte = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_usuario = table.Column<int>(type: "int", nullable: false),
                    categoria = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    titulo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    descripcion = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fecha_creacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    fecha_modificacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    activo = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reportes", x => x.Id_reporte);
                    table.ForeignKey(
                        name: "FK_Reportes_Usuarios_Id_usuario",
                        column: x => x.Id_usuario,
                        principalTable: "Usuarios",
                        principalColumn: "Id_usuario",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Ubicaciones",
                columns: table => new
                {
                    Id_ubicacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id_reporte = table.Column<int>(type: "int", nullable: false),
                    calle = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    altura = table.Column<int>(type: "int", nullable: false),
                    latitud = table.Column<int>(type: "int", nullable: false),
                    informacion_adicional = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ubicaciones", x => x.Id_ubicacion);
                    table.ForeignKey(
                        name: "FK_Ubicaciones_Reportes_Id_reporte",
                        column: x => x.Id_reporte,
                        principalTable: "Reportes",
                        principalColumn: "Id_reporte",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AlertasBarrios_Id_alerta",
                table: "AlertasBarrios",
                column: "Id_alerta");

            migrationBuilder.CreateIndex(
                name: "IX_AlertasBarrios_Id_barrio",
                table: "AlertasBarrios",
                column: "Id_barrio");

            migrationBuilder.CreateIndex(
                name: "IX_Ciudadanos_dni",
                table: "Ciudadanos",
                column: "dni",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ciudadanos_Id_barrio",
                table: "Ciudadanos",
                column: "Id_barrio");

            migrationBuilder.CreateIndex(
                name: "IX_Reportes_Id_usuario",
                table: "Reportes",
                column: "Id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Ubicaciones_Id_reporte",
                table: "Ubicaciones",
                column: "Id_reporte",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Id_ciudadano",
                table: "Usuarios",
                column: "Id_ciudadano",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlertasBarrios");

            migrationBuilder.DropTable(
                name: "Ubicaciones");

            migrationBuilder.DropTable(
                name: "Alertas");

            migrationBuilder.DropTable(
                name: "Reportes");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Ciudadanos");

            migrationBuilder.DropTable(
                name: "Barrios");
        }
    }
}
