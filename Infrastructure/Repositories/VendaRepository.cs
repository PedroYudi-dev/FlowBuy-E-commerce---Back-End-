
using System.Collections.Generic;
using System.Linq;
using api_ecommerce.Domain.Entities;
using api_ecommerce.Domain.Interfaces.Repositories;
using api_ecommerce.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace api_ecommerce.Infrastructure.Repositories
{
    public class VendaRepository : IVendaRepository
    {
        private readonly EcommerceDbContext _context;

        public VendaRepository(EcommerceDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Venda> GetAll()
        {
            return _context.Vendas
                .Include(v => v.Cliente)
                .Include(v => v.Produto)
                .AsNoTracking()
                .ToList();
        }

        public Venda GetById(int id)
        {
            var venda = _context.Vendas
                .Include(v => v.Cliente)
                .Include(v => v.Produto)
                .AsNoTracking()
                .FirstOrDefault(v => v.Id == id);

            if (venda == null)
                throw new KeyNotFoundException($"Venda com id {id} não encontrada.");

            return venda;
        }

        public void Add(Venda venda)
        {
            _context.Vendas.Add(venda);
            _context.SaveChanges();
        }

        public void Update(Venda venda)
        {
            _context.Vendas.Attach(venda);
            _context.Entry(venda).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var venda = _context.Vendas.Find(id);
            if (venda != null)
            {
                _context.Vendas.Remove(venda);
                _context.SaveChanges();
            }
        }

        public bool ExisteVenda(int id)
        {
            return _context.Vendas.Any(v => v.Id == id);
        }
    }
}
