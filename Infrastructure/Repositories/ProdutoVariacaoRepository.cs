using api_ecommerce.Domain.Entities;
using api_ecommerce.Domain.Interfaces.Repositories;
using api_ecommerce.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

public class ProdutoVariacaoRepository : IProdutoVariacaoRepository
{
    private readonly EcommerceDbContext _context;

    public ProdutoVariacaoRepository(EcommerceDbContext context)
    {
        _context = context;
    }

    public ProdutoVariacao? GetById(int id)
    {
        return _context.ProdutoVariacoes
            .Include(v => v.Estoque)
            .FirstOrDefault(v => v.Id == id);
    }

    public IEnumerable<ProdutoVariacao> GetByProdutoId(int produtoId)
    {
        return _context.ProdutoVariacoes
            .Where(v => v.ProdutoId == produtoId)
            .ToList();
    }

    public void Update(ProdutoVariacao variacao)
    {
        _context.ProdutoVariacoes.Update(variacao);
        _context.SaveChanges();
    }
}
