using BancoDidital.Infrastructure.Data.Models.ContaCorrente;
using BancoDigital.Application.Request;
using BancoDigital.Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoDigital.Application.Interface
{
    public interface IContaCorrente
    {
        Task cadastrarContaCorrente(ContaCorrenteRequest contaCorrente);
        Task<Task> MovimentoContaCorrente(movimentoRequest movimento);
        Task<movimentoResponse> GetSaldoContaCorrente(ContaCorrenteRequest contaCorrente);
        Task<contaCorrenteResponse> GetContaCorrente(ContaCorrenteRequest numeroContaCorrente);
    }
}
