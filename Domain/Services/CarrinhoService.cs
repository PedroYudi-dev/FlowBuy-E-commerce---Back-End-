
using System;
using System.Linq;
using api_ecommerce.Domain.DTOs;
using api_ecommerce.Domain.Entities;
using api_ecommerce.Domain.Interfaces.Repositories;
using api_ecommerce.Domain.Interfaces.Services;

namespace api_ecommerce.Domain.Services
{
    public class CarrinhoService : ICarrinhoService
    {
        private readonly ICarrinhoRepository _carrinhoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IEstoqueRepository _estoqueRepository;
        private readonly IVendaRepository _vendaRepository;

        public CarrinhoService(
            ICarrinhoRepository carrinhoRepository,
            IClienteRepository clienteRepository,
            IProdutoRepository produtoRepository,
            IEstoqueRepository estoqueRepository,
            IVendaRepository vendaRepository)
        {
            _carrinhoRepository = carrinhoRepository;
            _clienteRepository = clienteRepository;
            _produtoRepository = produtoRepository;
            _estoqueRepository = estoqueRepository;
            _vendaRepository = vendaRepository;
        }

        public Carrinho AdicionarItem(int clienteId, int produtoId, int quantidade)
        {
            if (!_clienteRepository.ExisteCliente(clienteId))
                throw new ArgumentException("Cliente não encontrado");

            var produto = _produtoRepository.GetById(produtoId);
            if (produto == null)
                throw new ArgumentException("Produto não encontrado");

            if (quantidade <= 0) throw new ArgumentException("Quantidade inválida");

            var estoque = _estoqueRepository.GetByProdutoId(produtoId);
            if (estoque == null || estoque.QuantidadeDisponivel < quantidade)
                throw new ArgumentException("Estoque insuficiente para o produto selecionado.");

            var carrinho = _carrinhoRepository.ObterOuCriarCarrinhoAberto(clienteId);

            // Se já existe item do mesmo produto, soma quantidade/subtotal
            var item = carrinho.Itens.FirstOrDefault(i => i.ProdutoId == produtoId);
            if (item == null)
            {
                item = new CarrinhoItem
                {
                    CarrinhoId = carrinho.Id,
                    ProdutoId = produtoId,
                    Quantidade = quantidade,
                    PrecoUnitario = produto.Preco,
                    Subtotal = produto.Preco * quantidade
                };
                _carrinhoRepository.AddItem(item);
                carrinho.Itens.Add(item);
            }
            else
            {
                item.Quantidade += quantidade;
                item.Subtotal = item.PrecoUnitario * item.Quantidade;
                _carrinhoRepository.SaveChanges();
            }

            return carrinho;
        }

        public Carrinho ObterCarrinhoAberto(int clienteId)
        {
            if (!_clienteRepository.ExisteCliente(clienteId))
                throw new ArgumentException("Cliente não encontrado");
            return _carrinhoRepository.ObterOuCriarCarrinhoAberto(clienteId);
        }

        public CarrinhoResumoDTO FinalizarCompra(int clienteId)
        {
            var carrinho = _carrinhoRepository.ObterOuCriarCarrinhoAberto(clienteId);
            if (carrinho.Itens.Count == 0)
                throw new ArgumentException("Carrinho vazio.");

            decimal total = 0m;

            foreach (var item in carrinho.Itens)
            {
                // cria vendas individuais por item
                var venda = new Venda
                {
                    ClienteId = clienteId,
                    ProdutoId = item.ProdutoId,
                    Quantidade = item.Quantidade,
                    Data = DateTime.Now
                };
                _vendaRepository.Add(venda);

                // baixa estoque
                var estoque = _estoqueRepository.GetByProdutoId(item.ProdutoId);
                if (estoque != null)
                {
                    estoque.QuantidadeDisponivel -= item.Quantidade;
                    _estoqueRepository.Update(estoque);
                }

                total += item.Subtotal;
            }

            // marca carrinho como não exibível
            carrinho.Exibir = "NAO";
            _carrinhoRepository.Update(carrinho);

            return new CarrinhoResumoDTO
            {
                CarrinhoId = carrinho.Id,
                ClienteId = clienteId,
                Exibir = carrinho.Exibir,
                Total = total
            };
        }
    }
}
