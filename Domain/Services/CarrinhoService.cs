
//using System;
//using System.Linq;
//using api_ecommerce.Domain.DTOs;
//using api_ecommerce.Domain.Entities;
//using api_ecommerce.Domain.Interfaces.Repositories;
//using api_ecommerce.Domain.Interfaces.Services;

//namespace api_ecommerce.Domain.Services
//{
//    public class CarrinhoService : ICarrinhoService
//    {
//        private readonly ICarrinhoRepository _carrinhoRepository;
//        private readonly IClienteRepository _clienteRepository;
//        private readonly IProdutoRepository _produtoRepository;
//        private readonly IEstoqueRepository _estoqueRepository;
//        private readonly IVendaRepository _vendaRepository;
//        private readonly IProdutoVariacaoRepository _produtoVariacaoRepository;

//        public CarrinhoService(
//            ICarrinhoRepository carrinhoRepository,
//            IClienteRepository clienteRepository,
//            IProdutoRepository produtoRepository,
//            IEstoqueRepository estoqueRepository,
//            IVendaRepository vendaRepository,
//            IProdutoVariacaoRepository produtoVariacaoRepository  
//            )
//        {
//            _carrinhoRepository = carrinhoRepository;
//            _clienteRepository = clienteRepository;
//            _produtoRepository = produtoRepository;
//            _estoqueRepository = estoqueRepository;
//            _vendaRepository = vendaRepository;
//            _produtoVariacaoRepository = produtoVariacaoRepository;
//        }

//        public Carrinho AdicionarItem(int clienteId, int variacaoId)
//        {
//            if (!_clienteRepository.ExisteCliente(clienteId))
//                throw new ArgumentException("Cliente não encontrado");

//            // buscar variação
//            var variacao = _produtoVariacaoRepository.GetById(variacaoId);
//            if (variacao == null)
//                throw new ArgumentException("Variação não encontrada");

//            // buscar estoque da variação
//            var estoque = _estoqueRepository.GetByVariacaoId(variacaoId);
//            if (estoque == null || estoque.QuantidadeDisponivel < 1)
//                throw new ArgumentException("Estoque insuficiente para esta variação.");

//            // pegar ou criar carrinho do cliente
//            var carrinho = _carrinhoRepository.ObterOuCriarCarrinhoAberto(clienteId);

//            // verificar se já existe item dessa variação
//            var item = carrinho.Itens.FirstOrDefault(i => i.VariacaoId == variacaoId);

//            if (item == null)
//            {
//                // cria o item com quantidade = 1
//                item = new CarrinhoItem
//                {
//                    CarrinhoId = carrinho.Id,
//                    VariacaoId = variacaoId,
//                    Quantidade = 1,
//                    PrecoUnitario = variacao.Preco,
//                    Subtotal = variacao.Preco * 1
//                };

//                _carrinhoRepository.AddItem(item);
//                carrinho.Itens.Add(item);
//            }
//            else
//            {
//                // se quiser que NÃO some quantidade, NÃO mexemos aqui
//                // apenas retorna o carrinho — item já existe
//            }

//            return carrinho;
//        }

//        public Carrinho ObterCarrinhoAberto(int clienteId)
//        {
//            if (!_clienteRepository.ExisteCliente(clienteId))
//                throw new ArgumentException("Cliente não encontrado");
//            return _carrinhoRepository.ObterOuCriarCarrinhoAberto(clienteId);
//        }

//        public CarrinhoResumoDTO FinalizarCompra(int clienteId)
//        {
//            var carrinho = _carrinhoRepository.ObterOuCriarCarrinhoAberto(clienteId);
//            if (carrinho.Itens.Count == 0)
//                throw new ArgumentException("Carrinho vazio.");

//            decimal total = 0m;

//            foreach (var item in carrinho.Itens)
//            {
//                // cria vendas individuais por item
//                var venda = new Venda
//                {
//                    ClienteId = clienteId,
//                    ProdutoId = item.VariacaoId,
//                    Quantidade = item.Quantidade,
//                    Data = DateTime.Now
//                };
//                _vendaRepository.Add(venda);

//                // baixa estoque
//                var estoque = _estoqueRepository.GetByProdutoId(item.VariacaoId);
//                if (estoque != null)
//                {
//                    estoque.QuantidadeDisponivel -= item.Quantidade;
//                    _estoqueRepository.Update(estoque);
//                }

//                total += item.Subtotal;
//            }

//            // marca carrinho como não exibível
//            carrinho.Exibir = "NAO";
//            _carrinhoRepository.Update(carrinho);

//            return new CarrinhoResumoDTO
//            {
//                CarrinhoId = carrinho.Id,
//                ClienteId = clienteId,
//                Exibir = carrinho.Exibir,
//                Total = total
//            };
//        }

//        public void RemoverItem(int clienteId, int itemId)
//        {
//            if (!_clienteRepository.ExisteCliente(clienteId))
//                throw new ArgumentException("Cliente não encontrado");

//            carrinho.Itens.Remove(item);
//            _carrinhoRepository.RemoveItem(item);
//        }

//        public Carrinho AtualizarQuantidade(int clienteId, int itemId, int novaQuantidade)
//        {
//            if (novaQuantidade <= 0) throw new ArgumentException("Quantidade inválida.");

//            var carrinho = _carrinhoRepository.ObterOuCriarCarrinhoAberto(clienteId);
//            var item = carrinho.Itens.FirstOrDefault(i => i.Id == itemId);

//            if (item == null)
//                throw new ArgumentException("Item não encontrado.");

//            item.Quantidade = novaQuantidade;
//            item.Subtotal = item.PrecoUnitario * novaQuantidade;
//            _carrinhoRepository.SaveChanges();

//            return carrinho;
//        }

//        public IEnumerable<Carrinho> ListarCarrinhos(int clienteId, string? exibir = null)
//        {
//            if (!_clienteRepository.ExisteCliente(clienteId))
//                throw new ArgumentException("Cliente não encontrado");

//            // reuso do repositório que já possui ListarPorCliente
//            return _carrinhoRepository.ListarPorCliente(clienteId, exibir);
//        }
//    }
//}
