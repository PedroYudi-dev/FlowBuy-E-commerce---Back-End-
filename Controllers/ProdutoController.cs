
using Microsoft.AspNetCore.Mvc;
using api_ecommerce.Domain.Entities;
using api_ecommerce.Domain.Interfaces.Repositories;
using api_ecommerce.Domain.DTOs;
using System;

namespace api_ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoController(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        // GET (Todos):
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var produtos = _produtoRepository.GetAll();
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
        [HttpPost]
        public IActionResult Create([FromBody] ProdutoDTO produtoDto)
        {
            if (produtoDto == null)
                return BadRequest("Produto não pode ser nulo.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var produto = new Produto
                {
                    Nome = produtoDto.Nome,
                    Preco = produtoDto.Preco,
                    FornecedorId = produtoDto.FornecedorId,
                    Marca = produtoDto.Marca,
                    Data = DateTime.UtcNow, // Timestamp no momento da criação

                    // Imagens:
                    Imagem1_base64 = produtoDto?.Imagem1_base64,
                    Imagem1_cor_nome = produtoDto?.Imagem1_cor_nome,
                    Imagem1_cor_codigo = produtoDto?.Imagem1_cor_codigo,

                    Imagem2_base64 = produtoDto?.Imagem2_base64,
                    Imagem2_cor_nome = produtoDto?.Imagem2_cor_nome,
                    Imagem2_cor_codigo = produtoDto?.Imagem2_cor_codigo,

                    Imagem3_base64 = produtoDto?.Imagem3_base64,
                    Imagem3_cor_nome = produtoDto?.Imagem3_cor_nome,
                    Imagem3_cor_codigo = produtoDto?.Imagem3_cor_codigo,
                };

                _produtoRepository.Add(produto);
                return CreatedAtAction(nameof(GetById), new { id = produto.Id }, produto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar produto: {ex.Message}");
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

                    // Imagens:
                    Imagem1_base64 = produtoDto?.Imagem1_base64,
                    Imagem1_cor_nome = produtoDto?.Imagem1_cor_nome,
                    Imagem1_cor_codigo = produtoDto?.Imagem1_cor_codigo,

                    Imagem2_base64 = produtoDto?.Imagem2_base64,
                    Imagem2_cor_nome = produtoDto?.Imagem2_cor_nome,
                    Imagem2_cor_codigo = produtoDto?.Imagem2_cor_codigo,

                    Imagem3_base64 = produtoDto?.Imagem3_base64,
                    Imagem3_cor_nome = produtoDto?.Imagem3_cor_nome,
                    Imagem3_cor_codigo = produtoDto?.Imagem3_cor_codigo,
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
