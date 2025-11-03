using System.ComponentModel.DataAnnotations;


namespace BancoDidital.Infrastructure.Data.Models.ContaCorrente
{
    public class movimento
    {
        [Key]
        public int idMovimento { get; set; }
        public int idContaCorrente { get; set; }
        public string tipoMovimento { get; set; }
        public decimal valor { get; set; }
        public DateTime dataMovimento { get; set; }
    }
}
