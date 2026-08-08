using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MenuFast.Api.Migrations
{
    /// <inheritdoc />
    public partial class ajustetabelas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormaPagamento_ProvedorPagamento_ProvedorPagamentoId",
                table: "FormaPagamento");

            migrationBuilder.DropForeignKey(
                name: "FK_Funcionario_Funcao_FuncaoId",
                table: "Funcionario");

            migrationBuilder.DropTable(
                name: "ConfiguracaoProvedorPagamento");

            migrationBuilder.DropTable(
                name: "Funcao");

            migrationBuilder.DropTable(
                name: "TemplateEmail");

            migrationBuilder.DropTable(
                name: "ProvedorPagamento");

            migrationBuilder.DropIndex(
                name: "IX_Funcionario_FuncaoId",
                table: "Funcionario");

            migrationBuilder.DropIndex(
                name: "IX_FormaPagamento_ProvedorPagamentoId",
                table: "FormaPagamento");

            migrationBuilder.DropColumn(
                name: "ProvedorPagamentoId",
                table: "FormaPagamento");

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "PerfilPermissao",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<decimal>(
                name: "Salario",
                table: "Funcionario",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataExpiracaoSenha",
                table: "Funcionario",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ConfiguracaoSeguranca",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LojaId = table.Column<int>(type: "int", nullable: false),
                    MaxTentativasLogin = table.Column<int>(type: "int", nullable: false),
                    TempoBloqueioMinutos = table.Column<int>(type: "int", nullable: false),
                    TempoExpiracaoSessaoDias = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracaoSeguranca", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfiguracaoSeguranca_Loja_LojaId",
                        column: x => x.LojaId,
                        principalTable: "Loja",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TemplatesEmail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LojaId = table.Column<int>(type: "int", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Assunto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Conteudo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplatesEmail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplatesEmail_Loja_LojaId",
                        column: x => x.LojaId,
                        principalTable: "Loja",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracaoSeguranca_LojaId",
                table: "ConfiguracaoSeguranca",
                column: "LojaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TemplatesEmail_LojaId",
                table: "TemplatesEmail",
                column: "LojaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfiguracaoSeguranca");

            migrationBuilder.DropTable(
                name: "TemplatesEmail");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "PerfilPermissao");

            migrationBuilder.DropColumn(
                name: "DataExpiracaoSenha",
                table: "Funcionario");

            migrationBuilder.AlterColumn<decimal>(
                name: "Salario",
                table: "Funcionario",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProvedorPagamentoId",
                table: "FormaPagamento",
                type: "int",
                nullable: true,
                comment: "Provedor de pagamento vinculado à forma de pagamento.");

            migrationBuilder.CreateTable(
                name: "Funcao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funcao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProvedorPagamento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identificador único do provedor de pagamento.")
                        .Annotation("SqlServer:Identity", "1001, 1"),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, comment: "Indica se o provedor de pagamento está ativo."),
                    Codigo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Código interno do provedor de pagamento."),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Nome do provedor de pagamento.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProvedorPagamento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TemplateEmail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LojaId = table.Column<int>(type: "int", nullable: false),
                    Assunto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    Conteudo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateEmail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateEmail_Loja_LojaId",
                        column: x => x.LojaId,
                        principalTable: "Loja",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConfiguracaoProvedorPagamento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identificador único da configuração do provedor de pagamento.")
                        .Annotation("SqlServer:Identity", "1001, 1"),
                    LojaId = table.Column<int>(type: "int", nullable: false, comment: "Loja vinculada ao provedor de pagamento."),
                    ProvedorPagamentoId = table.Column<int>(type: "int", nullable: false, comment: "Provedor de pagamento utilizado pela loja."),
                    Ativo = table.Column<bool>(type: "bit", nullable: false, comment: "Indica se a configuração do provedor está ativa."),
                    ChaveApi = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Chave de acesso da API do provedor de pagamento."),
                    SecretKey = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Chave secreta do provedor de pagamento."),
                    Token = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Token de autenticação do provedor de pagamento.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracaoProvedorPagamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfiguracaoProvedorPagamento_Loja_LojaId",
                        column: x => x.LojaId,
                        principalTable: "Loja",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConfiguracaoProvedorPagamento_ProvedorPagamento_ProvedorPagamentoId",
                        column: x => x.ProvedorPagamentoId,
                        principalTable: "ProvedorPagamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Funcionario_FuncaoId",
                table: "Funcionario",
                column: "FuncaoId");

            migrationBuilder.CreateIndex(
                name: "IX_FormaPagamento_ProvedorPagamentoId",
                table: "FormaPagamento",
                column: "ProvedorPagamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracaoProvedorPagamento_LojaId",
                table: "ConfiguracaoProvedorPagamento",
                column: "LojaId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracaoProvedorPagamento_ProvedorPagamentoId",
                table: "ConfiguracaoProvedorPagamento",
                column: "ProvedorPagamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Funcao_Nome",
                table: "Funcao",
                column: "Nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TemplateEmail_LojaId",
                table: "TemplateEmail",
                column: "LojaId");

            migrationBuilder.AddForeignKey(
                name: "FK_FormaPagamento_ProvedorPagamento_ProvedorPagamentoId",
                table: "FormaPagamento",
                column: "ProvedorPagamentoId",
                principalTable: "ProvedorPagamento",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Funcionario_Funcao_FuncaoId",
                table: "Funcionario",
                column: "FuncaoId",
                principalTable: "Funcao",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
