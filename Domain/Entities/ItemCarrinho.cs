using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api_ecommerce.Domain.Entities;

namespace api_ecommerce.Domain.Models
{
    public class ItemCarrinho
    {
        [Key]
        public int Id { get; set; }

        public int CarrinhoId { get; set; }
        public Carrinho Carrinho { get; set; }

        public int VariacaoId { get; set; }
        public ProdutoVariacao Variacao { get; set; }
        public int ProdutoId { get; set; }
        public string NomeProduto { get; set; }
        public decimal Preco { get; set; }

        public int Quantidade { get; set; }
        public int QuantidadeDisponivel { get; set; }
        public string? CorCodigo { get; set; }
        public string? ImagemBase64 { get; set; }
        public string CorNome { get; set; }
        public decimal Subtotal { get; set; }


    }
}
