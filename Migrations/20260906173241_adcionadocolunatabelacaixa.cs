using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MenuFast.Api.Migrations
{
    /// <inheritdoc />
    public partial class adcionadocolunatabelacaixa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IpTerminal",
                table: "Caixa",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                comment: "Endereço IP do terminal onde o caixa foi aberto.");

            migrationBuilder.AddColumn<string>(
                name: "Terminal",
                table: "Caixa",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                comment: "Nome do terminal onde o caixa foi aberto.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IpTerminal",
                table: "Caixa");

            migrationBuilder.DropColumn(
                name: "Terminal",
                table: "Caixa");
        }
    }
}
