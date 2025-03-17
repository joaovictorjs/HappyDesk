using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HappyDesk.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CriarTabelaPreferencias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "preferencias",
                columns: table => new
                {
                    codigo = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    auto_login_habilitado = table.Column<bool>(type: "INTEGER", nullable: false),
                    notificacao_habilitada = table.Column<bool>(type: "INTEGER", nullable: false),
                    pessoas_observadas = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_preferencias", x => x.codigo);
                });

            migrationBuilder.InsertData(
                table: "preferencias",
                columns: new[] { "codigo", "auto_login_habilitado", "notificacao_habilitada", "pessoas_observadas" },
                values: new object[] { 1, true, true, "" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "preferencias");
        }
    }
}
