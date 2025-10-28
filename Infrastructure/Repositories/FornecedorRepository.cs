
using System.Collections.Generic;
using System.Linq;
using api_ecommerce.Domain.Entities;
using api_ecommerce.Domain.Interfaces.Repositories;
using api_ecommerce.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace api_ecommerce.Infrastructure.Repositories
{
    public class FornecedorRepository : IFornecedorRepository
    {
        private readonly EcommerceDbContext _context;

        public FornecedorRepository(EcommerceDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Fornecedor> GetAll()
        {
            return _context.Fornecedores.AsNoTracking().ToList();
        }

        public Fornecedor GetById(int id)
        {
            var fornecedor = _context.Fornecedores
                //.Include(f => f.AlgumRelacionamento) // caso tenha relacionamento, incluir aqui
                .AsNoTracking()
                .FirstOrDefault(f => f.Id == id);

            if (fornecedor == null)
                throw new KeyNotFoundException($"Fornecedor com id {id} não encontrado.");

            return fornecedor;
        }

        public void Add(Fornecedor fornecedor)
        {
            _context.Fornecedores.Add(fornecedor);
            _context.SaveChanges();
        }

        public void Update(Fornecedor fornecedor)
        {
            _context.Fornecedores.Attach(fornecedor);
            _context.Entry(fornecedor).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var fornecedor = _context.Fornecedores.Find(id);
            if (fornecedor != null)
            {
                _context.Fornecedores.Remove(fornecedor);
                _context.SaveChanges();
            }
        }

        public bool ExisteFornecedor(int id)
        {
            return _context.Fornecedores.Any(f => f.Id == id);
        }
    }
}
