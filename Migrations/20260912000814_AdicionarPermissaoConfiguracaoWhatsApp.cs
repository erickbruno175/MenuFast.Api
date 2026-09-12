using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MenuFast.Api.Migrations {
    /// <inheritdoc />
    public partial class AdicionarPermissaoConfiguracaoWhatsApp : Migration {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoricoAcessos_Loja_LojaId",
                table: "HistoricoAcessos");

            migrationBuilder.AlterColumn<int>(
                name: "LojaId",
                table: "HistoricoAcessos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

        

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricoAcessos_Loja_LojaId",
                table: "HistoricoAcessos",
                column: "LojaId",
                principalTable: "Loja",
                principalColumn: "Id");

            migrationBuilder.Sql("""
                IF NOT EXISTS (
                    SELECT 1
                    FROM Permissao
                    WHERE Codigo = 'CONFIGURACAO_WHATSAPP_VISUALIZAR'
                )
                BEGIN
                    INSERT INTO Permissao (Descricao, Codigo)
                    VALUES ('Configuração WhatsApp / Bot', 'CONFIGURACAO_WHATSAPP_VISUALIZAR');
                END;

                IF NOT EXISTS (
                    SELECT 1
                    FROM PerfilPermissao pp
                    INNER JOIN Permissao p ON p.Id = pp.PermissaoId
                    WHERE pp.PerfilId = 1
                      AND p.Codigo = 'CONFIGURACAO_WHATSAPP_VISUALIZAR'
                )
                BEGIN
                    INSERT INTO PerfilPermissao (PerfilId, PermissaoId)
                    SELECT 1, Id
                    FROM Permissao
                    WHERE Codigo = 'CONFIGURACAO_WHATSAPP_VISUALIZAR';
                END;
            """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.Sql("""
                DELETE FROM PerfilPermissao
                WHERE PerfilId = 1
                  AND PermissaoId IN (
                      SELECT Id
                      FROM Permissao
                      WHERE Codigo = 'CONFIGURACAO_WHATSAPP_VISUALIZAR'
                  );

                DELETE FROM Permissao
                WHERE Codigo = 'CONFIGURACAO_WHATSAPP_VISUALIZAR';
            """);

            migrationBuilder.DropForeignKey(
                name: "FK_HistoricoAcessos_Loja_LojaId",
                table: "HistoricoAcessos");


            migrationBuilder.AlterColumn<int>(
                name: "LojaId",
                table: "HistoricoAcessos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricoAcessos_Loja_LojaId",
                table: "HistoricoAcessos",
                column: "LojaId",
                principalTable: "Loja",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}