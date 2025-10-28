
using System.ComponentModel.DataAnnotations;

namespace api_ecommerce.Domain.DTOs
{
    public class ProdutoDTO
    {
        [Required(ErrorMessage = "O nome do produto é obrigatório!")]
        [StringLength(100, ErrorMessage = "O nome do produto deve ter no máximo 100 caracteres!")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O preço do produto é obrigatório!")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero!")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "O ID do fornecedor é obrigatório!")]
        public int FornecedorId { get; set; }

        public string Marca { get; set; }

        // Imagens:
        public string? Imagem1_base64 { get; set; }
        public string? Imagem1_cor_nome { get; set; }
        public string? Imagem1_cor_codigo { get; set; }

        public string? Imagem2_base64 { get; set; }
        public string? Imagem2_cor_nome { get; set; }
        public string? Imagem2_cor_codigo { get; set; }

        public string? Imagem3_base64 { get; set; }
        public string? Imagem3_cor_nome { get; set; }
        public string? Imagem3_cor_codigo { get; set; }
    }
}
