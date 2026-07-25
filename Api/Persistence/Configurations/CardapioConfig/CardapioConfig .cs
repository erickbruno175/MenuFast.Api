using MenuFast.Api.Api.Domain.Entities.Models.Cardapio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations.CardapioConfig;

public class CardapioConfig : IEntityTypeConfiguration<Cardapio> {
    public void Configure(EntityTypeBuilder<Cardapio> builder) {
        builder.ToTable("Cardapio");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn(1001, 1);

        builder.Property(x => x.Id).HasComment("Identificador único do cardápio.");
        builder.Property(x => x.Nome).IsRequired().HasMaxLength(100).HasComment("Nome do cardápio.");
        builder.Property(x => x.Descricao).HasMaxLength(500).HasComment("Descrição do cardápio.");
        builder.Property(x => x.Ativo).HasComment("Indica se o cardápio está ativo.");
        builder.Property(x => x.DataCadastro).HasComment("Data de cadastro do cardápio.");
        builder.HasMany(x => x.Categorias).WithOne(x => x.Cardapio).HasForeignKey(x => x.CardapioId).OnDelete(DeleteBehavior.Cascade);
    }
}