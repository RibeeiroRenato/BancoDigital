using BancoDidital.Infrastructure.Data.Models.ContaCorrente;
using Microsoft.EntityFrameworkCore;

namespace BancoDidital.Infrastructure.Data.DbContext
{
    public class contaCorrenteContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public contaCorrenteContext(DbContextOptions<contaCorrenteContext> options) : base(options) { }

        public DbSet<ContaCorrente> contaCorrente { get; set; }
        public DbSet<movimento> movimento { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(contaCorrenteContext).Assembly);
        }
    }
}
