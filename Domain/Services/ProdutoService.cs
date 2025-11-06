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

    }
}
