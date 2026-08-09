using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MenuFast.Api.Migrations
{
    /// <inheritdoc />
    public partial class AjsuteFuncionerio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Funcionario_Login",
                table: "Funcionario");

            migrationBuilder.DropColumn(
                name: "Login",
                table: "Funcionario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Login",
                table: "Funcionario",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Funcionario_Login",
                table: "Funcionario",
                column: "Login",
                unique: true);
        }
    }
}
