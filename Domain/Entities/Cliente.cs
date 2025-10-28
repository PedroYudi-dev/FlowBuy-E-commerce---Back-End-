
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace api_ecommerce.Domain.Entities
{
    public class Cliente
    {

        // ID:
        public int Id { get; set; }

        // Nome:
        [Required(ErrorMessage = "O nome é obrigatório!")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres!")]
        public string Nome { get; set; } = string.Empty;

        // Documento:
        [Required(ErrorMessage = "O CPF é obrigatório!")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve conter exatamente 11 dígitos!")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "O CPF deve conter apenas números!")]
        public string Cpf { get; set; } = string.Empty;

        // E-mail:
        [Required(ErrorMessage = "O e-mail é obrigatório!")]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido!")]
        public string Email { get; set; } = string.Empty;

        // Relação com vendas (inicialização para evitar null):
        public ICollection<Venda> Vendas { get; set; } = new List<Venda>();

        // Data de criação do registro:
        public DateTime Data { get; set; } = DateTime.UtcNow;

        // Senha (armazenada como hash + salt):
        public string? SenhaHash { get; set; }
        public string? SenhaSalt { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}
