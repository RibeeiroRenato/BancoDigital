using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoDigital.Application.Response
{
    public class movimentoResponse
    {
        public int idContaCorrente { get; set; }
        public DateTime dataMovimento { get; set; }
        public string? tipoMovimento { get; set; }
        public decimal valor { get; set; }
    }
}
