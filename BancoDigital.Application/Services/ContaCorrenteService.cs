using BancoDigital.Application.Exceptions;
using BancoDigital.Application.Interface;
using BancoDigital.Application.Request;
using BancoDigital.Application.Response;
using Microsoft.Extensions.Logging;


namespace BancoDigital.Application.Services
{
    public class ContaCorrenteService : IContaCorrente
    {
        private readonly ILogger<ContaCorrenteService> _logger;
        private readonly IContaCorrenteRepository _contaCorrenteRepository;
        private readonly TokenService _tokenService;

        public ContaCorrenteService(ILogger<ContaCorrenteService> logger, IContaCorrenteRepository contaCorrenteRepository, TokenService tokenService)
        {
            _logger = logger;
            _contaCorrenteRepository = contaCorrenteRepository;
            _tokenService = tokenService;
        }

        public Task cadastrarContaCorrente(ContaCorrenteRequest contaCorrente)
        {


            if (!ValidaCPF(contaCorrente.cpf))
            {
                throw new BusinessValidationException("INVALID_DOCUMENT");
            }

            _contaCorrenteRepository.cadastrarContaCorrente(contaCorrente);


            return Task.CompletedTask;
        }

        public static bool ValidaCPF(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            // Remove máscara
            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            // Tamanho deve ser 11
            if (cpf.Length != 11)
                return false;

            // Elimina CPFs com todos os dígitos iguais
            if (cpf.Distinct().Count() == 1)
                return false;

            // Calcula e valida os dois dígitos verificadores
            int[] mult1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] mult2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);

            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += (tempCpf[i] - '0') * mult1[i];

            int resto = (soma * 10) % 11;
            if (resto == 10) resto = 0;

            if (resto != (cpf[9] - '0'))
                return false;

            tempCpf += cpf[9];
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += (tempCpf[i] - '0') * mult2[i];

            resto = (soma * 10) % 11;
            if (resto == 10) resto = 0;

            return resto == (cpf[10] - '0');
        }

        public async Task<Task> MovimentoContaCorrente(movimentoRequest movimento)
        {
            var contaCorrente = await _contaCorrenteRepository.getContaCorrente(new ContaCorrenteRequest
            {
                idContaCorrente = movimento.idContaCorrente,
            });

            if (contaCorrente == null)
            {
                throw new BusinessValidationException("INVALID_ACOUNT");
            }

            if (!contaCorrente.ativo) 
            {
                throw new BusinessValidationException("INACTIVE_ACCOUNT");
            }

            if (movimento.valor < 0) 
            {
                throw new BusinessValidationException("INVALID_VALUE");
            }

            if (!movimento.tipoMovimento.Equals("D") || movimento.tipoMovimento.Equals("C")) 
            {
                throw new BusinessValidationException("IVALID_TYPE");
            }

            var tokenRequest = new ContaCorrenteRequest 
            {
               numeroContaCorrente = contaCorrente.numeroContaCorrente,
               Senha = contaCorrente.Senha,
               nome = contaCorrente.nome
            };

            var token = _tokenService.GenerateToken(tokenRequest);

            token = _tokenService.ValidateToken(token);

            if (token == null)
            {
                throw new BusinessValidationException("INVALID_TOKEN");
            }

            await _contaCorrenteRepository.MovimentoContaCorrente(movimento);
            return Task.CompletedTask;
        }

        Task<movimentoResponse> IContaCorrente.GetSaldoContaCorrente(ContaCorrenteRequest contaCorrente)
        {
            var saldoContaCorrente = _contaCorrenteRepository.getSaldoContaCorrente(contaCorrente);

            var result = new movimentoResponse
            {
                valor = saldoContaCorrente.Result.valor,
            };

            return Task.FromResult(result);
        }

        public async Task<contaCorrenteResponse> GetContaCorrente(ContaCorrenteRequest numeroContaCorrente)
        {
           var contaCorrente = await _contaCorrenteRepository.getContaCorrente(numeroContaCorrente);

            if (contaCorrente == null)
            {
                throw new BusinessValidationException("INVALID_ACOUNT");
            }
            return contaCorrente;
        }
    }
}
