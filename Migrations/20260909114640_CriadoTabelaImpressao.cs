using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MenuFast.Api.Migrations
{
    /// <inheritdoc />
    public partial class CriadoTabelaImpressao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Telefone",
                table: "Funcionario",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.CreateTable(
                name: "ConfiguracaoImpressao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Identificador único da configuração de impressão.")
                        .Annotation("SqlServer:Identity", "1001, 1"),
                    LojaId = table.Column<int>(type: "int", nullable: false, comment: "Identificador da loja."),
                    ImprimirPedidoAoEnviar = table.Column<bool>(type: "bit", nullable: false, comment: "Define se o pedido deve ser impresso automaticamente ao ser enviado."),
                    ImprimirPedidoCozinha = table.Column<bool>(type: "bit", nullable: false, comment: "Define se o pedido deve ser impresso na cozinha."),
                    ImprimirPedidoBar = table.Column<bool>(type: "bit", nullable: false, comment: "Define se o pedido deve ser impresso no bar."),
                    ImpressoraPadrao = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "Nome da impressora padrão."),
                    LarguraPapel = table.Column<int>(type: "int", nullable: false, comment: "Largura do papel da impressora em milímetros."),
                    MostrarNomeLoja = table.Column<bool>(type: "bit", nullable: false, comment: "Define se o nome da loja será exibido no comprovante."),
                    MostrarCnpj = table.Column<bool>(type: "bit", nullable: false, comment: "Define se o CNPJ da loja será exibido no comprovante."),
                    MostrarEndereco = table.Column<bool>(type: "bit", nullable: false, comment: "Define se o endereço da loja será exibido no comprovante."),
                    MostrarTelefone = table.Column<bool>(type: "bit", nullable: false, comment: "Define se o telefone da loja será exibido no comprovante."),
                    MensagemRodape = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Mensagem exibida no rodapé do comprovante.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracaoImpressao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfiguracaoImpressao_Loja_LojaId",
                        column: x => x.LojaId,
                        principalTable: "Loja",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracaoImpressao_LojaId",
                table: "ConfiguracaoImpressao",
                column: "LojaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfiguracaoImpressao");

            migrationBuilder.AlterColumn<string>(
                name: "Telefone",
                table: "Funcionario",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);
        }
    }
}
