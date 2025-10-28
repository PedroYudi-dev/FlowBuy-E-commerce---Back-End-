
using Microsoft.AspNetCore.Mvc;
using api_ecommerce.Domain.Entities;
using api_ecommerce.Domain.Interfaces.Repositories;
using api_ecommerce.Domain.DTOs;
using System;

namespace api_ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstoqueController : ControllerBase
    {
        private readonly IEstoqueRepository _estoqueRepository;

        public EstoqueController(IEstoqueRepository estoqueRepository)
        {
            _estoqueRepository = estoqueRepository;
        }

        // GET (Todos):
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var estoques = _estoqueRepository.GetAll();
                return Ok(estoques);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar estoques: {ex.Message}");
            }
        }

        // GET (Por ProdutoId):
        [HttpGet("{produtoId}")]
        public IActionResult GetByProdutoId(int produtoId)
        {
            try
            {
                var estoque = _estoqueRepository.GetByProdutoId(produtoId);
                if (estoque == null)
                    return NotFound($"Estoque para o produto com id {produtoId} não encontrado.");

                return Ok(estoque);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar estoque: {ex.Message}");
            }
        }

        // POST:
        [HttpPost]
        public IActionResult Create([FromBody] EstoqueDTO estoqueDto)
        {
            if (estoqueDto == null)
                return BadRequest("Estoque não pode ser nulo.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var estoque = new Estoque
                {
                    ProdutoId = estoqueDto.ProdutoId,
                    QuantidadeDisponivel = estoqueDto.QuantidadeDisponivel,
                    Data = DateTime.UtcNow,
                    UltimaAtualizacao = estoqueDto.UltimaAtualizacao == default
                        ? DateTime.UtcNow
                        : estoqueDto.UltimaAtualizacao
                };

                _estoqueRepository.Add(estoque);
                return CreatedAtAction(nameof(GetByProdutoId), new { produtoId = estoque.ProdutoId }, estoque);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar estoque: {ex.Message}");
            }
        }

        // PUT:
        [HttpPut("{produtoId}")]
        public IActionResult Update(int produtoId, [FromBody] EstoqueDTO estoqueDto)
        {
            if (estoqueDto == null)
                return BadRequest("Estoque não pode ser nulo.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var estoqueExistente = _estoqueRepository.GetByProdutoId(produtoId);
                if (estoqueExistente == null)
                    return NotFound($"Estoque para o produto com id {produtoId} não encontrado.");

                estoqueExistente.QuantidadeDisponivel = estoqueDto.QuantidadeDisponivel;
                estoqueExistente.UltimaAtualizacao = estoqueDto.UltimaAtualizacao == default
                    ? DateTime.UtcNow
                    : estoqueDto.UltimaAtualizacao;

                _estoqueRepository.Update(estoqueExistente);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar estoque: {ex.Message}");
            }
        }

        // DELETE:
        [HttpDelete("{produtoId}")]
        public IActionResult Delete(int produtoId)
        {
            try
            {
                var estoqueExistente = _estoqueRepository.GetByProdutoId(produtoId);
                if (estoqueExistente == null)
                    return NotFound($"Estoque para o produto com id {produtoId} não encontrado.");

                _estoqueRepository.Delete(produtoId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao deletar estoque: {ex.Message}");
            }
        }
    }
}
