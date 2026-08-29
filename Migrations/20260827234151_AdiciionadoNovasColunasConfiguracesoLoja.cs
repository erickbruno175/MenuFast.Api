using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MenuFast.Api.Migrations
{
    /// <inheritdoc />
    public partial class AdiciionadoNovasColunasConfiguracesoLoja : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TaxaEntrega",
                table: "ConfiguracaoLoja",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                comment: "Valor da taxa fixa de entrega.",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true,
                oldComment: "Indica o valor da taxa de entrega.");

            migrationBuilder.AlterColumn<decimal>(
                name: "PercentualTaxaServico",
                table: "ConfiguracaoLoja",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: true,
                comment: "Percentual aplicado para cobrança da taxa de serviço.",
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldPrecision: 5,
                oldScale: 2,
                oldNullable: true,
                oldComment: "Indica o valor da taxa  de serviço do garçom");

            migrationBuilder.AlterColumn<bool>(
                name: "CobraTaxaEntrega",
                table: "ConfiguracaoLoja",
                type: "bit",
                nullable: false,
                comment: "Indica se o restaurante cobra taxa de entrega.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Indica se a taxa de entrega.");

            migrationBuilder.AlterColumn<bool>(
                name: "AbilitarImpressoraTermica",
                table: "ConfiguracaoLoja",
                type: "bit",
                nullable: false,
                comment: "Indica se a configuração está ativa da impressora térmica.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Indica se a configuração está ativa da impressora termica.");

            migrationBuilder.AddColumn<decimal>(
                name: "DistanciaMaximaEntregaKm",
                table: "ConfiguracaoLoja",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: true,
                comment: "Distância máxima em quilômetros permitida para entrega.");

            migrationBuilder.AddColumn<decimal>(
                name: "TaxaBaseEntrega",
                table: "ConfiguracaoLoja",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                comment: "Valor base utilizado no cálculo da taxa de entrega por distância.");

            migrationBuilder.AddColumn<int>(
                name: "TipoTaxaEntrega",
                table: "ConfiguracaoLoja",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Define se a taxa de entrega é fixa ou calculada por distância.");

            migrationBuilder.AddColumn<decimal>(
                name: "ValorPorKm",
                table: "ConfiguracaoLoja",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                comment: "Valor adicional cobrado por quilômetro percorrido.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DistanciaMaximaEntregaKm",
                table: "ConfiguracaoLoja");

            migrationBuilder.DropColumn(
                name: "TaxaBaseEntrega",
                table: "ConfiguracaoLoja");

            migrationBuilder.DropColumn(
                name: "TipoTaxaEntrega",
                table: "ConfiguracaoLoja");

            migrationBuilder.DropColumn(
                name: "ValorPorKm",
                table: "ConfiguracaoLoja");

            migrationBuilder.AlterColumn<decimal>(
                name: "TaxaEntrega",
                table: "ConfiguracaoLoja",
                type: "decimal(18,2)",
                nullable: true,
                comment: "Indica o valor da taxa de entrega.",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true,
                oldComment: "Valor da taxa fixa de entrega.");

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
                oldNullable: true,
                oldComment: "Percentual aplicado para cobrança da taxa de serviço.");

            migrationBuilder.AlterColumn<bool>(
                name: "CobraTaxaEntrega",
                table: "ConfiguracaoLoja",
                type: "bit",
                nullable: false,
                comment: "Indica se a taxa de entrega.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Indica se o restaurante cobra taxa de entrega.");

            migrationBuilder.AlterColumn<bool>(
                name: "AbilitarImpressoraTermica",
                table: "ConfiguracaoLoja",
                type: "bit",
                nullable: false,
                comment: "Indica se a configuração está ativa da impressora termica.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Indica se a configuração está ativa da impressora térmica.");
        }
    }
}
