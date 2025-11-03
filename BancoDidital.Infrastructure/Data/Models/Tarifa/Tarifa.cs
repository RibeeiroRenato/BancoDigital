using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoDidital.Infrastructure.Data.Models.Tarifa
{
    public class Tarifa
    {
        [Key]
        public int idTarifa { get; set; }
        public int idContaCorrente { get; set; }
        public DateTime dataMovimento { get; set; }
        public decimal valor { get; set; }
    }
}
