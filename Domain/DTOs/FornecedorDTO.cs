
using System.ComponentModel.DataAnnotations;

namespace api_ecommerce.Domain.DTOs
{
    public class FornecedorDTO
    {
        [Required(ErrorMessage = "O nome é obrigatório!")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres!")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CNPJ é obrigatório!")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "O CNPJ deve conter exatamente 14 dígitos!")]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "O CNPJ deve conter apenas números!")]
        public string Cnpj { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório!")]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido!")]
        public string Email { get; set; } = string.Empty;

        // Senha em texto (somente para criação/atualização; não retornar em respostas)
        public string? Senha { get; set; }
    }
}
