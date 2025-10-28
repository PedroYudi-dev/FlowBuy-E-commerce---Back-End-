
using System.Collections.Generic;
using api_ecommerce.Domain.Entities;

namespace api_ecommerce.Domain.Interfaces.Repositories
{
    public interface IClienteRepository
    {
        IEnumerable<Cliente> GetAll();
        Cliente GetById(int id);
        void Add(Cliente cliente);
        void Update(Cliente cliente);
        void Delete(int id);
        bool ExisteCliente(int id);
    }
}
