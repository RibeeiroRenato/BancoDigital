using BancoDidital.Infrastructure.Data.Models.Transferencia;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoDidital.Infrastructure.Data.ConfigurationTable
{
    public class transferenciaConfiguration : IEntityTypeConfiguration<Transferencia>
    {
        public void Configure(EntityTypeBuilder<Transferencia> builder)
        {
            builder.ToTable("tbTransferencia");
            builder.HasKey(e => e.idTransferencia);

            builder.Property(e => e.idContaCorrenteOrigem)
                .HasColumnName("idContaCorrenteOrigem");

            builder.Property(e => e.idContaCorrenteDestino)
                .HasColumnName("idContaCorrenteDestino");

            builder.Property(e => e.dataMovimento)
                .HasColumnName("dataMovimento");

            builder.Property(e => e.valor)
                .HasColumnName("valor");
        }
    }
}
