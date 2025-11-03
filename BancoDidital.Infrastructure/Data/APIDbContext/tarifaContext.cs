using BancoDidital.Infrastructure.Data.Models.Tarifa;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoDidital.Infrastructure.Data.DbContext
{
    public class tarifaContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public tarifaContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Tarifa> tarifa { get; set; }
    }
}
