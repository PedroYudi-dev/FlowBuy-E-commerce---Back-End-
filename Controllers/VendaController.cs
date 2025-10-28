
using System;
using Microsoft.AspNetCore.Mvc;
using api_ecommerce.Domain.DTOs;
using api_ecommerce.Domain.Entities;
using api_ecommerce.Domain.Interfaces.Services;

namespace api_ecommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendaController : ControllerBase
    {
        private readonly IVendaService _vendaService;

        public VendaController(IVendaService vendaService)
        {
            _vendaService = vendaService;
        }

        // POST: api/venda
        [HttpPost]
        public IActionResult CriarVenda([FromBody] VendaDTO vendaDto)
        {
            if (vendaDto == null)
                return BadRequest("Venda não pode ser nula.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // O serviço cria a venda e já atribui DataCriacao
                var novaVenda = _vendaService.CriarVenda(vendaDto.ClienteId, vendaDto.ProdutoId, vendaDto.Quantidade);
                return CreatedAtAction(nameof(GetById), new { id = novaVenda.Id }, novaVenda);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar venda: {ex.Message}");
            }
        }

        // GET: api/venda
        [HttpGet]
        public IActionResult ListarVendas()
        {
            try
            {
                var vendas = _vendaService.ListarVendas();
                return Ok(vendas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao listar vendas: {ex.Message}");
            }
        }

        // GET: api/venda/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var venda = _vendaService.GetById(id);
                if (venda == null)
                    return NotFound($"Venda com id {id} não encontrada.");

                return Ok(venda);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar venda: {ex.Message}");
            }
        }

        // PUT: api/venda/{id}
        [HttpPut("{id}")]
        public IActionResult AtualizarVenda(int id, [FromBody] VendaDTO vendaDto)
        {
            if (vendaDto == null)
                return BadRequest("Venda não pode ser nula.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (!_vendaService.ExisteVenda(id))
                    return NotFound($"Venda com id {id} não encontrada.");

                var vendaAtualizada = new Venda
                {
                    Id = id,
                    ClienteId = vendaDto.ClienteId,
                    ProdutoId = vendaDto.ProdutoId,
                    Quantidade = vendaDto.Quantidade
                    // Data não é alterada
                };

                _vendaService.AtualizarVenda(vendaAtualizada);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar venda: {ex.Message}");
            }
        }

        // DELETE: api/venda/{id}
        [HttpDelete("{id}")]
        public IActionResult DeletarVenda(int id)
        {
            try
            {
                if (!_vendaService.ExisteVenda(id))
                    return NotFound($"Venda com id {id} não encontrada.");

                _vendaService.DeletarVenda(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao deletar venda: {ex.Message}");
            }
        }
    }
}
