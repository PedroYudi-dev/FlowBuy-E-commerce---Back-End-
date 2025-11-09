
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

        // POST:
        //[HttpPost]
        //public IActionResult Create([FromBody] ProdutoDTO produtoDto)
        //{
        //    if (produtoDto == null)
        //        return BadRequest("Produto não pode ser nulo.");

        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    try
        //    {
        //        var produto = new Produto
        //        {
        //            Nome = produtoDto.Nome,
        //            Preco = produtoDto.Preco,
        //            FornecedorId = produtoDto.FornecedorId,
        //            Marca = produtoDto.Marca,
        //            Data = DateTime.UtcNow, // Timestamp no momento da criação

        //            // Imagens:
        //            Imagem1_base64 = produtoDto?.Imagem1_base64,
        //            Imagem1_cor_nome = produtoDto?.Imagem1_cor_nome,
        //            Imagem1_cor_codigo = produtoDto?.Imagem1_cor_codigo,

        //            Imagem2_base64 = produtoDto?.Imagem2_base64,
        //            Imagem2_cor_nome = produtoDto?.Imagem2_cor_nome,
        //            Imagem2_cor_codigo = produtoDto?.Imagem2_cor_codigo,

        //            Imagem3_base64 = produtoDto?.Imagem3_base64,
        //            Imagem3_cor_nome = produtoDto?.Imagem3_cor_nome,
        //            Imagem3_cor_codigo = produtoDto?.Imagem3_cor_codigo,
        //        };

        //        _produtoRepository.Add(produto);
        //        return CreatedAtAction(nameof(GetById), new { id = produto.Id }, produto);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Erro ao criar produto: {ex.Message}");
        //    }
        //}

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
                    criado.Preco,
                    criado.Marca,
                    criado.Data,
                    criado.ImagemPrincipalBase64,
                    criado.CorNomePrincipal,
                    criado.CorCodigoPrincipal,
                    QuantidadeDisponivel = criado.Estoque?.QuantidadeDisponivel ?? 0,
                    Variacoes = criado.Variacoes.Select(v => new
                    {
                        v.CorNome,
                        v.CorCodigo,
                        v.ImagemBase64
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

        // DELETE:
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (!_produtoRepository.ExisteProduto(id))
                    return NotFound($"Produto com id {id} não encontrado.");

                _produtoRepository.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao deletar produto: {ex.Message}");
            }
        }
    }
}
