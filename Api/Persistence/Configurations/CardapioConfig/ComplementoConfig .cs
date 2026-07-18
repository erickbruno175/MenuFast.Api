using MenuFast.Api.Api.Domain.Entities.Models.Cardapio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations.CardapioConfig;

public class ComplementoConfig : IEntityTypeConfiguration<Complemento> {
    public void Configure(EntityTypeBuilder<Complemento> builder) {
        builder.ToTable("Complemento");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasComment("Identificador único do complemento.");
        builder.Property(x => x.Nome).IsRequired().HasMaxLength(100).HasComment("Nome do complemento.");
        builder.Property(x => x.Preco).HasPrecision(18, 2).HasComment("Valor adicional do complemento.");
        builder.Property(x => x.Obrigatorio).HasComment("Indica se o complemento é obrigatório.");
        builder.Property(x => x.Ativo).HasComment("Indica se o complemento está ativo.");
        builder.HasMany(x => x.Produtos).WithMany(x => x.Complementos);
    }
}