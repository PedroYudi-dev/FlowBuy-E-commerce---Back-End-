using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_ecommerce.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVariationProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProdutoVariacao_Produtos_ProdutoId",
                table: "ProdutoVariacao");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProdutoVariacao",
                table: "ProdutoVariacao");

            migrationBuilder.RenameTable(
                name: "ProdutoVariacao",
                newName: "ProdutoVariacoes");

            migrationBuilder.RenameIndex(
                name: "IX_ProdutoVariacao_ProdutoId",
                table: "ProdutoVariacoes",
                newName: "IX_ProdutoVariacoes_ProdutoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProdutoVariacoes",
                table: "ProdutoVariacoes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProdutoVariacoes_Produtos_ProdutoId",
                table: "ProdutoVariacoes",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProdutoVariacoes_Produtos_ProdutoId",
                table: "ProdutoVariacoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProdutoVariacoes",
                table: "ProdutoVariacoes");

            migrationBuilder.RenameTable(
                name: "ProdutoVariacoes",
                newName: "ProdutoVariacao");

            migrationBuilder.RenameIndex(
                name: "IX_ProdutoVariacoes_ProdutoId",
                table: "ProdutoVariacao",
                newName: "IX_ProdutoVariacao_ProdutoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProdutoVariacao",
                table: "ProdutoVariacao",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProdutoVariacao_Produtos_ProdutoId",
                table: "ProdutoVariacao",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
