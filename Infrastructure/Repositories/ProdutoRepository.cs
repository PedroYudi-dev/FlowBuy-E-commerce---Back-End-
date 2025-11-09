
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

        public IQueryable<Produto> GetAll()
        {
            return _context.Produtos
            .Include(p => p.Variacoes)
            .Include(p => p.Estoque);
        }

        public Produto GetById(int id)
        {
            return _context.Produtos
                        .Include(p => p.Variacoes)
                        .Include(p => p.Estoque)
                        .FirstOrDefault(p => p.Id == id);
        }

        public Produto Add(Produto produto)
        {
            _context.Produtos.Add(produto);
            return produto;
        }

        public void Update(Produto produto)
        {
            _context.Produtos.Update(produto);
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