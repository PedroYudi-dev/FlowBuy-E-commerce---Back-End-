
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using api_ecommerce.Domain.Entities;
using api_ecommerce.Domain.Interfaces.Repositories;
using api_ecommerce.Infrastructure.Data.Context;

namespace api_ecommerce.Infrastructure.Repositories
{
    public class CarrinhoRepository : ICarrinhoRepository
    {
        private readonly EcommerceDbContext _context;
        public CarrinhoRepository(EcommerceDbContext context) { _context = context; }

        public Carrinho ObterOuCriarCarrinhoAberto(int clienteId)
        {
            var carrinho = _context.Carrinhos
                .Include(c => c.Itens)
                .ThenInclude(i => i.Produto)
                .FirstOrDefault(c => c.ClienteId == clienteId && c.Exibir == "SIM");

            if (carrinho == null)
            {
                carrinho = new Carrinho { ClienteId = clienteId, Exibir = "SIM" };
                _context.Carrinhos.Add(carrinho);
                _context.SaveChanges();
                _context.Entry(carrinho).Collection(c => c.Itens).Load();
            }
            return carrinho;
        }

        public Carrinho ObterPorId(int id)
        {
            return _context.Carrinhos
                .Include(c => c.Itens)
                .ThenInclude(i => i.Produto)
                .First(c => c.Id == id);
        }

        public IEnumerable<Carrinho> ListarPorCliente(int clienteId, string? exibir = null)
        {
            var q = _context.Carrinhos
                .Include(c => c.Itens)
                .Where(c => c.ClienteId == clienteId);
            if (!string.IsNullOrEmpty(exibir))
                q = q.Where(c => c.Exibir == exibir);
            return q.AsNoTracking().ToList();
        }

        public void AddItem(CarrinhoItem item)
        {
            _context.CarrinhosItens.Add(item);
            _context.SaveChanges();
        }

        public void Update(Carrinho carrinho)
        {
            _context.Carrinhos.Update(carrinho);
            _context.SaveChanges();
        }

        public void SaveChanges() => _context.SaveChanges();
    }
}
