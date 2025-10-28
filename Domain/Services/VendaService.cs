
using System;
using System.Collections.Generic;
using api_ecommerce.Domain.Entities;
using api_ecommerce.Domain.Interfaces.Repositories;
using api_ecommerce.Domain.Interfaces.Services;

namespace api_ecommerce.Domain.Services
{
    public class VendaService : IVendaService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IEstoqueRepository _estoqueRepository;
        private readonly IVendaRepository _vendaRepository;

        public VendaService(
            IClienteRepository clienteRepo,
            IProdutoRepository produtoRepo,
            IEstoqueRepository estoqueRepo,
            IVendaRepository vendaRepo)
        {
            _clienteRepository = clienteRepo;
            _produtoRepository = produtoRepo;
            _estoqueRepository = estoqueRepo;
            _vendaRepository = vendaRepo;
        }

        public Venda CriarVenda(int clienteId, int produtoId, int quantidade)
        {
            if (!_clienteRepository.ExisteCliente(clienteId))
                throw new ArgumentException("Cliente inválido");

            if (!_produtoRepository.ExisteProduto(produtoId))
                throw new ArgumentException("Produto inválido");

            if (!_estoqueRepository.SuficienteEstoque(produtoId, quantidade))
                throw new ArgumentException("Estoque insuficiente");

            var venda = new Venda
            {
                ClienteId = clienteId,
                ProdutoId = produtoId,
                Quantidade = quantidade,
                Data = DateTime.Now
            };

            _estoqueRepository.ReduzirEstoque(produtoId, quantidade);
            _vendaRepository.Add(venda);

            return venda;
        }

        public IEnumerable<Venda> ListarVendas()
        {
            return _vendaRepository.GetAll();
        }

        public Venda GetById(int id)
        {
            return _vendaRepository.GetById(id);
        }

        public void AtualizarVenda(Venda venda)
        {
            if (!_vendaRepository.ExisteVenda(venda.Id))
                throw new ArgumentException("Venda não encontrada");

            _vendaRepository.Update(venda);
        }

        public void DeletarVenda(int id)
        {
            if (!_vendaRepository.ExisteVenda(id))
                throw new ArgumentException("Venda não encontrada");

            _vendaRepository.Delete(id);
        }

        public bool ExisteVenda(int id)
        {
            return _vendaRepository.ExisteVenda(id);
        }
    }
}
