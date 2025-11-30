using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_ecommerce.Domain.Models
{
    public class Carrinho
    {
        [Key]
        public int Id { get; set; }

        // Usuário dono do carrinho
        public int UsuarioId { get; set; }

        public ICollection<ItemCarrinho> Itens { get; set; } = new List<ItemCarrinho>();
        public decimal Total { get; set; }
    }
}
