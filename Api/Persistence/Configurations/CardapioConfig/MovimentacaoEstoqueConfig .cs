using MenuFast.Api.Api.Domain.Entities.Models.Cardapio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations.Cardapio;

public class MovimentacaoEstoqueConfig : IEntityTypeConfiguration<MovimentacaoEstoque> {
    public void Configure(EntityTypeBuilder<MovimentacaoEstoque> builder) {
        builder.ToTable("MovimentacaoEstoque");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn(1001, 1);

        builder.Property(x => x.Id).HasComment("Identificador único da movimentação de estoque.");
        builder.Property(x => x.EstoqueProdutoId).HasComment("Estoque do produto vinculado à movimentação.");
        builder.Property(x => x.Tipo).HasComment("Tipo da movimentação realizada no estoque.");
        builder.Property(x => x.Quantidade).HasComment("Quantidade movimentada no estoque.");
        builder.Property(x => x.QuantidadeAnterior).HasComment("Quantidade disponível no estoque antes da movimentação.");
        builder.Property(x => x.QuantidadeAtual).HasComment("Quantidade disponível no estoque após a movimentação.");
        builder.Property(x => x.Observacao).HasMaxLength(500).HasComment("Observação referente à movimentação de estoque.");
        builder.Property(x => x.PedidoId).HasComment("Pedido relacionado à movimentação, quando aplicável.");
        builder.Property(x => x.DataCadastro).HasComment("Data e hora em que a movimentação foi registrada.");

        builder.HasOne(x => x.EstoqueProduto)
            .WithMany(x => x.Movimentacoe)
            .HasForeignKey(x => x.EstoqueProdutoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.EstoqueProdutoId);
        builder.HasIndex(x => x.PedidoId);
    }
}