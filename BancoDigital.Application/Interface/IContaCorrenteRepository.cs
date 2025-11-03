using BancoDigital.Application.Request;
using BancoDigital.Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoDigital.Application.Interface
{
    public interface IContaCorrenteRepository
    {
        Task<string> cadastrarContaCorrente(ContaCorrenteRequest contaCorrente);
        Task<string> MovimentoContaCorrente(movimentoRequest numeroContaCorrente);

        Task<contaCorrenteResponse> getContaCorrente(ContaCorrenteRequest contaCorrete);

        Task<movimentoResponse> getSaldoContaCorrente(ContaCorrenteRequest contaCorrente);
    }
}
