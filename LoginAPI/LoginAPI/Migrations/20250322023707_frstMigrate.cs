using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoginAPI.Migrations
{
    /// <inheritdoc />
    public partial class frstMigrate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Producto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Marca = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Precio = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Producto__3214EC07E8909AFD", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Correo = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Clave = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Usuario__3214EC07F27BC158", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Producto");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
