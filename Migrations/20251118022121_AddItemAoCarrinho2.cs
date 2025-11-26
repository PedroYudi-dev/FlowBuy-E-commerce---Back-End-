using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_ecommerce.Migrations
{
    /// <inheritdoc />
    public partial class AddItemAoCarrinho2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarrinhosItens_Produtos_ProdutoId",
                table: "CarrinhosItens");

            migrationBuilder.RenameColumn(
                name: "ProdutoId",
                table: "CarrinhosItens",
                newName: "VariacaoId");

            migrationBuilder.RenameIndex(
                name: "IX_CarrinhosItens_ProdutoId",
                table: "CarrinhosItens",
                newName: "IX_CarrinhosItens_VariacaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarrinhosItens_ProdutoVariacoes_VariacaoId",
                table: "CarrinhosItens",
                column: "VariacaoId",
                principalTable: "ProdutoVariacoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarrinhosItens_ProdutoVariacoes_VariacaoId",
                table: "CarrinhosItens");

            migrationBuilder.RenameColumn(
                name: "VariacaoId",
                table: "CarrinhosItens",
                newName: "ProdutoId");

            migrationBuilder.RenameIndex(
                name: "IX_CarrinhosItens_VariacaoId",
                table: "CarrinhosItens",
                newName: "IX_CarrinhosItens_ProdutoId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarrinhosItens_Produtos_ProdutoId",
                table: "CarrinhosItens",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
