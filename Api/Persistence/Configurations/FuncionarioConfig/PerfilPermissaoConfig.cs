using MenuFast.Api.Api.Domain.Entities.Models.Funcionario;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations;

public class PerfilPermissaoConfig : IEntityTypeConfiguration<PerfilPermissao> {
    public void Configure(EntityTypeBuilder<PerfilPermissao> builder) {
        builder.ToTable("PerfilPermissao");
        builder.HasKey(x => new{x.PerfilId,x.PermissaoId});
        builder.HasOne(x => x.Perfil).WithMany(x => x.PerfilPermissoes).HasForeignKey(x => x.PerfilId);
        builder.HasOne(x => x.Permissao).WithMany(x => x.PerfilPermissoes).HasForeignKey(x => x.PermissaoId);
    }
}