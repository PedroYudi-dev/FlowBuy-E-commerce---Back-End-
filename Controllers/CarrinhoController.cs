using Microsoft.AspNetCore.Mvc;
using api_ecommerce.Services;
using api_ecommerce.Domain.DTOs;

namespace api_ecommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarrinhoController : ControllerBase
    {
        private readonly CarrinhoService _service;

        public CarrinhoController(CarrinhoService service)
        {
            _service = service;
        }

        [HttpGet("{usuarioId}")]
        public async Task<IActionResult> Get(int usuarioId)
        {
            var carrinho = await _service.GetByUsuario(usuarioId);
            return Ok(carrinho);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(AddItemCarrinhoDTO dto)
        {
            var carrinho = await _service.AddItem(dto);
            return Ok(carrinho);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdateQuantidadeDTO dto)
        {
            var carrinho = await _service.AtualizarQuantidade(dto);
            return Ok(carrinho);
        }

        [HttpDelete("{itemId}")]
        public async Task<IActionResult> Delete(int itemId)
        {
            var ok = await _service.RemoverItem(itemId);
            return ok ? Ok() : NotFound();
        }
    }
}
