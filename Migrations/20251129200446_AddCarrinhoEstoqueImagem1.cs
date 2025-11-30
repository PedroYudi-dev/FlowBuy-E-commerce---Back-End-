using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_ecommerce.Migrations
{
    /// <inheritdoc />
    public partial class AddCarrinhoEstoqueImagem1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CorCodigo",
                table: "ItensCarrinho",
                type: "longtext",
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "CorNome",
                table: "ItensCarrinho",
                type: "longtext",
                nullable: false,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ImagemBase64",
                table: "ItensCarrinho",
                type: "longtext",
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "QuantidadeDisponivel",
                table: "ItensCarrinho",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorCodigo",
                table: "ItensCarrinho");

            migrationBuilder.DropColumn(
                name: "CorNome",
                table: "ItensCarrinho");

            migrationBuilder.DropColumn(
                name: "ImagemBase64",
                table: "ItensCarrinho");

            migrationBuilder.DropColumn(
                name: "QuantidadeDisponivel",
                table: "ItensCarrinho");
        }
    }
}
