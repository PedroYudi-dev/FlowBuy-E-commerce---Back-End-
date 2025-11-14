using api_ecommerce.Domain.DTOs;
using api_ecommerce.Domain.Entities;
using api_ecommerce.Domain.Interfaces.Repositories;
using api_ecommerce.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;

namespace api_ecommerce.Domain.Services
{
    public class ProdutoService
    {
        private readonly EcommerceDbContext _context;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IEstoqueRepository _estoqueRepository;

        public ProdutoService(EcommerceDbContext context, IProdutoRepository produtoRepository, IEstoqueRepository estoqueRepository)
        {
            _context = context;
            _produtoRepository = produtoRepository;
            _estoqueRepository = estoqueRepository;
        }

        public Produto CriarProdutoComEstoque(Produto produto, int quantidadeInicial)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                // 1️⃣ Cria o produto e salva para gerar o Id
                _produtoRepository.Add(produto);
                _context.SaveChanges();

                // 2️⃣ Cria o estoque inicial já com o Id do produto
                var estoque = new Estoque
                {
                    ProdutoId = produto.Id,
                    QuantidadeDisponivel = quantidadeInicial,
                    Data = DateTime.UtcNow,
                    UltimaAtualizacao = DateTime.UtcNow
                };

                _estoqueRepository.Add(estoque);
                _context.SaveChanges();

                produto.Estoque = estoque;

                // 3️⃣ Confirma a transação
                transaction.Commit();
                return produto;

            }
            catch
            {
                transaction.Rollback();
                throw new Exception($"Erro ao criar produto com estoque");
            }
        }

        public Produto CriarProdutoComVariacoesEEstoque(ProdutoComVariacoesDTO dto)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                // 🔹 Cria o produto base
                var produto = new Produto
                {
                    Nome = dto.Nome,
                    Preco = dto.Variacoes[0].Preco,
                    Marca = dto.Marca,
                    Descricao = dto.Descricao,
                    FornecedorId = dto.FornecedorId,
                    Data = DateTime.UtcNow,
                    ImagemPrincipalBase64 = dto.ImagemPrincipalBase64,
                    CorNomePrincipal = dto.CorNomePrincipal,
                    CorCodigoPrincipal = dto.CorCodigoPrincipal,
                };

                _context.Produtos.Add(produto);
                _context.SaveChanges();

                // 🔹 Adiciona variações com estoque próprio
                foreach (var variacaoDto in dto.Variacoes)
                {
                    var variacao = new ProdutoVariacao
                    {
                        ProdutoId = produto.Id,
                        CorNome = variacaoDto.CorNome,
                        CorCodigo = variacaoDto.CorCodigo,
                        ImagemBase64 = variacaoDto.ImagemBase64,
                        Preco = variacaoDto.Preco,
                    };

                    _context.ProdutoVariacoes.Add(variacao);
                    _context.SaveChanges();

                    // cria o estoque e atribui a variacao
                    variacao.Estoque = new Estoque
                    {
                        ProdutoId = produto.Id,
                        QuantidadeDisponivel = variacaoDto.QuantidadeEstoque,
                        Data = DateTime.UtcNow,
                        UltimaAtualizacao = DateTime.UtcNow,
                        ProdutoVariacaoId = variacao.Id
                    };

                    _context.Estoques.Add(variacao.Estoque);
                }

                // 🔹 Salva o produto com as variações e estoques
                _context.SaveChanges(); // salva tudo
                transaction.Commit();
                return produto;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Erro ao criar produto com variações e estoque: {ex.Message}", ex);
            }
        }

        public void ExcluirProdutoCompleto(int produtoId)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                _produtoRepository.DeleteProdutoCompleto(produtoId);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw new Exception($"Erro ao excluir produto completo (ID: {produtoId}).");
            }
        }

        public async Task<Produto?> UpdateAsync(int id, ProdutoComVariacoesUpdateDTO dto)
        {
            var produto = await _context.Produtos
                .Include(p => p.Variacoes)
                    .ThenInclude(v => v.Estoque)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (produto == null)
                return null;

            // 🔹 Atualiza campos principais
            if (!string.IsNullOrEmpty(dto.Nome))
                produto.Nome = dto.Nome;

            if (!string.IsNullOrEmpty(dto.Descricao))
                produto.Descricao = dto.Descricao;

            if (dto.Preco.HasValue)
                produto.Preco = dto.Preco.Value;

            if (dto.FornecedorId.HasValue)
                produto.FornecedorId = dto.FornecedorId.Value;

            if (!string.IsNullOrEmpty(dto.Marca))
                produto.Marca = dto.Marca;

            if (!string.IsNullOrEmpty(dto.ImagemPrincipalBase64))
                produto.ImagemPrincipalBase64 = dto.ImagemPrincipalBase64;

            if (!string.IsNullOrEmpty(dto.CorNomePrincipal))
                produto.CorNomePrincipal = dto.CorNomePrincipal;

            if (!string.IsNullOrEmpty(dto.CorCodigoPrincipal))
                produto.CorCodigoPrincipal = dto.CorCodigoPrincipal;

            // 🔹 Atualiza variações
            if (dto.Variacoes != null && dto.Variacoes.Any())
            {
                foreach (var variacaoDto in dto.Variacoes)
                {
                    var variacaoExistente = produto.Variacoes
                        .FirstOrDefault(v => v.Id == variacaoDto.Id);

                    if (variacaoExistente != null)
                    {
                        // 🔸 Atualiza somente os campos enviados
                        if (!string.IsNullOrEmpty(variacaoDto.CorNome))
                            variacaoExistente.CorNome = variacaoDto.CorNome;

                        if (!string.IsNullOrEmpty(variacaoDto.CorCodigo))
                            variacaoExistente.CorCodigo = variacaoDto.CorCodigo;

                        if (!string.IsNullOrEmpty(variacaoDto.ImagemBase64))
                            variacaoExistente.ImagemBase64 = variacaoDto.ImagemBase64;

                        if (variacaoDto.Preco.HasValue)
                            variacaoExistente.Preco = variacaoDto.Preco.Value;

                        // 🔸 Atualiza o estoque da variação
                        if (variacaoDto.QuantidadeEstoque.HasValue)
                        {
                            if (variacaoExistente.Estoque == null)
                                variacaoExistente.Estoque = new Estoque
                                {
                                    QuantidadeDisponivel = variacaoDto.QuantidadeEstoque.Value,
                                    UltimaAtualizacao = DateTime.UtcNow
                                };
                            else
                            {
                                variacaoExistente.Estoque.QuantidadeDisponivel = variacaoDto.QuantidadeEstoque.Value;
                                variacaoExistente.Estoque.UltimaAtualizacao = DateTime.UtcNow;
                            }
                        }
                    }
                    else
                    {
                        var novaVariacao = new ProdutoVariacao
                        {
                            CorNome = variacaoDto.CorNome,
                            CorCodigo = variacaoDto.CorCodigo,
                            ImagemBase64 = variacaoDto.ImagemBase64,
                            Preco = variacaoDto.Preco ?? produto.Preco,
                            ProdutoId = produto.Id, // 🔹 garante vínculo correto
                            Estoque = new Estoque
                            {
                                QuantidadeDisponivel = variacaoDto.QuantidadeEstoque ?? 0,
                                Data = DateTime.UtcNow,
                                UltimaAtualizacao = DateTime.UtcNow
                            }
                        };
                        produto.Variacoes.Add(novaVariacao);
                    }
                }
            }

            // 🔹 Calcula estoque total e preço principal
            var estoqueTotal = produto.Variacoes.Sum(v => v.Estoque?.QuantidadeDisponivel ?? 0);
            var precoPrincipal = produto.Variacoes.FirstOrDefault()?.Preco ?? produto.Preco;

            // 🔹 Atualiza estoque principal
            if (produto.Estoque != null)
                produto.Estoque.QuantidadeDisponivel = estoqueTotal;
            else
                produto.Estoque = new Estoque
                {
                    QuantidadeDisponivel = estoqueTotal,
                    UltimaAtualizacao = DateTime.UtcNow
                };

            // 🔹 Atualiza preço e data
            produto.Preco = precoPrincipal;
            produto.Data = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return produto;
        }

        public IEnumerable<Produto> GetByMarca(string marca)
        {
            return _produtoRepository.GetByMarca(marca);
        }

        public IEnumerable<Produto> SearchProducts(string nome)
        {
            return _produtoRepository.SearchByNome(nome);
        }

        public object MapProdutoToDto(Produto produto)
        {
            return new
            {
                produto.Id,
                produto.Nome,
                produto.Marca,
                produto.Preco,
                produto.FornecedorId,
                produto.Data,
                produto.ImagemPrincipalBase64,
                Variacoes = produto.Variacoes.Select(v => new
                {
                    v.Id,
                    v.CorNome,
                    v.CorCodigo,
                    v.Preco,
                    v.ImagemBase64,
                    Estoque = v.Estoque?.QuantidadeDisponivel ?? 0
                }),
                EstoqueTotal = produto.Variacoes.Sum(v => v.Estoque?.QuantidadeDisponivel ?? 0)
            };
        }


    }
}