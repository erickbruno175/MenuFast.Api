using MenuFast.Api.Api.Domain.Entities.Models.Pedido;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations.PedidoConfig;

public class PedidoConfig : IEntityTypeConfiguration<Pedido> {
    public void Configure(EntityTypeBuilder<Pedido> builder) {
        builder.ToTable("Pedido");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn(1001, 10);

        builder.Property(x => x.Id).HasComment("Identificador único do pedido.");
        builder.Property(x => x.MesaId).HasComment("Mesa vinculada ao pedido.");
        builder.Property(x => x.ClienteId).HasComment("Cliente vinculado ao pedido.");
        builder.Property(x => x.FuncionarioId).HasComment("Funcionário responsável pelo pedido.");
        builder.Property(x => x.Status).HasComment("Status atual do pedido.");
        builder.Property(x => x.TipoPedido).HasComment("Tipo do pedido realizado.");
        builder.Property(x => x.Subtotal).HasPrecision(18, 2).HasComment("Valor subtotal dos itens do pedido.");
        builder.Property(x => x.Desconto).HasPrecision(18, 2).HasComment("Valor de desconto aplicado.");
        builder.Property(x => x.TaxaServico).HasPrecision(18, 2).HasComment("Taxa de serviço aplicada.");
        builder.Property(x => x.TaxaEntrega).HasPrecision(18, 2).HasComment("Taxa de entrega aplicada.");
        builder.Property(x => x.Total).HasPrecision(18, 2).HasComment("Valor total do pedido.");
        builder.Property(x => x.DataPedidoHora).HasComment("Data e hora da criação do pedido.");
        builder.Property(x => x.Observacao).HasMaxLength(500).HasComment("Observações do pedido.");
        builder.HasMany(x => x.Itens).WithOne(x => x.Pedido).HasForeignKey(x => x.PedidoId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.Pagamentos).WithOne(x => x.Pedido).HasForeignKey(x => x.PedidoId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Entrega).WithOne(x => x.Pedido).HasForeignKey<Entrega>(x => x.PedidoId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Mesa).WithMany(x => x.Pedidos).HasForeignKey(x => x.MesaId).OnDelete(DeleteBehavior.Restrict);
    }
}