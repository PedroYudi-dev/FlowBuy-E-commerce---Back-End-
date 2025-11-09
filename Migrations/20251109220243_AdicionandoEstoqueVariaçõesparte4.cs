using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_ecommerce.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoEstoqueVariaçõesparte4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estoques_ProdutoVariacoes_ProdutoVariacaoId",
                table: "Estoques");

            migrationBuilder.AddForeignKey(
                name: "FK_Estoques_ProdutoVariacoes_ProdutoVariacaoId",
                table: "Estoques",
                column: "ProdutoVariacaoId",
                principalTable: "ProdutoVariacoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estoques_ProdutoVariacoes_ProdutoVariacaoId",
                table: "Estoques");

            migrationBuilder.AddForeignKey(
                name: "FK_Estoques_ProdutoVariacoes_ProdutoVariacaoId",
                table: "Estoques",
                column: "ProdutoVariacaoId",
                principalTable: "ProdutoVariacoes",
                principalColumn: "Id");
        }
    }
}
