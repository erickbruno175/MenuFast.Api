using MenuFast.Api.Api.Domain.Entities.Models.Seguranca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations.SegurancaConfig;

public class PermissaoConfigu: IEntityTypeConfiguration<Permissao> {
    public void Configure(EntityTypeBuilder<Permissao> builder) {
        builder.ToTable("Permissao");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn(1, 1);
        builder.Property(x => x.Descricao).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Codigo).IsRequired().HasMaxLength(100);
        builder.HasIndex(x => x.Codigo).IsUnique();
        builder.Property(x => x.Descricao).HasMaxLength(255);
        builder.HasMany(x => x.PerfilPermissoes).WithOne(x => x.Permissao).HasForeignKey(x => x.PermissaoId).OnDelete(DeleteBehavior.Cascade);
    }
}