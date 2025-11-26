using api_ecommerce.Domain.Entities;

namespace api_ecommerce.Domain.Interfaces.Repositories
{
    public interface IProdutoVariacaoRepository
    {
        ProdutoVariacao? GetById(int id);
        IEnumerable<ProdutoVariacao> GetByProdutoId(int produtoId);
        void Update(ProdutoVariacao variacao);
    }
}