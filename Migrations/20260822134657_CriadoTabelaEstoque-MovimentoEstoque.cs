using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MenuFast.Api.Migrations
{
    /// <inheritdoc />
    public partial class CriadoTabelaEstoqueMovimentoEstoque : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComplementoProduto");

            migrationBuilder.DropTable(
                name: "OpcaoProduto");

            migrationBuilder.DropTable(
                name: "Complemento");

            migrationBuilder.DropColumn(
                name: "ProdutoEsgotado",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "ControlaEstoque",
                table: "ConfiguracaoLoja");

            migrationBuilder.AlterColumn<bool>(
                name: "ControlaEstoque",
                table: "Produto",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Indica se o produto controla estoque.");

            migrationBuilder.CreateTable(
                name: "EstoqueProduto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identificador único do estoque do produto.")
                        .Annotation("SqlServer:Identity", "1001, 1"),
                    ProdutoId = table.Column<int>(type: "int", nullable: false, comment: "Produto vinculado ao estoque."),
                    Quantidade = table.Column<int>(type: "int", nullable: false, comment: "Quantidade atual disponível em estoque."),
                    EstoqueMinimo = table.Column<int>(type: "int", nullable: false, comment: "Quantidade mínima de estoque utilizada para gerar alerta de estoque baixo."),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Data e hora em que o controle de estoque foi cadastrado."),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Data e hora da última atualização do estoque."),
                    AlertaEstoqueEnviado = table.Column<bool>(type: "bit", nullable: false, comment: "Indica se o alerta de estoque baixo já foi enviado."),
                    UltimoAlertaEstoque = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Data e hora do último alerta de estoque enviado.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstoqueProduto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstoqueProduto_Produto_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovimentacaoEstoque",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identificador único da movimentação de estoque.")
                        .Annotation("SqlServer:Identity", "1001, 1"),
                    EstoqueProdutoId = table.Column<int>(type: "int", nullable: false, comment: "Estoque do produto vinculado à movimentação."),
                    Tipo = table.Column<int>(type: "int", nullable: false, comment: "Tipo da movimentação realizada no estoque."),
                    Quantidade = table.Column<int>(type: "int", nullable: false, comment: "Quantidade movimentada no estoque."),
                    QuantidadeAnterior = table.Column<int>(type: "int", nullable: false, comment: "Quantidade disponível no estoque antes da movimentação."),
                    QuantidadeAtual = table.Column<int>(type: "int", nullable: false, comment: "Quantidade disponível no estoque após a movimentação."),
                    Observacao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Observação referente à movimentação de estoque."),
                    PedidoId = table.Column<int>(type: "int", nullable: true, comment: "Pedido relacionado à movimentação, quando aplicável."),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Data e hora em que a movimentação foi registrada.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimentacaoEstoque", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovimentacaoEstoque_EstoqueProduto_EstoqueProdutoId",
                        column: x => x.EstoqueProdutoId,
                        principalTable: "EstoqueProduto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EstoqueProduto_ProdutoId",
                table: "EstoqueProduto",
                column: "ProdutoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MovimentacaoEstoque_EstoqueProdutoId",
                table: "MovimentacaoEstoque",
                column: "EstoqueProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_MovimentacaoEstoque_PedidoId",
                table: "MovimentacaoEstoque",
                column: "PedidoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovimentacaoEstoque");

            migrationBuilder.DropTable(
                name: "EstoqueProduto");

            migrationBuilder.AlterColumn<bool>(
                name: "ControlaEstoque",
                table: "Produto",
                type: "bit",
                nullable: false,
                comment: "Indica se o produto controla estoque.",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<bool>(
                name: "ProdutoEsgotado",
                table: "Produto",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Indica se o produto está ativo.");

            migrationBuilder.AddColumn<bool>(
                name: "ControlaEstoque",
                table: "ConfiguracaoLoja",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Indica se o restaurante utiliza controle de estoque.");

            migrationBuilder.CreateTable(
                name: "Complemento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identificador único do complemento.")
                        .Annotation("SqlServer:Identity", "1001, 1"),
                    LojaId = table.Column<int>(type: "int", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, comment: "Indica se o complemento está ativo."),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Nome do complemento."),
                    Obrigatorio = table.Column<bool>(type: "bit", nullable: false, comment: "Indica se o complemento é obrigatório."),
                    Preco = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "Valor adicional do complemento.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Complemento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Complemento_Loja_LojaId",
                        column: x => x.LojaId,
                        principalTable: "Loja",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OpcaoProduto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identificador único da opção do produto.")
                        .Annotation("SqlServer:Identity", "1001, 1"),
                    ProdutoId = table.Column<int>(type: "int", nullable: false, comment: "Produto ao qual a opção pertence."),
                    Acrescimo = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "Valor de acréscimo da opção."),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Nome da opção do produto.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpcaoProduto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpcaoProduto_Produto_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComplementoProduto",
                columns: table => new
                {
                    ComplementosId = table.Column<int>(type: "int", nullable: false),
                    ProdutosId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplementoProduto", x => new { x.ComplementosId, x.ProdutosId });
                    table.ForeignKey(
                        name: "FK_ComplementoProduto_Complemento_ComplementosId",
                        column: x => x.ComplementosId,
                        principalTable: "Complemento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComplementoProduto_Produto_ProdutosId",
                        column: x => x.ProdutosId,
                        principalTable: "Produto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Complemento_LojaId",
                table: "Complemento",
                column: "LojaId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplementoProduto_ProdutosId",
                table: "ComplementoProduto",
                column: "ProdutosId");

            migrationBuilder.CreateIndex(
                name: "IX_OpcaoProduto_ProdutoId",
                table: "OpcaoProduto",
                column: "ProdutoId");
        }
    }
}
