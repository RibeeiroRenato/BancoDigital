using BancoDidital.Infrastructure.Data.DbContext;
using BancoDigital.Application.Request;
using BancoDigital.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BancoDigital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly contaCorrenteContext _context;
        private readonly TokenService tokenService;

        public AuthController(contaCorrenteContext context, TokenService tokenService)
        {
            _context = context;
            this.tokenService = tokenService;
        }

        [HttpPost("/login")]
        public IActionResult Login([FromBody] ContaCorrenteRequest request)
        {
            var user = _context.contaCorrente
                .FirstOrDefault(u => u.numeroContaCorrente == request.numeroContaCorrente && u.Senha == request.Senha);

            if (user == null)
            {
                return Unauthorized(new { mensagem = "Número da conta ou senha inválidos." });
            }

            var newUser = new ContaCorrenteRequest
            {
                idContaCorrente = user.idContaCorrente,
                nome = user.nome,
                numeroContaCorrente = user.numeroContaCorrente,
                Senha = user.Senha,
                ativo = user.ativo == 1,
                Saldo = user.Saldo,
                cpf = user.cpf
            };

            var token = tokenService.GenerateToken(newUser);

            return Ok(new
            {
                mensagem = "Login realizado com sucesso!",
                token = token
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] ContaCorrenteRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.numeroContaCorrente) || string.IsNullOrWhiteSpace(request.Senha))
            {
                return BadRequest(new { mensagem = "Número da conta e senha são obrigatórios." });
            }
            var existingUser = _context.contaCorrente
                .FirstOrDefault(u => u.numeroContaCorrente == request.numeroContaCorrente);
            if (existingUser != null)
            {
                return Conflict(new { mensagem = "Número da conta já está em uso." });
            }
            var newUser = new BancoDidital.Infrastructure.Data.Models.ContaCorrente.ContaCorrente
            {
                nome = request.nome,
                numeroContaCorrente = request.numeroContaCorrente,
                Senha = request.Senha,
                ativo = request.ativo ? 1 : 0,
                Saldo = request.Saldo,
                cpf = request.cpf
            };
            _context.contaCorrente.Add(newUser);
            await _context.SaveChangesAsync();
            return Ok(new { mensagem = "Usuário registrado com sucesso!" });
        }
    }
}
