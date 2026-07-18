using MenuFast.Api.Api.Domain.Entities.Models.Pedido;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations.PedidoConfig;

public class ItemPedidoConfig : IEntityTypeConfiguration<ItemPedido> {
    public void Configure(EntityTypeBuilder<ItemPedido> builder) {
        builder.ToTable("ItemPedido");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasComment("Identificador único do item do pedido.");
        builder.Property(x => x.PedidoId).HasComment("Pedido vinculado ao item.");
        builder.Property(x => x.ProdutoId).HasComment("Produto vinculado ao item.");
        builder.Property(x => x.Quantidade).HasPrecision(18, 3).HasComment("Quantidade do produto no pedido.");
        builder.Property(x => x.ValorUnitario).HasPrecision(18, 2).HasComment("Valor unitário do produto.");
        builder.Property(x => x.Desconto).HasPrecision(18, 2).HasComment("Desconto aplicado no item.");
        builder.Property(x => x.Total).HasPrecision(18, 2).HasComment("Valor total do item.");
        builder.Property(x => x.Observacao).HasMaxLength(500).HasComment("Observação do item do pedido.");
        builder.HasOne(x => x.Pedido)
            .WithMany(x => x.Itens)
            .HasForeignKey(x => x.PedidoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}