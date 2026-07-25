using MenuFast.Api.Api.Domain.Entities.Models.Funcionario;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations;

public class PerfilConfiguration : IEntityTypeConfiguration<Perfil>
{
    public void Configure(EntityTypeBuilder<Perfil> builder)
    {
        builder.ToTable("Perfil");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn(1, 1);

        builder.Property(x => x.Nome).IsRequired().HasMaxLength(100);
        builder.HasIndex(x => x.Nome).IsUnique();
        builder.Property(x => x.Descricao).HasMaxLength(255);
        builder.Property(x => x.Ativo).IsRequired();
        builder.HasMany(x => x.PerfilPermissoes).WithOne(x => x.Perfil).HasForeignKey(x => x.PerfilId).OnDelete(DeleteBehavior.Cascade);
    }
}