using api_ecommerce.Domain.Entities;
using api_ecommerce.Domain.Interfaces.Repositories;
using api_ecommerce.Infrastructure.Data.Context;

public class ReviewRepository : IReviewRepository
{
    private readonly EcommerceDbContext _context;

    public ReviewRepository(EcommerceDbContext context)
    {
        _context = context;
    }

    public Review Add(Review review)
    {
        _context.Reviews.Add(review);
        return review;
    }

    public IEnumerable<Review> GetByProdutoId(int produtoId)
    {
        return _context.Reviews
            .Where(r => r.ProdutoId == produtoId)
            .OrderByDescending(r => r.DataCriacao)
            .ToList();
    }
}