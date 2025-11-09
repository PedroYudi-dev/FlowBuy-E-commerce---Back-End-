using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_ecommerce.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoEstoqueVariaçõesparte2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProdutoVariacaoId",
                table: "Estoques",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Estoques_ProdutoVariacaoId",
                table: "Estoques",
                column: "ProdutoVariacaoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Estoques_ProdutoVariacoes_ProdutoVariacaoId",
                table: "Estoques",
                column: "ProdutoVariacaoId",
                principalTable: "ProdutoVariacoes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estoques_ProdutoVariacoes_ProdutoVariacaoId",
                table: "Estoques");

            migrationBuilder.DropIndex(
                name: "IX_Estoques_ProdutoVariacaoId",
                table: "Estoques");

            migrationBuilder.DropColumn(
                name: "ProdutoVariacaoId",
                table: "Estoques");
        }
    }
}
