
using Microsoft.AspNetCore.Mvc;
using api_ecommerce.Domain.Entities;
using api_ecommerce.Domain.Interfaces.Repositories;
using api_ecommerce.Domain.DTOs;
using System;
using api_ecommerce.Domain.Utils;
using api_ecommerce.Infrastructure.Repositories;

namespace api_ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IUserRepository _userRepository;

        public ClienteController(IClienteRepository clienteRepository,
    IFornecedorRepository fornecedorRepository,
    IUserRepository userRepository)
        {
            _clienteRepository = clienteRepository;
            _fornecedorRepository = fornecedorRepository;
            _userRepository = userRepository;
        }

        // GET (Todos):
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var clientes = _clienteRepository.GetAll();
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar clientes: {ex.Message}");
            }
        }

        // GET (Por Id):
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var cliente = _clienteRepository.GetById(id);
                if (cliente == null)
                    return NotFound($"Cliente com id {id} não encontrado.");

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar cliente: {ex.Message}");
            }
        }

        // POST:
        [HttpPost]
        public IActionResult Create([FromBody] ClienteDTO clienteDto)
        {
            if (clienteDto == null)
                return BadRequest("Fornecedor não pode ser nulo.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Verifica se já existe User com o mesmo email
                if (_userRepository.ExisteEmail(clienteDto.Email))
                    return BadRequest("E-mail já cadastrado no sistema.");

                var creds = PasswordHelper.HashPassword(clienteDto.Senha ?? "");
                var user = new User
                {
                    Email = clienteDto.Email,
                    SenhaHash = creds.hash,
                    SenhaSalt = creds.salt,
                    Role = "Buyer"
                };
                _userRepository.Add(user);

                var cliente = new Cliente
                {
                    Nome = clienteDto.Nome,
                    Cpf = clienteDto.Cpf,
                    Email = clienteDto.Email,
                    Data = DateTime.UtcNow,
                    UserId = user.Id // vínculo
                };
                _clienteRepository.Add(cliente);

                return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, cliente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar Cliente: {ex.Message}");
            }
        }

        // PUT:
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ClienteDTO clienteDto)
        {
            if (clienteDto == null)
                return BadRequest("Cliente não pode ser nulo.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (!_clienteRepository.ExisteCliente(id))
                    return NotFound($"Cliente com id {id} não encontrado.");

                var cliente = new Cliente
                {
                    Id = id,
                    Nome = clienteDto.Nome,
                    Cpf = clienteDto.Cpf,             // Adicionado
                    Email = clienteDto.Email
                };

                // Atualizar senha se fornecida
                if (!string.IsNullOrEmpty(clienteDto.Senha))
                {
                    var creds = PasswordHelper.HashPassword(clienteDto.Senha);
                    cliente.SenhaHash = creds.hash;
                    cliente.SenhaSalt = creds.salt;
                }

                _clienteRepository.Update(cliente);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar cliente: {ex.Message}");
            }
        }

        // DELETE:
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (!_clienteRepository.ExisteCliente(id))
                    return NotFound($"Cliente com id {id} não encontrado.");

                _clienteRepository.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao deletar cliente: {ex.Message}");
            }
        }
    }
}
