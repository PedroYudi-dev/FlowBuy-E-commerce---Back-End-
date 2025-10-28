
using System.Collections.Generic;
using System.Linq;
using api_ecommerce.Domain.Entities;
using api_ecommerce.Domain.Interfaces.Repositories;
using api_ecommerce.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace api_ecommerce.Infrastructure.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly EcommerceDbContext _context;

        public ProdutoRepository(EcommerceDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Produto> GetAll()
        {
            return _context.Produtos.AsNoTracking().ToList();
        }

        public Produto GetById(int id)
        {
            var produto = _context.Produtos
                //.Include(p => p.Relacionamentos) // Se tiver relacionamentos, inclua aqui
                .AsNoTracking()
                .FirstOrDefault(p => p.Id == id);

            if (produto == null)
                throw new KeyNotFoundException($"Produto com id {id} não encontrado.");

            return produto;
        }

        public void Add(Produto produto)
        {
            _context.Produtos.Add(produto);
            _context.SaveChanges();
        }

        public void Update(Produto produto)
        {
            _context.Produtos.Attach(produto);
            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var produto = _context.Produtos.Find(id);
            if (produto != null)
            {
                _context.Produtos.Remove(produto);
                _context.SaveChanges();
            }
        }

        public bool ExisteProduto(int id)
        {
            return _context.Produtos.Any(p => p.Id == id);
        }
    }
}