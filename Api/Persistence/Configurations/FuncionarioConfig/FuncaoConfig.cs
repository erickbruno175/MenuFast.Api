using MenuFast.Api.Api.Domain.Entities.Models.Funcionario;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations;

public class FuncaoConfig : IEntityTypeConfiguration<Funcao> {
    public void Configure(EntityTypeBuilder<Funcao> builder) {
        builder.ToTable("Funcao");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn(1, 1);

        builder.Property(x => x.Nome).IsRequired().HasMaxLength(100);
        builder.HasIndex(x => x.Nome).IsUnique();
        builder.Property(x => x.Descricao).HasMaxLength(255);
        builder.Property(x => x.Ativo).IsRequired();
        builder.HasMany(x => x.Funcionarios).WithOne(x => x.Funcao).HasForeignKey(x => x.FuncaoId).OnDelete(DeleteBehavior.Restrict);
    }
}