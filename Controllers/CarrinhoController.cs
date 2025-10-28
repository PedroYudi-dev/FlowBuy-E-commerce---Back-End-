
using System;
using Microsoft.AspNetCore.Mvc;
using api_ecommerce.Domain.DTOs;
using api_ecommerce.Domain.Interfaces.Services;

namespace api_ecommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarrinhoController : ControllerBase
    {
        private readonly ICarrinhoService _carrinhoService;
        public CarrinhoController(ICarrinhoService carrinhoService)
        {
            _carrinhoService = carrinhoService;
        }

        // GET: Carrinho aberto do cliente
        [HttpGet("{clienteId:int}")]
        public IActionResult GetCarrinho(int clienteId)
        {
            try
            {
                var carrinho = _carrinhoService.ObterCarrinhoAberto(clienteId);
                return Ok(carrinho);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // POST: Adicionar item ao carrinho
        [HttpPost("{clienteId:int}/itens")]
        public IActionResult AddItem(int clienteId, [FromBody] CarrinhoItemAddDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var carrinho = _carrinhoService.AdicionarItem(clienteId, dto.ProdutoId, dto.Quantidade);
                return Ok(carrinho);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // POST: Finalizar compra
        [HttpPost("{clienteId:int}/finalizar")]
        public IActionResult Finalizar(int clienteId)
        {
            try
            {
                var resumo = _carrinhoService.FinalizarCompra(clienteId);
                return Ok(resumo);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
