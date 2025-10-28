
using System;
using System.ComponentModel.DataAnnotations;

namespace api_ecommerce.Domain.Entities
{
    public class Venda
    {
        // ID:
        public int Id { get; set; }

        // ID do Cliente:
        [Required(ErrorMessage = "O ID do cliente é obrigatório!")]
        public int ClienteId { get; set; }

        // Propriedade de navegação para o cliente:
        public Cliente Cliente { get; set; } = null!;

        // ID do Produto:
        [Required(ErrorMessage = "O ID do produto é obrigatório!")]
        public int ProdutoId { get; set; }

        // Propriedade de navegação para o produto:
        public Produto Produto { get; set; } = null!;

        // Quantidade vendida:
        [Required(ErrorMessage = "A quantidade é obrigatória!")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser de pelo menos 1 unidade!")]
        public int Quantidade { get; set; }

        // Data de criação do registro:
        public DateTime Data { get; set; } = DateTime.UtcNow;

    }
}
