using BancoDidital.Infrastructure.Data.Models.ContaCorrente;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoDidital.Infrastructure.Data.ConfigurationTable
{
    public class movimentoConfiguration : IEntityTypeConfiguration<movimento>
    {
        public void Configure(EntityTypeBuilder<movimento> builder)
        {
            builder.ToTable("tbMovimento");

            builder.HasKey(e => e.idMovimento);

            builder.Property(e => e.idMovimento)
                .HasColumnName("idMovimento").ValueGeneratedOnAdd();

            builder.Property(e => e.idContaCorrente)
                .HasColumnName("idContaCorrente");

            builder.Property(e => e.dataMovimento)
                .HasColumnName("dataMovimento");

            builder.Property(e => e.tipoMovimento)
                .HasColumnName("tipoMovimento");

            builder.Property(e => e.valor)
                .HasColumnName("valor");

        }
    }
}
