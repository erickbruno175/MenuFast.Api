using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MenuFast.Api.Migrations
{
    /// <inheritdoc />
    public partial class AdicionadoNovaColunaEntidadeLoja : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ConfiguracaoFinalizada",
                table: "Loja",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Verifica se todas as configurações iniciai foram cadastradas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfiguracaoFinalizada",
                table: "Loja");
        }
    }
}
