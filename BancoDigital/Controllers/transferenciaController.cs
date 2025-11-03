using BancoDigital.Application.Interface;
using BancoDigital.Application.Request;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BancoDigital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class transferenciaController: ControllerBase
    {    
        private readonly ILogger<transferenciaController> _logger;
        private readonly ITransferencia _transferencia;

        public transferenciaController(ILogger<transferenciaController> logger, ITransferencia transferencia)
        {
            _logger = logger;
            _transferencia = transferencia;
        }

        [HttpPost("/transferencia")]
        public async Task<IActionResult> Transferencia([FromBody] transferenciaRequest transferencia)
        {

            await _transferencia.Transferencia(transferencia);
            _logger.LogInformation("Transferencia realizada com sucesso.");
            return Ok(new { mensagem = "Transferencia realizada com sucesso." });
        }
    }
}
