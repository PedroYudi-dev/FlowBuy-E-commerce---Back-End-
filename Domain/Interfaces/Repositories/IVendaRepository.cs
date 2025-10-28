
using System.Collections.Generic;
using api_ecommerce.Domain.Entities;

namespace api_ecommerce.Domain.Interfaces.Repositories
{
    public interface IVendaRepository
    {
        IEnumerable<Venda> GetAll();
        Venda GetById(int id);
        void Add(Venda venda);
        void Update(Venda venda);
        void Delete(int id);
        bool ExisteVenda(int id);
    }
}
