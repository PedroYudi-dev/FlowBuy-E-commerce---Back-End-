
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace api_ecommerce.Domain.Entities
{
    public class Carrinho
    {
        public int Id { get; set; }

        [Required]
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; } = null!;

        // "SIM" enquanto não finalizado; depois "NAO"
        [Required]
        [StringLength(3)]
        public string Exibir { get; set; } = "SIM";

        public DateTime Data { get; set; } = DateTime.UtcNow;

        public ICollection<CarrinhoItem> Itens { get; set; } = new List<CarrinhoItem>();
    }

    public class CarrinhoItem
    {
        public int Id { get; set; }

        [Required]
        public int CarrinhoId { get; set; }
        public Carrinho Carrinho { get; set; } = null!;

        [Required]
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; } = null!;

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantidade { get; set; }

        [Range(0, double.MaxValue)]
        public decimal PrecoUnitario { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Subtotal { get; set; }

        public DateTime Data { get; set; } = DateTime.UtcNow;
    }
}
