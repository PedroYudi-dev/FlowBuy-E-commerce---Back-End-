using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace api_ecommerce.Domain.Entities
{
    public class ProdutoVariacao
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProdutoId { get; set; }
        
        [JsonIgnore]
        [ForeignKey("ProdutoId")]
        public Produto Produto { get; set; }

        // Nome da variação (ex: "Vermelho", "Azul")
        [Required(ErrorMessage = "O nome da cor é obrigatório.")]
        public string CorNome { get; set; }

        public string? CorCodigo { get; set; }
        public decimal Preco { get; set; }

        public string? ImagemBase64 { get; set; }


        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public Estoque? Estoque { get; set; }
    }
}
