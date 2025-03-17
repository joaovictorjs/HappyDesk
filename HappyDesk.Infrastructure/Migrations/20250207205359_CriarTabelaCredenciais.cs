using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HappyDesk.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CriarTabelaCredenciais : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "credenciais",
                columns: table => new
                {
                    codigo = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    email = table.Column<string>(type: "TEXT", nullable: false),
                    password = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credenciais", x => x.codigo);
                });

            migrationBuilder.InsertData(
                table: "credenciais",
                columns: new[] { "codigo", "email", "password" },
                values: new object[] { 1, "", "" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "credenciais");
        }
    }
}
