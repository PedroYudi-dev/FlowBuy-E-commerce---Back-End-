
using api_ecommerce.Domain.DTOs;
using api_ecommerce.Domain.Entities;

namespace api_ecommerce.Domain.Interfaces.Services
{
    public interface ICarrinhoService
    {
        Carrinho AdicionarItem(int clienteId, int produtoId, int quantidade);
        Carrinho ObterCarrinhoAberto(int clienteId);
        CarrinhoResumoDTO FinalizarCompra(int clienteId);
        IEnumerable<Carrinho> ListarCarrinhos(int clienteId, string? exibir = null);
        Carrinho AtualizarQuantidade(int clienteId, int itemId, int novaQuantidade);
        void RemoverItem(int clienteId, int itemId);

    }
}
