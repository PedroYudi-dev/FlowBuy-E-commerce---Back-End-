namespace api_ecommerce.Domain.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        public int Nota { get; set; } // 1 a 5
        public string Comentario { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    }
}
