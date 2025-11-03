using System.ComponentModel.DataAnnotations;

namespace BancoDidital.Infrastructure.Data.Models.ContaCorrente
{
    public class ContaCorrente
    {
        [Key]
        public int idContaCorrente { get; set; }
        public string nome { get; set; }
        public string? numeroContaCorrente { get; set; }
        public string? Senha { get; set; }
        public int ativo { get; set; }
        public decimal Saldo { get; set; }
        public string cpf { get; set; }
    }
}
