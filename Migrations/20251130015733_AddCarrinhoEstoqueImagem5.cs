using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_ecommerce.Migrations
{
    /// <inheritdoc />
    public partial class AddCarrinhoEstoqueImagem5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItensCarrinho_ProdutoVariacoes_VariacaoId",
                table: "ItensCarrinho");

            migrationBuilder.DropIndex(
                name: "IX_ItensCarrinho_VariacaoId",
                table: "ItensCarrinho");

            migrationBuilder.DropColumn(
                name: "QuantidadeDisponivel",
                table: "ItensCarrinho");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuantidadeDisponivel",
                table: "ItensCarrinho",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ItensCarrinho_VariacaoId",
                table: "ItensCarrinho",
                column: "VariacaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItensCarrinho_ProdutoVariacoes_VariacaoId",
                table: "ItensCarrinho",
                column: "VariacaoId",
                principalTable: "ProdutoVariacoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
