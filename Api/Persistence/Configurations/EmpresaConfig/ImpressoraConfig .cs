using MenuFast.Api.Api.Domain.Entities.Models.ConfiguracoesEmpresa;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations.EmpresaConfig;

public class ImpressoraConfig : IEntityTypeConfiguration<Impressora> {
    public void Configure(EntityTypeBuilder<Impressora> builder) {
        builder.ToTable("Impressora");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasComment("Identificador único da impressora.");
        builder.Property(x => x.TerminalId).HasComment("Terminal ao qual a impressora está vinculada.");
        builder.Property(x => x.Nome).IsRequired().HasMaxLength(100).HasComment("Nome da impressora.");
        builder.Property(x => x.Modelo).IsRequired().HasMaxLength(100).HasComment("Modelo da impressora.");
        builder.Property(x => x.EnderecoIp).IsRequired().HasMaxLength(100).HasComment("Endereço IP ou hostname da impressora.");
        builder.Property(x => x.Porta).HasComment("Porta de comunicação da impressora.");
        builder.Property(x => x.Padrao).HasComment("Indica se esta é a impressora padrão do terminal.");
        builder.Property(x => x.Ativa).HasComment("Indica se a impressora está ativa.");
        builder.HasOne(x => x.Terminal).WithMany().HasForeignKey(x => x.TerminalId).OnDelete(DeleteBehavior.Cascade);
    }
}