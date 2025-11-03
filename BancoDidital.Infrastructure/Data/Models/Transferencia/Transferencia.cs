using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoDidital.Infrastructure.Data.Models.Transferencia
{
    public class Transferencia
    {
        [Key]
        public int idTransferencia { get; set; }
        public int idContaCorrenteOrigem { get; set; }
        public int idContaCorrenteDestino { get; set; }
        public DateTime dataMovimento { get; set; }
        public decimal valor { get; set; }
    }
}
