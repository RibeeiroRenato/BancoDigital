using BancoDidital.Infrastructure.Data.Models.ContaCorrente;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace BancoDidital.Infrastructure.Data.ConfigurationTable
{
    public class contaCorrenteConfiguration : IEntityTypeConfiguration<ContaCorrente>
    {
        public void Configure(EntityTypeBuilder<ContaCorrente> builder)
        {
            builder.ToTable("tbContaCorrente");

            builder.HasKey(e => e.idContaCorrente);
            builder.Property(e => e.idContaCorrente)
                .HasColumnName("idContaCorrente").ValueGeneratedOnAdd();

            builder.Property(e => e.numeroContaCorrente)
            .HasColumnName("numeroContaCorrente");

            builder.Property(e => e.nome)
                .HasColumnName("nome");

            builder.Property(e => e.Senha)
                .HasColumnName("Senha");

            builder.Property(e => e.ativo)
                .HasColumnName("ativo");

            builder.Property(e => e.Saldo)
                .HasColumnName("salt");

            builder.Property(e => e.cpf)
                .HasColumnName("cpf");



        }
    }
}
