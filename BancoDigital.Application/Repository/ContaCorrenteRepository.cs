using BancoDigital.Application.Interface;
using BancoDigital.Application.Request;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using BancoDidital.Infrastructure.Data.DbContext;
using BancoDidital.Infrastructure.Data.Models.ContaCorrente;
using BancoDigital.Application.Response;

namespace BancoDigital.Application.Repository
{
    public class ContaCorrenteRepository : IContaCorrenteRepository
    {
        private readonly contaCorrenteContext _context;
        private readonly ILogger<ContaCorrenteRepository> Logger;

        public ContaCorrenteRepository(contaCorrenteContext context, ILogger<ContaCorrenteRepository> logger)
        {
            _context = context;
            Logger = logger;
        }

        public async Task<string> cadastrarContaCorrente(ContaCorrenteRequest contaCorrente)
        {
            if (contaCorrente is null)
                throw new ArgumentNullException(nameof(contaCorrente));

            if (string.IsNullOrWhiteSpace(contaCorrente.numeroContaCorrente))
                throw new ArgumentException("numeroContaCorrente is required", nameof(contaCorrente.numeroContaCorrente));


            var contaCorrenteNew = new ContaCorrente
            {
                idContaCorrente = contaCorrente.idContaCorrente,
                nome = contaCorrente.nome,
                cpf = contaCorrente.cpf,
                numeroContaCorrente = contaCorrente.numeroContaCorrente,
                Senha = contaCorrente.Senha,
                // convert bool -> int (true => 1, false => 0)
                ativo = contaCorrente.ativo ? 1 : 0,
                Saldo = contaCorrente.Saldo
            };

            try
            {
                _context.contaCorrente.Add(contaCorrenteNew);
                await _context.SaveChangesAsync();
                Logger.LogInformation("Conta criada: {numero}", contaCorrenteNew.numeroContaCorrente);
                return contaCorrenteNew.numeroContaCorrente!;
            }
            catch (DbUpdateException ex)
            {
                Logger.LogError(ex, "Save failed: {Inner}", ex.InnerException?.Message ?? ex.Message);
                throw;
            }
        }

        public async Task<string> MovimentoContaCorrente(movimentoRequest numeroContaCorrente)
        {
            if (numeroContaCorrente is null)
                throw new ArgumentNullException(nameof(numeroContaCorrente));
            var movimentoNew = new movimento
            {
                idMovimento = numeroContaCorrente.idMovimento,
                idContaCorrente = numeroContaCorrente.idContaCorrente,
                dataMovimento = numeroContaCorrente.dataMovimento,
                tipoMovimento = numeroContaCorrente.tipoMovimento,
                valor = numeroContaCorrente.valor
            };
            try
            {
                _context.movimento.Add(movimentoNew);
                await _context.SaveChangesAsync();
                Logger.LogInformation("Movimento realizado: {id}", movimentoNew.idMovimento);
                return movimentoNew.idMovimento.ToString();
            }
            catch (DbUpdateException ex)
            {
                Logger.LogError(ex, "Save failed: {Inner}", ex.InnerException?.Message ?? ex.Message);
                throw;
            }
        }

        public async Task<contaCorrenteResponse> GetContaCorrente(ContaCorrenteRequest contaCorrente)
        {
            var idContaCorretne = await _context.contaCorrente
                .Where(c => c.idContaCorrente == contaCorrente.idContaCorrente)
                .FirstOrDefaultAsync();

            if (idContaCorretne == null)
            {
                throw new KeyNotFoundException("Conta corrente não encontrada.");
            }

            var result = new contaCorrenteResponse
            {
                nome = idContaCorretne.nome,
                numeroContaCorrente = idContaCorretne.numeroContaCorrente,
                ativo = idContaCorretne.ativo == 1,
                saldo = idContaCorretne.Saldo,
                cpf = idContaCorretne.cpf
            };

            return result;
        }

        public async Task<movimentoResponse> getSaldoContaCorrente(ContaCorrenteRequest contaCorrente)
        {
            var saldoConta = await _context.movimento
                .Where(c => c.idContaCorrente == contaCorrente.idContaCorrente)
                .Select(m => m.tipoMovimento == "C" ? m.valor : -m.valor)
                .DefaultIfEmpty(0)
                .SumAsync();

            return new movimentoResponse
            {
                valor = saldoConta
            };
        }

        public async Task<contaCorrenteResponse> getContaCorrente(ContaCorrenteRequest contaCorrente) 
        {
            var conta = await _context.contaCorrente
                .FirstOrDefaultAsync(c => c.numeroContaCorrente == contaCorrente.numeroContaCorrente);

            if (conta == null)
                {
                throw new KeyNotFoundException("Conta corrente não encontrada.");
            }

            return new contaCorrenteResponse
            {
                nome = conta.nome,
                numeroContaCorrente = conta.numeroContaCorrente,
                ativo = conta.ativo == 1,
                saldo = conta.Saldo,
                cpf = conta.cpf
            };
        }
    }
 }
