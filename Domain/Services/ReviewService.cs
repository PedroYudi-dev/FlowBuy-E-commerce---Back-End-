using api_ecommerce.Domain.DTOs;
using api_ecommerce.Domain.Entities;
using api_ecommerce.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

public class ReviewService
{
    private readonly EcommerceDbContext _context;

    public ReviewService(EcommerceDbContext context)
    {
        _context = context;
    }

    public async Task AddReviewAsync(int produtoId, ReviewDTO dto)
    {
        var produto = await _context.Produtos.FindAsync(produtoId);
        if (produto == null)
            throw new Exception("Produto não encontrado.");

        var cliente = await _context.Clientes.FindAsync(dto.ClienteId);
        if (cliente == null)
            throw new Exception("Cliente não encontrado.");

        // Impede fornecedor avaliar o próprio produto
        if (produto.FornecedorId != null && cliente.UserId == produto.FornecedorId)
            throw new Exception("Fornecedor não pode avaliar o próprio produto.");

        if (dto.Nota < 1 || dto.Nota > 5)
            throw new Exception("A nota deve ser entre 1 e 5.");

        var review = new Review
        {
            ProdutoId = produtoId,
            ClienteId = dto.ClienteId,
            Nota = dto.Nota,
            Comentario = dto.Comentario
        };

        _context.Add(review);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteReviewAsync(int reviewId, int clienteId)
    {
        var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);

        if (review == null)
            throw new Exception("Avaliação não encontrada.");

        // segurança: cliente só pode remover o que é dele
        if (review.ClienteId != clienteId)
            throw new Exception("Você não tem permissão para excluir esta avaliação.");

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Review>> GetReviewsByProductAsync(int produtoId)
    {
        return await _context.Reviews
            .Where(r => r.ProdutoId == produtoId)
            .Include(r => r.Cliente) // opcional
            .OrderByDescending(r => r.Id)
            .ToListAsync();
    }
}