
using System.Collections.Generic;
using System.Linq;
using api_ecommerce.Domain.Entities;
using api_ecommerce.Domain.Interfaces.Repositories;
using api_ecommerce.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace api_ecommerce.Infrastructure.Repositories
{
    public class EstoqueRepository : IEstoqueRepository
    {
        private readonly EcommerceDbContext _context;

        public EstoqueRepository(EcommerceDbContext context)
        {
            _context = context;
        }

        public bool SuficienteEstoque(int produtoId, int quantidade)
        {
            var estoque = _context.Estoques
                .AsNoTracking()
                .FirstOrDefault(e => e.ProdutoId == produtoId);

            return estoque != null && estoque.QuantidadeDisponivel >= quantidade;
        }

        public void ReduzirEstoque(int produtoId, int quantidade)
        {
            var estoque = _context.Estoques.FirstOrDefault(e => e.ProdutoId == produtoId);
            if (estoque != null && estoque.QuantidadeDisponivel >= quantidade)
            {
                estoque.QuantidadeDisponivel -= quantidade;
                estoque.UltimaAtualizacao = System.DateTime.Now;
                _context.SaveChanges();
            }
            else
            {
                // Opcional: lançar exceção ou tratar estoque insuficiente
                // throw new InvalidOperationException("Estoque insuficiente para o produto.");
            }
        }

        public IEnumerable<Estoque> GetAll()
        {
            return _context.Estoques.AsNoTracking().ToList();
        }

        public Estoque? GetByProdutoId(int produtoId)
        {
            return _context.Estoques.FirstOrDefault(e => e.ProdutoId == produtoId);
        }
        public Estoque Add(Estoque estoque)
        {
            _context.Estoques.Add(estoque);
            _context.SaveChanges();
            return estoque;
        }

        public void Update(Estoque estoque)
        {
            _context.Estoques.Update(estoque);
            _context.SaveChanges();
        }

        public void Delete(int produtoId)
        {
            var estoque = _context.Estoques.FirstOrDefault(e => e.ProdutoId == produtoId);
            if (estoque != null)
            {
                _context.Estoques.Remove(estoque);
                _context.SaveChanges();
            }
        }

        public Estoque? GetByVariacaoId(int variacaoId)
        {
            return _context.Estoques.FirstOrDefault(e => e.ProdutoVariacaoId == variacaoId);
        }
    }
}
