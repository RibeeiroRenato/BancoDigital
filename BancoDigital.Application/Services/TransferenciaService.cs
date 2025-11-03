using Azure.Core;
using BancoDidital.Infrastructure.Data.Models.Transferencia;
using BancoDigital.Application.Interface;
using BancoDigital.Application.Request;
using BancoDigital.Application.Response;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BancoDigital.Application.Services
{
    public class TransferenciaService : ITransferencia
    {
        private readonly ILogger<TransferenciaService> _logger;
        private readonly ITransfereciaRepository _transfereciaRepository;
        private readonly HttpClient _httpClient;

        public TransferenciaService(ILogger<TransferenciaService> logger, ITransfereciaRepository transfereciaRepository)
        {
            _logger = logger;
            _transfereciaRepository = transfereciaRepository;
        }
        public async Task<transferenciaResponse> Transferencia(transferenciaRequest transferencia)
        {

            var url = "https://localhost:7187/movimento";

            var movimentoOrigem = new movimentoRequest
            {
                idContaCorrente = transferencia.idContaCorrenteOrigem,
                tipoMovimento = "C",
                valor = transferencia.valor,
                dataMovimento = DateTime.Now
            };

            var json = JsonSerializer.Serialize(transferencia);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Erro ao realizar a transferência: {StatusCode}", response.StatusCode);
                throw new Exception("Erro ao realizar a transferência.");
            }

            var newTransferencia = new transferenciaRequest
            {
                idContaCorrenteOrigem = transferencia.idContaCorrenteOrigem,
                idContaCorrenteDestino = transferencia.idContaCorrenteDestino,
                valor = transferencia.valor,
                dataMovimento = DateTime.Now
            };

            var transferenciaDone = await _transfereciaRepository.TransferenciaContaCorrente(transferencia);

            // Read the response content as transferenciaResponse
            return await response.Content.ReadFromJsonAsync<transferenciaResponse>();
        }
    }
}
