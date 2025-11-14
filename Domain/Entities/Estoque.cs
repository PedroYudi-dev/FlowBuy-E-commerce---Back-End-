
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace api_ecommerce.Domain.Entities
{
    public class Estoque
    {
        // ID:
        public int Id { get; set; }

        // ID do Produto:
        public int? ProdutoId { get; set; }
        public int? ProdutoVariacaoId { get; set; }


        // Quantidade disponível:
        [Required(ErrorMessage = "A quantidade disponível é obrigatória!")]
        [Range(0, int.MaxValue, ErrorMessage = "A quantidade disponível não pode ser negativa!")]
        public int QuantidadeDisponivel { get; set; }

        // Data da última atualização (inicializada com a data atual):
        public DateTime UltimaAtualizacao { get; set; } = DateTime.Now;

        // Data de criação do registro:
        public DateTime Data { get; set; } = DateTime.UtcNow;

        [ForeignKey("ProdutoId")]
        [JsonIgnore]
        public Produto Produto { get; set; }

        [ForeignKey("ProdutoVariacaoId")]
        [JsonIgnore]
        public ProdutoVariacao? ProdutoVariacao { get; set; }
    }
}
