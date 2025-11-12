
using System.Collections.Generic;
using api_ecommerce.Domain.Entities;

namespace api_ecommerce.Domain.Interfaces.Repositories
{
    public interface IProdutoRepository
    {
        IQueryable<Produto> GetAll();
        Produto? GetById(int id);
        Produto Add(Produto produto);
        void Update(Produto produto);
        void DeleteProdutoCompleto(int id);
        bool ExisteProduto(int id);
        IEnumerable<Produto> GetByFornecedorId(int fornecedorId);
        Task<Produto?> GetProdutoComVariacoesAsync(int produtoId);
        Task UpdateAsync(Produto produto); // 🔹 novo método assíncrono

        IEnumerable<Produto> GetByMarca(string marca);

    }
}
