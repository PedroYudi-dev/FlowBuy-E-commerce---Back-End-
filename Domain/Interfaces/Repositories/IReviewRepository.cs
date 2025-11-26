using api_ecommerce.Domain.Entities;

namespace api_ecommerce.Domain.Interfaces.Repositories
{
    public interface IReviewRepository
    {
        Review Add(Review review);
        IEnumerable<Review> GetByProdutoId(int produtoId);
    }
}
