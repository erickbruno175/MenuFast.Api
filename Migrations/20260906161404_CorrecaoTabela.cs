using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MenuFast.Api.Migrations
{
    /// <inheritdoc />
    public partial class CorrecaoTabela : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VendaId",
                table: "Pedido",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PercentualComissao",
                table: "Funcionario",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorAberturaCaixa",
                table: "ConfiguracaoLoja",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: true,
                comment: "Valor de abertura de caixa.");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataPagamento",
                table: "ComissaoVenda",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusComissao",
                table: "ComissaoVenda",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Vendas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LojaId = table.Column<int>(type: "int", nullable: false),
                    ValorBruto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Desconto = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    Acrescimo = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    ValorTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataVenda = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FuncionarioId = table.Column<int>(type: "int", nullable: true),
                    StatusPagamento = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PagamentosVenda",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VendaId = table.Column<int>(type: "int", nullable: false),
                    FormaPagamentoId = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Troco = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagamentosVenda", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PagamentosVenda_Vendas_VendaId",
                        column: x => x.VendaId,
                        principalTable: "Vendas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pedido_VendaId",
                table: "Pedido",
                column: "VendaId");

            migrationBuilder.CreateIndex(
                name: "IX_PagamentosVenda_FormaPagamentoId",
                table: "PagamentosVenda",
                column: "FormaPagamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_PagamentosVenda_VendaId",
                table: "PagamentosVenda",
                column: "VendaId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_DataVenda",
                table: "Vendas",
                column: "DataVenda");

            migrationBuilder.CreateIndex(
                name: "IX_Vendas_LojaId",
                table: "Vendas",
                column: "LojaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedido_Vendas_VendaId",
                table: "Pedido",
                column: "VendaId",
                principalTable: "Vendas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedido_Vendas_VendaId",
                table: "Pedido");

            migrationBuilder.DropTable(
                name: "PagamentosVenda");

            migrationBuilder.DropTable(
                name: "Vendas");

            migrationBuilder.DropIndex(
                name: "IX_Pedido_VendaId",
                table: "Pedido");

            migrationBuilder.DropColumn(
                name: "VendaId",
                table: "Pedido");

            migrationBuilder.DropColumn(
                name: "PercentualComissao",
                table: "Funcionario");

            migrationBuilder.DropColumn(
                name: "ValorAberturaCaixa",
                table: "ConfiguracaoLoja");

            migrationBuilder.DropColumn(
                name: "DataPagamento",
                table: "ComissaoVenda");

            migrationBuilder.DropColumn(
                name: "StatusComissao",
                table: "ComissaoVenda");
        }
    }
}
