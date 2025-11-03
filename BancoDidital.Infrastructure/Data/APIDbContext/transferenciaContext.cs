using BancoDidital.Infrastructure.Data.Models.Transferencia;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoDidital.Infrastructure.Data.DbContext
{
    public class transferenciaContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public transferenciaContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Transferencia> transferencia { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(transferenciaContext).Assembly);
        }
    }
}
