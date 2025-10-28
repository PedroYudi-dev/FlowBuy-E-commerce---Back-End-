
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace api_ecommerce.Domain.Entities
{
    public class Produto
    {
        // ID:
        public int Id { get; set; }

        // Nome do produto:
        [Required(ErrorMessage = "O nome do produto é obrigatório!")]
        [StringLength(100, ErrorMessage = "O nome do produto deve ter no máximo 100 caracteres!")]
        public string Nome { get; set; } = string.Empty;

        // Preço do produto:
        [Required(ErrorMessage = "O preço é obrigatório!")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero!")]
        public decimal Preco { get; set; }

        // ID do fornecedor:
        [Required(ErrorMessage = "O ID do fornecedor é obrigatório!")]
        public int FornecedorId { get; set; }

        public string Marca { get; set; }

        // Relação com vendas (inicialização para evitar null):
        public ICollection<Venda> Vendas { get; set; } = new List<Venda>();

        // Data de criação do registro:
        public DateTime Data { get; set; } = DateTime.UtcNow;

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
