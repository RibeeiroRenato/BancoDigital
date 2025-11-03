using Microsoft.AspNetCore.Mvc;

namespace BancoDigital.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class movimentoController:ControllerBase
    {
        private readonly ILogger<movimentoController> _logger;
    }
}
