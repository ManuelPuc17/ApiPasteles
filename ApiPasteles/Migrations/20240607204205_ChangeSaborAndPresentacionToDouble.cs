using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiPasteles.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSaborAndPresentacionToDouble : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pastel",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Origen = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Pastel__3213E83F446B6C09", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    clave = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Usuario__3213E83F34DEF25A", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Calificacion",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Usuario = table.Column<int>(type: "int", nullable: true),
                    Pastel = table.Column<int>(type: "int", nullable: true),
                    sabor = table.Column<double>(type: "float", unicode: false, maxLength: 50, nullable: true),
                    Presentacion = table.Column<double>(type: "float", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Califica__3213E83FC1F99D51", x => x.id);
                    table.ForeignKey(
                        name: "FK__Calificac__Paste__5FB337D6",
                        column: x => x.Pastel,
                        principalTable: "Pastel",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Calificac__Usuar__5EBF139D",
                        column: x => x.Usuario,
                        principalTable: "Usuario",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Calificacion_Pastel",
                table: "Calificacion",
                column: "Pastel");

            migrationBuilder.CreateIndex(
                name: "IX_Calificacion_Usuario",
                table: "Calificacion",
                column: "Usuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Calificacion");

            migrationBuilder.DropTable(
                name: "Pastel");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
