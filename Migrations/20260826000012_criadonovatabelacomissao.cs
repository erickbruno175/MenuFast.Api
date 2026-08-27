using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MenuFast.Api.Migrations
{
    /// <inheritdoc />
    public partial class criadonovatabelacomissao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContaReceber");

            migrationBuilder.DropColumn(
                name: "ImprimirPedidoAutomaticamente",
                table: "ConfiguracaoLoja");

            migrationBuilder.AddColumn<bool>(
                name: "AbilitarImpressoraTermica",
                table: "ConfiguracaoLoja",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Indica se a configuração está ativa da impressora termica.");

            migrationBuilder.AddColumn<bool>(
                name: "AbilitarKDS",
                table: "ConfiguracaoLoja",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Indica se a configuração está ativa do KDS.");

            migrationBuilder.CreateTable(
                name: "ComissaoVenda",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FuncionarioId = table.Column<int>(type: "int", nullable: false),
                    PedidoId = table.Column<int>(type: "int", nullable: false),
                    ValorVenda = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PercentualComissao = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    ValorComissao = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DataVenda = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComissaoVenda", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComissaoVenda_Funcionario_FuncionarioId",
                        column: x => x.FuncionarioId,
                        principalTable: "Funcionario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ComissaoVenda_Pedido_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedido",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComissaoVenda_FuncionarioId",
                table: "ComissaoVenda",
                column: "FuncionarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ComissaoVenda_PedidoId",
                table: "ComissaoVenda",
                column: "PedidoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComissaoVenda");

            migrationBuilder.DropColumn(
                name: "AbilitarImpressoraTermica",
                table: "ConfiguracaoLoja");

            migrationBuilder.DropColumn(
                name: "AbilitarKDS",
                table: "ConfiguracaoLoja");

            migrationBuilder.AddColumn<bool>(
                name: "ImprimirPedidoAutomaticamente",
                table: "ConfiguracaoLoja",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Indica se o pedido deve ser impresso automaticamente.");

            migrationBuilder.CreateTable(
                name: "ContaReceber",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identificador único da conta a receber.")
                        .Annotation("SqlServer:Identity", "12001, 1"),
                    LojaId = table.Column<int>(type: "int", nullable: false, comment: "Loja responsável pela conta a receber."),
                    DataRecebimento = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Data de recebimento da conta."),
                    DataVencimento = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Data de vencimento da conta."),
                    Descricao = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "Descrição da conta a receber."),
                    FuncionarioId = table.Column<int>(type: "int", nullable: false),
                    Recebido = table.Column<bool>(type: "bit", nullable: false, comment: "Indica se a conta foi recebida."),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "Status atual da conta financeira."),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "Valor da conta a receber.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContaReceber", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContaReceber_Loja_LojaId",
                        column: x => x.LojaId,
                        principalTable: "Loja",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContaReceber_LojaId",
                table: "ContaReceber",
                column: "LojaId");
        }
    }
}
