using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MenuFast.Api.Migrations
{
    /// <inheritdoc />
    public partial class ajuste : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoriaProduto_Cardapio_CardapioId",
                table: "CategoriaProduto");

            migrationBuilder.DropTable(
                name: "Cardapio");

            migrationBuilder.DropIndex(
                name: "IX_CategoriaProduto_CardapioId",
                table: "CategoriaProduto");

            migrationBuilder.DropColumn(
                name: "CodigoBarras",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "Custo",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "CategoriaProduto");

            migrationBuilder.DropColumn(
                name: "CardapioId",
                table: "CategoriaProduto");

            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "CategoriaProduto");

            migrationBuilder.DropColumn(
                name: "Ordem",
                table: "CategoriaProduto");

            migrationBuilder.AlterColumn<bool>(
                name: "Ativo",
                table: "Produto",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Indica se o produto está ativo.");

            migrationBuilder.AddColumn<string>(
                name: "Codigo",
                table: "Produto",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCadastro",
                table: "Produto",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                table: "Produto",
                type: "nvarchar(max)",
                nullable: true,
                comment: "Indica os igredientes ");

            migrationBuilder.AddColumn<bool>(
                name: "ProdutoEsgotado",
                table: "Produto",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Indica se o produto está ativo.");

            migrationBuilder.CreateIndex(
                name: "IX_Pedido_ClienteId",
                table: "Pedido",
                column: "ClienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedido_Cliente_ClienteId",
                table: "Pedido",
                column: "ClienteId",
                principalTable: "Cliente",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedido_Cliente_ClienteId",
                table: "Pedido");

            migrationBuilder.DropIndex(
                name: "IX_Pedido_ClienteId",
                table: "Pedido");

            migrationBuilder.DropColumn(
                name: "Codigo",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "DataCadastro",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "ProdutoEsgotado",
                table: "Produto");

            migrationBuilder.AlterColumn<bool>(
                name: "Ativo",
                table: "Produto",
                type: "bit",
                nullable: false,
                comment: "Indica se o produto está ativo.",
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<string>(
                name: "CodigoBarras",
                table: "Produto",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                comment: "Código de barras do produto.");

            migrationBuilder.AddColumn<decimal>(
                name: "Custo",
                table: "Produto",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                comment: "Custo do produto.");

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "CategoriaProduto",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Indica se a categoria está ativa.");

            migrationBuilder.AddColumn<int>(
                name: "CardapioId",
                table: "CategoriaProduto",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Cardápio ao qual a categoria pertence.");

            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                table: "CategoriaProduto",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                comment: "Descrição da categoria.");

            migrationBuilder.AddColumn<int>(
                name: "Ordem",
                table: "CategoriaProduto",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Ordem de exibição da categoria no cardápio.");

            migrationBuilder.CreateTable(
                name: "Cardapio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identificador único do cardápio.")
                        .Annotation("SqlServer:Identity", "1001, 1"),
                    LojaId = table.Column<int>(type: "int", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, comment: "Indica se o cardápio está ativo."),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Data de cadastro do cardápio."),
                    Descricao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Descrição do cardápio."),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Nome do cardápio.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cardapio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cardapio_Loja_LojaId",
                        column: x => x.LojaId,
                        principalTable: "Loja",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoriaProduto_CardapioId",
                table: "CategoriaProduto",
                column: "CardapioId");

            migrationBuilder.CreateIndex(
                name: "IX_Cardapio_LojaId",
                table: "Cardapio",
                column: "LojaId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoriaProduto_Cardapio_CardapioId",
                table: "CategoriaProduto",
                column: "CardapioId",
                principalTable: "Cardapio",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
