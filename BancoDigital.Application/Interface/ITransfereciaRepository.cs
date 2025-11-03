using BancoDigital.Application.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoDigital.Application.Interface
{
    public interface ITransfereciaRepository
    {
        Task<string> TransferenciaContaCorrente(transferenciaRequest transferecia);
    }
}
