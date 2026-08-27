using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MenuFast.Api.Migrations
{
    /// <inheritdoc />
    public partial class NovaColunaTamnhoTabelaProduto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tamanho",
                table: "Produto",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Indica o tamanho do produto caso for pizza ex:");

            migrationBuilder.AlterColumn<decimal>(
                name: "PercentualTaxaServico",
                table: "ConfiguracaoLoja",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: true,
                comment: "Indica o valor da taxa  de serviço do garçom",
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldPrecision: 5,
                oldScale: 2,
                oldComment: "Percentual aplicado para cobrança da taxa de serviço.");

            migrationBuilder.AddColumn<bool>(
                name: "CobraTaxaEntrega",
                table: "ConfiguracaoLoja",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Indica se a taxa de entrega.");

            migrationBuilder.AddColumn<decimal>(
                name: "TaxaEntrega",
                table: "ConfiguracaoLoja",
                type: "decimal(18,2)",
                nullable: true,
                comment: "Indica o valor da taxa de entrega.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tamanho",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "CobraTaxaEntrega",
                table: "ConfiguracaoLoja");

            migrationBuilder.DropColumn(
                name: "TaxaEntrega",
                table: "ConfiguracaoLoja");

            migrationBuilder.AlterColumn<decimal>(
                name: "PercentualTaxaServico",
                table: "ConfiguracaoLoja",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                comment: "Percentual aplicado para cobrança da taxa de serviço.",
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldPrecision: 5,
                oldScale: 2,
                oldNullable: true,
                oldComment: "Indica o valor da taxa  de serviço do garçom");
        }
    }
}
