using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoDidital.Infrastructure.Data.Models
{
    public class Idempotencia
    {
        public int idIdempotencia { get; set; }
        public string? requisicao { get; set; }
        public string? resultado { get; set; }
    }
}
