
using System.Collections.Generic;
using api_ecommerce.Domain.Entities;

namespace api_ecommerce.Domain.Interfaces.Repositories
{
    public interface ICarrinhoRepository
    {
        Carrinho ObterOuCriarCarrinhoAberto(int clienteId);
        Carrinho ObterPorId(int id);
        IEnumerable<Carrinho> ListarPorCliente(int clienteId, string? exibir = null);
        void AddItem(CarrinhoItem item);
        void Update(Carrinho carrinho);
        void SaveChanges();
        void RemoveItem(CarrinhoItem item);

    }
}
