using System.ComponentModel.DataAnnotations;

namespace api_ecommerce.Domain.DTOs
{
    public class ProdutoComEstoqueDTO
    {
        [Required(ErrorMessage = "O nome do produto é obrigatório!")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O preço é obrigatório!")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero!")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "O ID do fornecedor é obrigatório!")]
        public int FornecedorId { get; set; }

        public string? Marca { get; set; }

        public string? Imagem1_base64 { get; set; }
        public string? Imagem1_cor_nome { get; set; }
        public string? Imagem1_cor_codigo { get; set; }

        public string? Imagem2_base64 { get; set; }
        public string? Imagem2_cor_nome { get; set; }
        public string? Imagem2_cor_codigo { get; set; }

        public string? Imagem3_base64 { get; set; }
        public string? Imagem3_cor_nome { get; set; }
        public string? Imagem3_cor_codigo { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "A quantidade inicial não pode ser negativa.")]
        public int QuantidadeInicial { get; set; } = 0;
    }
}
