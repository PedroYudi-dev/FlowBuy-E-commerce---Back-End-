
using System.Collections.Generic;
using api_ecommerce.Domain.Entities;

namespace api_ecommerce.Domain.Interfaces.Repositories
{
    public interface IEstoqueRepository
    {
        IEnumerable<Estoque> GetAll();
        Estoque GetByProdutoId(int produtoId);
        void Add(Estoque estoque);
        void Update(Estoque estoque);
        void Delete(int produtoId);

        bool SuficienteEstoque(int produtoId, int quantidade);
        void ReduzirEstoque(int produtoId, int quantidade);
    }
}
