using System.ComponentModel.DataAnnotations;

namespace api_ecommerce.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string SenhaHash { get; set; } = string.Empty;

        [Required]
        public string SenhaSalt { get; set; } = string.Empty;
        
        [Required]
        public string Role { get; set; } = string.Empty;

    }
}
