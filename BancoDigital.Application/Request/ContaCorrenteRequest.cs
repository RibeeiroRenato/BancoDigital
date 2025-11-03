using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BancoDigital.Application.Request
{
    public class ContaCorrenteRequest
    {
        [Key]
        public int idContaCorrente { get; set; }
        public string nome { get; set; }
        public string? numeroContaCorrente { get; set; }
        public string? Senha { get; set; }
        public bool ativo { get; set; }
        public decimal Saldo { get; set; }
        public string cpf { get; set; }
    }
}
