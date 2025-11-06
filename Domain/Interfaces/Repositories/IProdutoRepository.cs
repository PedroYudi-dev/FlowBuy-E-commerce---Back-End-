
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
        void Delete(int id);
        bool ExisteProduto(int id);
    }
}
