namespace api_ecommerce.Domain.DTOs
{
    public class ProdutoUpdateDTO
    {
        public string? Nome { get; set; }

        public string? Descricao { get; set; }

        public decimal? Preco { get; set; }

        public int? FornecedorId { get; set; }

        public string? Marca { get; set; }

        public int QuantidadeInicial { get; set; }

        public string? ImagemPrincipalBase64 { get; set; }

        public string? CorNomePrincipal { get; set; }

        public string? CorCodigoPrincipal { get; set; }

        public List<ProdutoUpdateVariacaoDTO>? Variacoes { get; set; }
    }

    public class ProdutoUpdateVariacaoDTO
    {
        public string? CorNome { get; set; }
        public string? CorCodigo { get; set; }
        public string? ImagemBase64 { get; set; }
    }
}
