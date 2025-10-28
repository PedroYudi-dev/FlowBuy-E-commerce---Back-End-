
using System.Collections.Generic;
using api_ecommerce.Domain.Entities;

namespace api_ecommerce.Domain.Interfaces.Repositories
{
    public interface IFornecedorRepository
    {
        IEnumerable<Fornecedor> GetAll();
        Fornecedor GetById(int id);
        void Add(Fornecedor fornecedor);
        void Update(Fornecedor fornecedor);
        void Delete(int id);
        bool ExisteFornecedor(int id);
    }
}
