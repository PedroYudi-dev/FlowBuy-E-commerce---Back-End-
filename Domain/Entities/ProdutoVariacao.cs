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

        // Código hexadecimal da cor (ex: "#FF0000")
        public string? CorCodigo { get; set; }

        // Imagem específica da variação (em base64)
        public string? ImagemBase64 { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    }
}
