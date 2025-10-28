
using System.Collections.Generic;
using System.Linq;
using api_ecommerce.Domain.Entities;
using api_ecommerce.Domain.Interfaces.Repositories;
using api_ecommerce.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace api_ecommerce.Infrastructure.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly EcommerceDbContext _context;

        public ClienteRepository(EcommerceDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Cliente> GetAll()
        {
            return _context.Clientes.AsNoTracking().ToList();
        }

        public Cliente GetById(int id)
        {
            var cliente = _context.Clientes
                .Include(c => c.Vendas)
                .AsNoTracking()
                .FirstOrDefault(c => c.Id == id);

            if (cliente == null)
                throw new KeyNotFoundException($"Cliente com id {id} não encontrado.");

            return cliente;
        }

        public void Add(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            _context.SaveChanges();
        }

        public void Update(Cliente cliente)
        {
            _context.Clientes.Attach(cliente);
            _context.Entry(cliente).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var cliente = _context.Clientes.Find(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
                _context.SaveChanges();
            }
        }

        public bool ExisteCliente(int id)
        {
            return _context.Clientes.Any(c => c.Id == id);
        }
    }
}
