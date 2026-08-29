using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MenuFast.Api.Migrations
{
    /// <inheritdoc />
    public partial class nomecoluna : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PagamentoPedido");

            migrationBuilder.AlterColumn<decimal>(
                name: "TaxaServico",
                table: "Pedido",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldComment: "Taxa de serviço aplicada.");

            migrationBuilder.AlterColumn<decimal>(
                name: "TaxaEntrega",
                table: "Pedido",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldComment: "Taxa de entrega aplicada.");

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "Pedido",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "ItemPedido",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nome",
                table: "Pedido");

            migrationBuilder.DropColumn(
                name: "Nome",
                table: "ItemPedido");

            migrationBuilder.AlterColumn<decimal>(
                name: "TaxaServico",
                table: "Pedido",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                comment: "Taxa de serviço aplicada.",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TaxaEntrega",
                table: "Pedido",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                comment: "Taxa de entrega aplicada.",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.CreateTable(
                name: "PagamentoPedido",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identificador único do pagamento do pedido.")
                        .Annotation("SqlServer:Identity", "2001, 1"),
                    FormaPagamentoId = table.Column<int>(type: "int", nullable: false, comment: "Forma de pagamento utilizada."),
                    PedidoId = table.Column<int>(type: "int", nullable: false, comment: "Pedido vinculado ao pagamento."),
                    DataPagamento = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Data e hora do pagamento."),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "Valor pago.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagamentoPedido", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PagamentoPedido_FormaPagamento_FormaPagamentoId",
                        column: x => x.FormaPagamentoId,
                        principalTable: "FormaPagamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PagamentoPedido_Pedido_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedido",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PagamentoPedido_FormaPagamentoId",
                table: "PagamentoPedido",
                column: "FormaPagamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_PagamentoPedido_PedidoId",
                table: "PagamentoPedido",
                column: "PedidoId");
        }
    }
}
