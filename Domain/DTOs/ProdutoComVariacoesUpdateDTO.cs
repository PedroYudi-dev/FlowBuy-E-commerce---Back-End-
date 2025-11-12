namespace api_ecommerce.Domain.DTOs
{
    public class ProdutoComVariacoesUpdateDTO
    {
        public int Id { get; set; }

        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public decimal? Preco { get; set; }
        public int? FornecedorId { get; set; }
        public string? Marca { get; set; }

        public string? ImagemPrincipalBase64 { get; set; }
        public string? CorNomePrincipal { get; set; }
        public string? CorCodigoPrincipal { get; set; }

        public List<ProdutoVariacaoUpdateDTO>? Variacoes { get; set; } = new();
    }

    public class ProdutoVariacaoUpdateDTO
    {
        public int Id { get; set; } 
        public string? CorNome { get; set; }
        public string? CorCodigo { get; set; }
        public string? ImagemBase64 { get; set; }
        public int? QuantidadeEstoque { get; set; }
        public decimal? Preco { get; set; }
    }
}

