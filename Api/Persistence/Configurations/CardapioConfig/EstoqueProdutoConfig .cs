using MenuFast.Api.Api.Domain.Entities.Models.Cardapio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations.Cardapio;

public class EstoqueProdutoConfig : IEntityTypeConfiguration<EstoqueProduto> {
    public void Configure(EntityTypeBuilder<EstoqueProduto> builder) {
        builder.ToTable("EstoqueProduto");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn(1001, 1);

        builder.Property(x => x.Id).HasComment("Identificador único do estoque do produto.");
        builder.Property(x => x.ProdutoId).HasComment("Produto vinculado ao estoque.");
        builder.Property(x => x.Quantidade).HasComment("Quantidade atual disponível em estoque.");
        builder.Property(x => x.EstoqueMinimo).HasComment("Quantidade mínima de estoque utilizada para gerar alerta de estoque baixo.");
        builder.Property(x => x.DataCadastro).HasComment("Data e hora em que o controle de estoque foi cadastrado.");
        builder.Property(x => x.DataAtualizacao).HasComment("Data e hora da última atualização do estoque.");
        builder.Property(x => x.AlertaEstoqueEnviado).HasComment("Indica se o alerta de estoque baixo já foi enviado.");
        builder.Property(x => x.UltimoAlertaEstoque).HasComment("Data e hora do último alerta de estoque enviado.");
        builder.HasOne(x => x.Produto)
            .WithOne(x => x.EstoqueProduto)
            .HasForeignKey<EstoqueProduto>(x => x.ProdutoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.ProdutoId).IsUnique();
    }
}