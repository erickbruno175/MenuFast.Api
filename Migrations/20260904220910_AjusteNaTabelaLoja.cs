using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MenuFast.Api.Migrations
{
    /// <inheritdoc />
    public partial class AjusteNaTabelaLoja : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Foto",
                table: "FormaPagamento");

            migrationBuilder.DropColumn(
                name: "EnviarPedidoAutomaticamenteBar",
                table: "ConfiguracaoLoja");

            migrationBuilder.DropColumn(
                name: "EnviarPedidoAutomaticamenteCozinha",
                table: "ConfiguracaoLoja");

            migrationBuilder.DropColumn(
                name: "ExigirGarcomNaMesa",
                table: "ConfiguracaoLoja");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "FormaPagamento",
                newName: "Descricao");

            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                table: "Loja",
                type: "decimal(18,2)",
                nullable: true,
                comment: "Latitude da localização da empresa.");

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                table: "Loja",
                type: "decimal(18,2)",
                nullable: true,
                comment: "Longitude da localização da empresa.");

            migrationBuilder.AddColumn<int>(
                name: "TaxaEntregaMinima",
                table: "ConfiguracaoLoja",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Cliente",
                type: "decimal(9,6)",
                precision: 9,
                scale: 6,
                nullable: true,
                comment: "Longitude do endereço do cliente.",
                oldClrType: typeof(decimal),
                oldType: "decimal(9,6)",
                oldPrecision: 9,
                oldScale: 6,
                oldComment: "Longitude do endereço do cliente.");

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Cliente",
                type: "decimal(9,6)",
                precision: 9,
                scale: 6,
                nullable: true,
                comment: "Latitude do endereço do cliente.",
                oldClrType: typeof(decimal),
                oldType: "decimal(9,6)",
                oldPrecision: 9,
                oldScale: 6,
                oldComment: "Latitude do endereço do cliente.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Loja");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Loja");

            migrationBuilder.DropColumn(
                name: "TaxaEntregaMinima",
                table: "ConfiguracaoLoja");

            migrationBuilder.RenameColumn(
                name: "Descricao",
                table: "FormaPagamento",
                newName: "Nome");

            migrationBuilder.AddColumn<string>(
                name: "Foto",
                table: "FormaPagamento",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                comment: "Imagem ou ícone da forma de pagamento.");

            migrationBuilder.AddColumn<bool>(
                name: "EnviarPedidoAutomaticamenteBar",
                table: "ConfiguracaoLoja",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Indica se o pedido deve ser enviado automaticamente para o bar.");

            migrationBuilder.AddColumn<bool>(
                name: "EnviarPedidoAutomaticamenteCozinha",
                table: "ConfiguracaoLoja",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Indica se o pedido deve ser enviado automaticamente para a cozinha.");

            migrationBuilder.AddColumn<bool>(
                name: "ExigirGarcomNaMesa",
                table: "ConfiguracaoLoja",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Indica se é obrigatório informar garçom responsável pela mesa.");

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Cliente",
                type: "decimal(9,6)",
                precision: 9,
                scale: 6,
                nullable: false,
                defaultValue: 0m,
                comment: "Longitude do endereço do cliente.",
                oldClrType: typeof(decimal),
                oldType: "decimal(9,6)",
                oldPrecision: 9,
                oldScale: 6,
                oldNullable: true,
                oldComment: "Longitude do endereço do cliente.");

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Cliente",
                type: "decimal(9,6)",
                precision: 9,
                scale: 6,
                nullable: false,
                defaultValue: 0m,
                comment: "Latitude do endereço do cliente.",
                oldClrType: typeof(decimal),
                oldType: "decimal(9,6)",
                oldPrecision: 9,
                oldScale: 6,
                oldNullable: true,
                oldComment: "Latitude do endereço do cliente.");
        }
    }
}
