using Microsoft.AspNetCore.Mvc;
using api_ecommerce.Domain.DTOs;
using api_ecommerce.Domain.Interfaces.Repositories;
using api_ecommerce.Domain.Utils;
using api_ecommerce.Infrastructure.Repositories;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IFornecedorRepository _fornecedorRepository;

    public AuthController(IUserRepository userRepository,
    IClienteRepository clienteRepository,
    IFornecedorRepository fornecedorRepository)
    {
        _userRepository = userRepository;
        _clienteRepository = clienteRepository;
        _fornecedorRepository = fornecedorRepository;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDTO loginDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = _userRepository.GetByEmail(loginDto.Email);
        if (user == null)
            return Unauthorized("Usuário não encontrado");

        if (!PasswordHelper.VerifyPassword(loginDto.Senha, user.SenhaHash, user.SenhaSalt))
            return Unauthorized("Senha inválida");

        // Verifica se é Cliente
        var cliente = _clienteRepository.GetAll().FirstOrDefault(c => c.UserId == user.Id);
        // Verifica se é Fornecedor
        var fornecedor = _fornecedorRepository.GetAll().FirstOrDefault(f => f.UserId == user.Id);

        return Ok(new
        {
            UserId = user.Id,
            Email = user.Email,
            Role = user.Role,
            ClienteId = cliente?.Id,
            FornecedorId = fornecedor?.Id
        });
    }
}