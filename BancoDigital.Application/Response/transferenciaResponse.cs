using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoDigital.Application.Response
{
    public class transferenciaResponse
    {
        public int idContaCorrenteOrigem { get; set; }
        public int idContaCorrenteDestino { get; set; }
        public DateTime dataMovimento { get; set; }
        public decimal valor { get; set; }
    }
}
