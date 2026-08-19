using MenuFast.Api.Api.Domain.Entities.Models.Cardapio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations.CardapioConfig;

public class CategoriaProdutoConfig : IEntityTypeConfiguration<CategoriaProduto> {
    public void Configure(EntityTypeBuilder<CategoriaProduto> builder) {
        builder.ToTable("CategoriaProduto");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn(1001, 1);
        builder.Property(x => x.Id).HasComment("Identificador único da categoria de produtos.");
        builder.Property(x => x.Nome).IsRequired().HasMaxLength(100).HasComment("Nome da categoria.");
        builder.HasOne(x => x.Loja).WithMany().HasForeignKey(x => x.LojaId).OnDelete(DeleteBehavior.NoAction);
    }
}