
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

        public void DeleteProdutoCompleto(int id)
        {
            var produto = _context.Produtos
                .Include(p => p.Variacoes)
                    .ThenInclude(v => v.Estoque)
                .Include(p => p.Estoque)
                .FirstOrDefault(p => p.Id == id);

            if (produto == null)
                throw new Exception($"Produto com id {id} não encontrado.");

            // Remove estoques das variações
            var estoquesVariacoes = produto.Variacoes
                .Where(v => v.Estoque != null)
                .Select(v => v.Estoque)
                .ToList();

            if (estoquesVariacoes.Any())
                _context.Estoques.RemoveRange(estoquesVariacoes);

            // Remove estoques do produto principal (se houver)
            if (produto.Estoque != null)
                _context.Estoques.Remove(produto.Estoque);

            // Remove as variações
            if (produto.Variacoes.Any())
                _context.ProdutoVariacoes.RemoveRange(produto.Variacoes);

            // Por fim, remove o produto
            _context.Produtos.Remove(produto);

            _context.SaveChanges();
        }

        public bool ExisteProduto(int id)
        {
            return _context.Produtos.Any(p => p.Id == id);
        }

        public IEnumerable<Produto> GetByFornecedorId(int fornecedorId)
        {
            return _context.Produtos
                .Where(p => p.FornecedorId == fornecedorId)
                .Include(p => p.Variacoes)
                    .ThenInclude(v => v.Estoque)
                .Include(p => p.Estoque)
                .AsNoTracking()
                .ToList();
        }

        public async Task<Produto?> GetProdutoComVariacoesAsync(int produtoId)
        {
            return await _context.Produtos
                .Include(p => p.Variacoes)
                    .ThenInclude(v => v.Estoque)
                .FirstOrDefaultAsync(p => p.Id == produtoId);
        }

        public async Task UpdateAsync(Produto produto)
        {
            _context.Produtos.Update(produto);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Produto> GetByMarca(string marca)
        {
            return _context.Produtos
                .Include(p => p.Variacoes)
                    .ThenInclude(v => v.Estoque)
                .Include(p => p.Estoque)
                .Where(p => p.Marca.ToLower() == marca.ToLower())
                .ToList();
        }
    }
}