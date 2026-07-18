using MenuFast.Api.Api.Domain.Entities.Models.Cardapio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations.CardapioConfig;

public class OpcaoProdutoConfig : IEntityTypeConfiguration<OpcaoProduto> {
    public void Configure(EntityTypeBuilder<OpcaoProduto> builder) {
        builder.ToTable("OpcaoProduto");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasComment("Identificador único da opção do produto.");
        builder.Property(x => x.ProdutoId).HasComment("Produto ao qual a opção pertence.");
        builder.Property(x => x.Nome).IsRequired().HasMaxLength(100).HasComment("Nome da opção do produto.");
        builder.Property(x => x.Acrescimo).HasPrecision(18, 2).HasComment("Valor de acréscimo da opção.");
        builder.HasOne(x => x.Produto).WithMany().HasForeignKey(x => x.ProdutoId).OnDelete(DeleteBehavior.Cascade);
    }
}