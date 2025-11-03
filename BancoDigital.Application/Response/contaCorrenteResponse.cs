using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BancoDigital.Application.Response
{
    public class contaCorrenteResponse
    {
        public string nome { get; set; }
        public string? numeroContaCorrente { get; set; }
        public string? Senha { get; set; }
        public bool ativo { get; set; }
        public decimal saldo { get; set; }
        public string cpf { get; set; }
    }
}
