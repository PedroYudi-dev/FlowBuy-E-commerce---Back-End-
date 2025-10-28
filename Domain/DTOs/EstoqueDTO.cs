
using System;
using System.ComponentModel.DataAnnotations;

namespace api_ecommerce.Domain.DTOs
{
    public class EstoqueDTO
    {
        [Required(ErrorMessage = "O ID do produto é obrigatório!")]
        public int ProdutoId { get; set; }

        [Required(ErrorMessage = "A quantidade disponível é obrigatória!")]
        [Range(0, int.MaxValue, ErrorMessage = "A quantidade disponível não pode ser negativa!")]
        public int QuantidadeDisponivel { get; set; }

        // Opcional: normalmente essa data é gerenciada automaticamente,
        // mas se quiser que seja passada, pode incluir validação aqui.
        public DateTime UltimaAtualizacao { get; set; } = DateTime.Now;
    }
}
