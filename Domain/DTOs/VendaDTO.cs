
using System.ComponentModel.DataAnnotations;

namespace api_ecommerce.Domain.DTOs
{
    public class VendaDTO
    {
        [Required(ErrorMessage = "O ID do cliente é obrigatório!")]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "O ID do produto é obrigatório!")]
        public int ProdutoId { get; set; }

        [Required(ErrorMessage = "A quantidade é obrigatória!")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser no mínimo 1.")]
        public int Quantidade { get; set; }
    }
}
