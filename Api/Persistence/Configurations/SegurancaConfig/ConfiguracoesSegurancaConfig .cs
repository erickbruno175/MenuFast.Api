using MenuFast.Api.Api.Domain.Entities.Models.Seguranca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations.SegurancaConfig;

public class ConfiguracaoSegurancaConfig
    : IEntityTypeConfiguration<ConfiguracaoSeguranca> {
    public void Configure(EntityTypeBuilder<ConfiguracaoSeguranca> builder) {
        builder.ToTable("ConfiguracaoSeguranca");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn(1, 1);
        builder.Property(x => x.LojaId).IsRequired();
        builder.Property(x => x.MaxTentativasLogin).IsRequired();
        builder.Property(x => x.TempoBloqueioMinutos).IsRequired();
        builder.Property(x => x.TempoExpiracaoSessaoDias).IsRequired();
        builder.HasOne(x => x.Loja).WithOne().HasForeignKey<ConfiguracaoSeguranca>(x => x.LojaId).OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(x => x.LojaId).IsUnique();
    }
}