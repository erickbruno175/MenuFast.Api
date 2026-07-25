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
        builder.Property(x => x.CardapioId).HasComment("Cardápio ao qual a categoria pertence.");
        builder.Property(x => x.Nome).IsRequired().HasMaxLength(100).HasComment("Nome da categoria.");
        builder.Property(x => x.Descricao).HasMaxLength(500).HasComment("Descrição da categoria.");
        builder.Property(x => x.Ordem).HasComment("Ordem de exibição da categoria no cardápio.");
        builder.Property(x => x.Ativo).HasComment("Indica se a categoria está ativa.");
        builder.HasOne(x => x.Cardapio).WithMany(x => x.Categorias).HasForeignKey(x => x.CardapioId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.Produtos).WithOne(x => x.CategoriaProduto).HasForeignKey(x => x.CategoriaProdutoId).OnDelete(DeleteBehavior.Cascade);
    }
}