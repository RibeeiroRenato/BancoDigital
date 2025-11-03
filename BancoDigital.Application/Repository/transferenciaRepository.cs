using BancoDigital.Application.Interface;
using BancoDigital.Application.Request;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;


namespace BancoDigital.Application.Repository
{
    public class transferenciaRepository : ITransfereciaRepository
    {
        private readonly ILogger<transferenciaRepository> _logger;
        private readonly IContaCorrenteRepository _contaCorrenteRepository;
        private readonly HttpClient _httpClient;

        public transferenciaRepository(ILogger<transferenciaRepository> logger, IContaCorrenteRepository contaCorrenteRepository, HttpClient httpClient)
        {
            _logger = logger;
            _contaCorrenteRepository = contaCorrenteRepository;
            _httpClient = httpClient;
        }

        public Task<string> TransferenciaContaCorrente(transferenciaRequest transferecia)
        {
            return Task.Run(async () =>
            {
                var url = "https://localhost:7187/movimento";
                var movimentoDestino = new movimentoRequest
                {
                    idContaCorrente = transferecia.idContaCorrenteDestino,
                    tipoMovimento = "D",
                    valor = transferecia.valor,
                    dataMovimento = DateTime.Now
                };
                var json = System.Text.Json.JsonSerializer.Serialize(movimentoDestino);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Erro ao realizar a transferência: {StatusCode}", response.StatusCode);
                    throw new Exception("Erro ao realizar a transferência.");
                }
                _logger.LogInformation("Transferência realizada com sucesso para a conta: {ContaDestino}", transferecia.idContaCorrenteDestino);
                return "Transferência realizada com sucesso.";
            });
        }
    }
}
