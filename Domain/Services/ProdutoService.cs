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
            var produto = new Produto
            {
                Nome = dto.Nome,
                Preco = dto.Preco,
                Marca = dto.Marca,
                Descricao = dto.Descricao,
                FornecedorId = dto.FornecedorId,
                Data = DateTime.UtcNow,
                ImagemPrincipalBase64 = dto.ImagemPrincipalBase64,
                CorNomePrincipal = dto.CorNomePrincipal,
                CorCodigoPrincipal = dto.CorCodigoPrincipal
            };

            // Adiciona variações
            if (dto.Variacoes != null && dto.Variacoes.Any())
            {
                foreach (var variacaoDto in dto.Variacoes)
                {
                    produto.Variacoes.Add(new ProdutoVariacao
                    {
                        CorNome = variacaoDto.CorNome,
                        CorCodigo = variacaoDto.CorCodigo,
                        ImagemBase64 = variacaoDto.ImagemBase64
                    });
                }
            }

            _context.Produtos.Add(produto);
            _context.SaveChanges();

            // Cria estoque
            var estoque = new Estoque
            {
                ProdutoId = produto.Id,
                QuantidadeDisponivel = dto.QuantidadeInicial > 0 ? dto.QuantidadeInicial : 0
            };

            _context.Estoques.Add(estoque);
            _context.SaveChanges();

            produto.Estoque = estoque;
            return produto;
        }

    }
}