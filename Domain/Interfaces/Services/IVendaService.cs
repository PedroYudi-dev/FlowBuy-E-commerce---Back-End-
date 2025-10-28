
using System.Collections.Generic;
using api_ecommerce.Domain.Entities;

namespace api_ecommerce.Domain.Interfaces.Services
{
    public interface IVendaService
    {
        Venda CriarVenda(int clienteId, int produtoId, int quantidade);
        IEnumerable<Venda> ListarVendas();
        Venda GetById(int id);
        void AtualizarVenda(Venda venda);
        void DeletarVenda(int id);
        bool ExisteVenda(int id);
    }
}
