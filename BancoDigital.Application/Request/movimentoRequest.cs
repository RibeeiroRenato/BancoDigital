using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoDigital.Application.Request
{
    public class movimentoRequest
    {
        public int idMovimento { get; set; }
        public int idContaCorrente { get; set; }
        public DateTime dataMovimento { get; set; }
        public string? tipoMovimento { get; set; }
        public decimal valor { get; set; }
    }
}
