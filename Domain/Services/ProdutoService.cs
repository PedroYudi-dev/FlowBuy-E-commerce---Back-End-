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

                // 🔹 Adiciona variações com estoque próprio
                foreach (var variacaoDto in dto.Variacoes)
                {
                    var variacao = new ProdutoVariacao
                    {
                        CorNome = variacaoDto.CorNome,
                        CorCodigo = variacaoDto.CorCodigo,
                        ImagemBase64 = variacaoDto.ImagemBase64,
                        Preco = variacaoDto.Preco,
                    };

                    // cria o estoque e atribui a variacao
                    variacao.Estoque = new Estoque
                    {
                        QuantidadeDisponivel = variacaoDto.QuantidadeEstoque,
                        Data = DateTime.UtcNow,
                        UltimaAtualizacao = DateTime.UtcNow
                    };

                    // opcional: apontar de volta (não estritamente necessário, mas evita ambiguidade)
                    variacao.Estoque.ProdutoVariacao = variacao;

                    produto.Variacoes.Add(variacao);
                }

                // 🔹 Salva o produto com as variações e estoques
                _context.Produtos.Add(produto);
                _context.SaveChanges();

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

    }
}