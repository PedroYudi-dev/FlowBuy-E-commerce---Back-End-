using api_ecommerce.Domain.Entities;

namespace api_ecommerce.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        void Add(User user);
        User GetByEmail(string email);
        bool ExisteEmail(string email);
    }
}
