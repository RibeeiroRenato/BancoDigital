using BancoDigital.Application.Request;
using BancoDigital.Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoDigital.Application.Interface
{
    public interface ITransferencia
    {
        Task<transferenciaResponse> Transferencia(transferenciaRequest transferencia);
    }
}
