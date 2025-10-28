
using System.ComponentModel.DataAnnotations;

namespace api_ecommerce.Domain.DTOs
{
    public class CarrinhoItemAddDTO
    {
        [Required]
        public int ProdutoId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantidade { get; set; }
    }

    public class CarrinhoResumoDTO
    {
        public int CarrinhoId { get; set; }
        public int ClienteId { get; set; }
        public string Exibir { get; set; } = "SIM";
        public decimal Total { get; set; }
    }
}
