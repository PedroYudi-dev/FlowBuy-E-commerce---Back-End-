using System.ComponentModel.DataAnnotations;

namespace api_ecommerce.Domain.DTOs
{
    public class ProdutoComVariacoesDTO
    {
        [Required]
        public string Nome { get; set; }

        [Required]
        public decimal Preco { get; set; }

        [Required]
        public int FornecedorId { get; set; }

        public string? Marca { get; set; }

        [Range(0, int.MaxValue)]
        public int QuantidadeInicial { get; set; }

        public string? ImagemPrincipalBase64 { get; set; }
        public string? CorNomePrincipal { get; set; }
        public string? CorCodigoPrincipal { get; set; }
        // 🔹 Lista de variações (cor, imagem, etc.)
        public List<ProdutoVariacaoDTO> Variacoes { get; set; } = new();
    }

    public class ProdutoVariacaoDTO
    {
        public string CorNome { get; set; }
        public string? CorCodigo { get; set; }
        public string? ImagemBase64 { get; set; }
    }
}
