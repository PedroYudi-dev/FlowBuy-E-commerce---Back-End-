
using System;
using System.ComponentModel.DataAnnotations;

namespace api_ecommerce.Domain.Entities
{
    public class Estoque
    {
        // ID:
        public int Id { get; set; }

        // ID do Produto:
        [Required(ErrorMessage = "O ID do produto é obrigatório!")]
        public int ProdutoId { get; set; }

        // Quantidade disponível:
        [Required(ErrorMessage = "A quantidade disponível é obrigatória!")]
        [Range(0, int.MaxValue, ErrorMessage = "A quantidade disponível não pode ser negativa!")]
        public int QuantidadeDisponivel { get; set; }

        // Data da última atualização (inicializada com a data atual):
        public DateTime UltimaAtualizacao { get; set; } = DateTime.Now;

        // Data de criação do registro:
        public DateTime Data { get; set; } = DateTime.UtcNow;
    }
}
