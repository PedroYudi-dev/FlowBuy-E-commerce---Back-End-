
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
    public class FornecedorController : ControllerBase
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IUserRepository _userRepository;

        public FornecedorController(IFornecedorRepository fornecedorRepository, IUserRepository userRepository)
        {
            _fornecedorRepository = fornecedorRepository;
            _userRepository = userRepository;
        }

        // GET (Todos):
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var fornecedores = _fornecedorRepository.GetAll();
                return Ok(fornecedores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar fornecedores: {ex.Message}");
            }
        }

        // GET (Por Id):
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var fornecedor = _fornecedorRepository.GetById(id);
                if (fornecedor == null)
                    return NotFound($"Fornecedor com id {id} não encontrado.");

                return Ok(fornecedor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar fornecedor: {ex.Message}");
            }
        }

        // POST:
        [HttpPost]
        public IActionResult Create([FromBody] FornecedorDTO fornecedorDto)
        {
            if (fornecedorDto == null)
                return BadRequest("Fornecedor não pode ser nulo.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Verifica se já existe User com o mesmo email
                if (_userRepository.ExisteEmail(fornecedorDto.Email))
                    return BadRequest("E-mail já cadastrado no sistema.");

                var creds = PasswordHelper.HashPassword(fornecedorDto.Senha ?? "");
                var user = new User
                {
                    Email = fornecedorDto.Email,
                    SenhaHash = creds.hash,
                    SenhaSalt = creds.salt,
                    Role = "Seller"
                };
                _userRepository.Add(user);

                var fornecedor = new Fornecedor
                {
                    Nome = fornecedorDto.Nome,
                    Cnpj = fornecedorDto.Cnpj,
                    Email = fornecedorDto.Email,
                    Data = DateTime.UtcNow,
                    UserId = user.Id // vínculo
                };
                _fornecedorRepository.Add(fornecedor);

                return CreatedAtAction(nameof(GetById), new { id = fornecedor.Id }, fornecedor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar fornecedor: {ex.Message}");
            }
        }

        // PUT:
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] FornecedorDTO fornecedorDto)
        {
            if (fornecedorDto == null)
                return BadRequest("Fornecedor não pode ser nulo.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (!_fornecedorRepository.ExisteFornecedor(id))
                    return NotFound($"Fornecedor com id {id} não encontrado.");

                var fornecedor = new Fornecedor
                {
                    Id = id,
                    Nome = fornecedorDto.Nome,
                    Cnpj = fornecedorDto.Cnpj,
                    Email = fornecedorDto.Email
                };

                // Atualizar senha se fornecida
                if (!string.IsNullOrEmpty(fornecedorDto.Senha))
                {
                    var creds = PasswordHelper.HashPassword(fornecedorDto.Senha);
                    fornecedor.SenhaHash = creds.hash;
                    fornecedor.SenhaSalt = creds.salt;
                }

                _fornecedorRepository.Update(fornecedor);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao atualizar fornecedor: {ex.Message}");
            }
        }

        // DELETE:
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (!_fornecedorRepository.ExisteFornecedor(id))
                    return NotFound($"Fornecedor com id {id} não encontrado.");

                _fornecedorRepository.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao deletar fornecedor: {ex.Message}");
            }
        }
    }
}
