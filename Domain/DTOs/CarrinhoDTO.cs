namespace api_ecommerce.Domain.DTOs
{
    public class AddItemCarrinhoDTO
    {
        public int UsuarioId { get; set; }
        public int VariacaoId { get; set; }
        public int Quantidade { get; set; }
    }

    public class UpdateQuantidadeDTO
    {
        public int ItemCarrinhoId { get; set; }
        public int Quantidade { get; set; }
    }
}
