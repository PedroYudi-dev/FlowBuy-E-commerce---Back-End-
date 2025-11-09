using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_ecommerce.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandPrecoVariaçõesparte1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Preco",
                table: "ProdutoVariacoes",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Preco",
                table: "ProdutoVariacoes");
        }
    }
}
