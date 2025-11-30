using api_ecommerce.Domain.Models;
using api_ecommerce.Domain.DTOs;
using Microsoft.EntityFrameworkCore;
using api_ecommerce.Infrastructure.Data.Context;

namespace api_ecommerce.Services
{
    public class CarrinhoService
    {
        private readonly EcommerceDbContext _context;

        public CarrinhoService(EcommerceDbContext context)
        {
            _context = context;
        }

        public async Task<Carrinho> GetByUsuario(int usuarioId)
        {
            var carrinho = await _context.Carrinhos
                .Include(c => c.Itens)
                .ThenInclude(i => i.Variacao)
                .ThenInclude(v => v.Estoque)
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);

            if (carrinho != null)
            {
                // Calcula subtotal de cada item
                foreach (var item in carrinho.Itens)
                {
                    item.Subtotal = item.Preco * item.Quantidade;
                    item.QuantidadeDisponivel = item.Variacao?.Estoque?.QuantidadeDisponivel ?? 0;
                }

                // Calcula o total do carrinho
                carrinho.Total = carrinho.Itens.Sum(i => i.Subtotal);
            }

            return carrinho;
        }

        public async Task<Carrinho> AddItem(AddItemCarrinhoDTO dto)
        {
            var carrinho = await GetByUsuario(dto.UsuarioId);

            if (carrinho == null)
            {
                carrinho = new Carrinho
                {
                    UsuarioId = dto.UsuarioId,
                    Itens = new List<ItemCarrinho>()
                };
                _context.Carrinhos.Add(carrinho);
                await _context.SaveChangesAsync();
            }

            var produto = await _context.Produtos
                .Include(p => p.Variacoes)
                .FirstOrDefaultAsync(p => p.Variacoes.Any(v => v.Id == dto.VariacaoId));

            if (produto == null)
                throw new Exception("Produto da variação não encontrado.");

            var variacao = await _context.ProdutoVariacoes.Include(v => v.Estoque).FirstOrDefaultAsync(v => v.Id == dto.VariacaoId);
            if (variacao == null) throw new Exception("Variacao não encontrada.");

            var item = carrinho.Itens.FirstOrDefault(i => i.VariacaoId == dto.VariacaoId);

            ItemCarrinho itemCarrinho;
            if (item != null)
            {
                int diferenca = dto.Quantidade;

                // Verifica se há estoque suficiente
                if (variacao.Estoque.QuantidadeDisponivel < diferenca)
                    throw new Exception("Estoque insuficiente");

                item.Quantidade += dto.Quantidade;

                variacao.Estoque.QuantidadeDisponivel -= diferenca;

                itemCarrinho = item;
            }
            else
            {
                if (variacao.Estoque.QuantidadeDisponivel < dto.Quantidade)
                    throw new Exception("Estoque insuficiente");

                itemCarrinho = new ItemCarrinho
                {
                    ProdutoId = produto.Id,
                    VariacaoId = variacao.Id,
                    NomeProduto = produto.Nome,
                    Preco = variacao.Preco,
                    Quantidade = dto.Quantidade,
                    ImagemBase64 = variacao.ImagemBase64,
                    CorCodigo = variacao.CorCodigo,
                    CorNome = variacao.CorNome,
                    QuantidadeDisponivel = variacao.Estoque?.QuantidadeDisponivel ?? 0
                };

                variacao.Estoque.QuantidadeDisponivel -= dto.Quantidade;

                carrinho.Itens.Add(itemCarrinho);
            }

            // Atualiza subtotal do item
            itemCarrinho.QuantidadeDisponivel = variacao.Estoque?.QuantidadeDisponivel ?? 0;
            itemCarrinho.Subtotal = itemCarrinho.Preco * itemCarrinho.Quantidade;

            // Atualiza total do carrinho
            carrinho.Total = carrinho.Itens.Sum(i => i.Subtotal);

            await _context.SaveChangesAsync();

            return carrinho;
        }

        public async Task<Carrinho> AtualizarQuantidade(UpdateQuantidadeDTO dto)
        {
            var item = await _context.ItensCarrinho
                .Include(i => i.Carrinho)
                .Include(i => i.Variacao)
                    .ThenInclude(v => v.Estoque)
                .FirstOrDefaultAsync(i => i.Id == dto.ItemCarrinhoId);

            if (item == null)
                throw new Exception("Item não encontrado.");

            int diferenca = dto.Quantidade - item.Quantidade;

            if (diferenca > 0 && item.Variacao.Estoque.QuantidadeDisponivel < diferenca)
                throw new Exception("Estoque insuficiente");

            // Atualiza a quantidade
            item.Quantidade = dto.Quantidade;
            
            item.Variacao.Estoque.QuantidadeDisponivel -= diferenca;

            // Atualiza subtotal do item
            item.Subtotal = item.Preco * item.Quantidade;
            item.QuantidadeDisponivel = item.Variacao?.Estoque?.QuantidadeDisponivel ?? 0;

            await _context.SaveChangesAsync();

            // Recarrega o carrinho completo
            var carrinho = await _context.Carrinhos
                .Include(c => c.Itens)
                .ThenInclude(i => i.Variacao)
                        .ThenInclude(v => v.Estoque)
                .FirstOrDefaultAsync(c => c.Id == item.CarrinhoId);

            if (carrinho != null)
            {
                foreach (var i in carrinho.Itens)
                {
                    i.Subtotal = i.Preco * i.Quantidade;
                    i.QuantidadeDisponivel = i.Variacao?.Estoque?.QuantidadeDisponivel ?? 0;
                }
                carrinho.Total = carrinho.Itens.Sum(i => i.Subtotal);
            }

            return carrinho;
        }

        public async Task<bool> RemoverItem(int itemId)
        {
            var item = await _context.ItensCarrinho
                .Include(i => i.Variacao)
                    .ThenInclude(v => v.Estoque)
                .FirstOrDefaultAsync(i => i.Id == itemId);

            if (item == null) return false;

            // Restaurar estoque
            if (item.Variacao?.Estoque != null)
            {
                item.Variacao.Estoque.QuantidadeDisponivel += item.Quantidade;
            }

            // Remove o item do carrinho
            _context.ItensCarrinho.Remove(item);

            // Atualiza subtotal e total do carrinho
            var carrinho = await _context.Carrinhos
                .Include(c => c.Itens)
                    .ThenInclude(i => i.Variacao)
                        .ThenInclude(v => v.Estoque)
                .FirstOrDefaultAsync(c => c.Id == item.CarrinhoId);

            if (carrinho != null)
            {
                foreach (var i in carrinho.Itens.Where(i => i.Id != itemId))
                {
                    i.Subtotal = i.Preco * i.Quantidade;
                    i.QuantidadeDisponivel = i.Variacao?.Estoque?.QuantidadeDisponivel ?? 0;
                }
                carrinho.Total = carrinho.Itens.Where(i => i.Id != itemId).Sum(i => i.Subtotal);
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
