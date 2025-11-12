
using Microsoft.AspNetCore.Mvc;
using api_ecommerce.Domain.Entities;
using api_ecommerce.Domain.Interfaces.Repositories;
using api_ecommerce.Domain.DTOs;
using System;
using api_ecommerce.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace api_ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly ProdutoService _produtoService;

        public ProdutoController(IProdutoRepository produtoRepository, ProdutoService produtoService)
        {
            _produtoRepository = produtoRepository;
            _produtoService = produtoService;
        }

        // GET (Todos):
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var produtos = _produtoRepository.GetAll().ToList();
                return Ok(produtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar produtos: {ex.Message}");
            }
        }

        // GET (Por Id):
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var produto = _produtoRepository.GetById(id);

                if (produto == null)
                    return NotFound($"Produto com id {id} não encontrado.");

                return Ok(produto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar produto: {ex.Message}");
            }
        }

        [HttpGet("fornecedor/{fornecedorId}")]
        public IActionResult GetByFornecedorId(int fornecedorId)
        {
            try
            {
                var produtos = _produtoRepository.GetByFornecedorId(fornecedorId);

                if (produtos == null || !produtos.Any())
                    return NotFound($"Nenhum produto encontrado para o fornecedor {fornecedorId}.");

                // opcional: mapear para DTO simples se quiser evitar retornar tudo
                var result = produtos.Select(p => new
                {
                    p.Id,
                    p.Nome,
                    p.Marca,
                    p.Preco,
                    p.FornecedorId,
                    p.Data,
                    p.ImagemPrincipalBase64,
                    Variacoes = p.Variacoes.Select(v => new
                    {
                        v.Id,
                        v.CorNome,
                        v.CorCodigo,
                        v.Preco,
                        Estoque = v.Estoque != null ? v.Estoque.QuantidadeDisponivel : 0
                    }),
                    EstoqueTotal = p.Variacoes.Sum(v => v.Estoque != null ? v.Estoque.QuantidadeDisponivel : 0)
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar produtos do fornecedor: {ex.Message}");
            }
        }

        [HttpGet("marca/{marca}")]
        public IActionResult GetByMarca(string marca)
        {
            try
            {
                var produtos = _produtoService.GetByMarca(marca);

                if (produtos == null || !produtos.Any())
                    return NotFound($"Nenhum produto encontrado para a marca: {marca}");

                return Ok(produtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar produtos da marca {marca}: {ex.Message}");
            }
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] string nome)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nome))
                    return BadRequest("O nome não pode ser vazio.");

                var produtos = _produtoService.SearchProducts(nome);

                if (produtos == null || !produtos.Any())
                    return NotFound($"Nenhum produto encontrado com o nome: {nome}");

                return Ok(produtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar produtos: {ex.Message}");
            }
        }


        [HttpPost("Create-Product")]
        public IActionResult CreateComVariacoes([FromBody] ProdutoComVariacoesDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var criado = _produtoService.CriarProdutoComVariacoesEEstoque(dto);

                var resposta = new
                {
                    criado.Id,
                    criado.Nome,
                    PrecoPrincipal = criado.Preco,
                    criado.Marca,
                    criado.Data,
                    criado.ImagemPrincipalBase64,
                    criado.CorNomePrincipal,
                    criado.CorCodigoPrincipal,
                    EstoqueTotal = criado.Variacoes.Sum(v => v.Estoque?.QuantidadeDisponivel ?? 0),
                    Variacoes = criado.Variacoes.Select(v => new
                    {
                        v.CorNome,
                        v.CorCodigo,
                        v.Preco,
                        v.ImagemBase64,
                        QuantidadeDisponivel = v.Estoque?.QuantidadeDisponivel ?? 0
                    })
                };

                return CreatedAtAction(nameof(GetById), new { id = criado.Id }, resposta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar produto com variações: {ex.Message}");
            }
        }

        // PUT:
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ProdutoDTO produtoDto)
        {
            if (produtoDto == null)
                return BadRequest("Produto não pode ser nulo.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (!_produtoRepository.ExisteProduto(id))
                    return NotFound($"Produto com id {id} não encontrado.");

                var produto = new Produto
                {
                    Id = id,
                    Nome = produtoDto.Nome,
                    Preco = produtoDto.Preco,
                    FornecedorId = produtoDto.FornecedorId,

                    
                };

                _produtoRepository.Update(produto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar produto: {ex.Message}");
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> AtualizarProduto(int id, [FromBody] ProdutoComVariacoesUpdateDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Dados do produto são obrigatórios.");

                var produtoAtualizado = await _produtoService.UpdateAsync(id, dto);

                if (produtoAtualizado == null)
                    return NotFound($"Produto com ID {id} não encontrado.");

                var estoqueTotal = produtoAtualizado.Variacoes
                    .Sum(v => v.Estoque?.QuantidadeDisponivel ?? 0);

                var primeiraVariacao = produtoAtualizado.Variacoes.FirstOrDefault();
                var precoPrincipal = primeiraVariacao?.Preco ?? produtoAtualizado.Preco;
                var corPrincipal = primeiraVariacao?.CorNome ?? produtoAtualizado.CorNomePrincipal;
                var corCodigoPrincipal = primeiraVariacao?.CorCodigo ?? produtoAtualizado.CorCodigoPrincipal;
                var imagemPrincipal = primeiraVariacao?.ImagemBase64 ?? produtoAtualizado.ImagemPrincipalBase64;

                var produtoResponse = new
                {
                    id = produtoAtualizado.Id,
                    nome = produtoAtualizado.Nome,
                    descricao = produtoAtualizado.Descricao,
                    marca = produtoAtualizado.Marca,
                    data = produtoAtualizado.Data,
                    precoPrincipal = precoPrincipal,
                    imagemPrincipalBase64 = imagemPrincipal,
                    corNomePrincipal = corPrincipal,
                    corCodigoPrincipal = corCodigoPrincipal,
                    estoqueTotal = estoqueTotal,
                    variacoes = produtoAtualizado.Variacoes.Select(v => new
                    {
                        id = v.Id,
                        corNome = v.CorNome,
                        corCodigo = v.CorCodigo,
                        preco = v.Preco,
                        imagemBase64 = v.ImagemBase64,
                        quantidadeDisponivel = v.Estoque?.QuantidadeDisponivel ?? 0
                    })
                };

                return Ok(new
                {
                    message = "Produto atualizado com sucesso!",
                    produto = produtoResponse
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Ocorreu um erro ao atualizar o produto.",
                    error = ex.Message
                });
            }
        }

        // DELETE:
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (!_produtoRepository.ExisteProduto(id))
                    return NotFound($"Produto com id {id} não encontrado.");

                _produtoService.ExcluirProdutoCompleto(id);

                return Ok(new { message = $"Produto {id} e suas variações foram removidos com sucesso." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao deletar produto: {ex.Message}");
            }
        }
    }
}
