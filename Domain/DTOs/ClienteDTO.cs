
using System.ComponentModel.DataAnnotations;

namespace api_ecommerce.Domain.DTOs
{
    public class ClienteDTO
    {
        [Required(ErrorMessage = "O nome é obrigatório!")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres!")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CPF é obrigatório!")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve conter exatamente 11 dígitos!")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "O CPF deve conter apenas números!")]
        public string Cpf { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório!")]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido!")]
        public string Email { get; set; } = string.Empty;

        // Senha em texto (somente para criação/atualização; não retornar em respostas)
        public string? Senha { get; set; }
    }
}
