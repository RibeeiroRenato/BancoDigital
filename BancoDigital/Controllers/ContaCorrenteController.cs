using BancoDidital.Infrastructure.Data.Models.ContaCorrente;
using BancoDigital.Application.Exceptions;
using BancoDigital.Application.Interface;
using BancoDigital.Application.Request;
using BancoDigital.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BancoDigital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly ILogger<ContaCorrenteController> _logger;
        private readonly IContaCorrente _contaCorrenteService;

        public ContaCorrenteController(
            ILogger<ContaCorrenteController> logger,
            IContaCorrente contaCorrenteService,
            TokenService tokenService)
        {
            _logger = logger;
            _contaCorrenteService = contaCorrenteService;
        }

        [HttpPost("/cadastrarContaCorrente")]
        public IActionResult CadastrarContaCorrente([FromBody] ContaCorrenteRequest contaCorrente)
        {
            try
            {
                _contaCorrenteService.cadastrarContaCorrente(contaCorrente);

                return Ok(new
                {
                    mensagem = "Conta cadastrada com sucesso!",
                    dados = contaCorrente.numeroContaCorrente
                });
            }
            catch (BusinessValidationException ex)
            {
                _logger.LogError(ex, "Erro ao cadastrar conta corrente: {Message}", ex.Message);
                return BadRequest(new
                {
                    erro = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao cadastrar conta corrente: {Message}", ex.Message);
                return StatusCode(400, new
                {
                    erro = "INVALID_DOCUMENT"
                });
            }
        }

        [HttpPost("/movimento")]
        public async Task<IActionResult> MovimentoContaCorrente([FromBody] movimentoRequest movimento)
        {

            await _contaCorrenteService.MovimentoContaCorrente(movimento);

            return Ok(new
            {
                mensagem = "Movimento realizado com sucesso!",
                dados = movimento
            });
        }

        [HttpGet("/Saldo")]
        public IActionResult GetSaldo([FromQuery] ContaCorrenteRequest numeroContaCorrente)
        {
            try
            {
                var saldo = _contaCorrenteService.GetSaldoContaCorrente(numeroContaCorrente);
                return Ok(new
                {
                    mensagem = "Saldo obtido com sucesso!",
                    dados = saldo
                });
            }
            catch (BusinessValidationException ex)
            {
                _logger.LogError(ex, "Erro ao obter saldo: {Message}", ex.Message);
                return BadRequest(new
                {
                    erro = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter saldo: {Message}", ex.Message);
                return StatusCode(400, new
                {
                    erro = "ERROR_OBTAINING_BALANCE"
                });
            }
        }

        [HttpGet("/contaCorrente")]
        public IActionResult GetContaCorrente([FromQuery] ContaCorrenteRequest numeroContaCorrente)
        {
            try
            {
                var contaCorrente = _contaCorrenteService.GetContaCorrente(numeroContaCorrente);
                return Ok(new
                {
                    mensagem = "Conta corrente obtida com sucesso!",
                    dados = contaCorrente
                });
            }
            catch (BusinessValidationException ex)
            {
                _logger.LogError(ex, "Erro ao obter conta corrente: {Message}", ex.Message);
                return BadRequest(new
                {
                    erro = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter conta corrente: {Message}", ex.Message);
                return StatusCode(400, new
                {
                    erro = "ERROR_OBTAINING_CURRENT_ACCOUNT"
                });
            }
        }

    }
}
