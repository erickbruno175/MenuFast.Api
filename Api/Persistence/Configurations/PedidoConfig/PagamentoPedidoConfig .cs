using MenuFast.Api.Api.Domain.Entities.Models.Pedido;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations.PedidoConfig;

public class PagamentoPedidoConfig : IEntityTypeConfiguration<PagamentoPedido> {
    public void Configure(EntityTypeBuilder<PagamentoPedido> builder) {
        builder.ToTable("PagamentoPedido");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn(2001, 1);

        builder.Property(x => x.Id).HasComment("Identificador único do pagamento do pedido.");
        builder.Property(x => x.PedidoId).HasComment("Pedido vinculado ao pagamento.");
        builder.Property(x => x.FormaPagamentoId).HasComment("Forma de pagamento utilizada.");
        builder.Property(x => x.Valor).HasPrecision(18, 2).HasComment("Valor pago.");
        builder.Property(x => x.DataPagamento).HasComment("Data e hora do pagamento.");

        builder.HasOne(x => x.Pedido)
            .WithMany(x => x.Pagamentos)
            .HasForeignKey(x => x.PedidoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.FormaPagamento)
            .WithMany()
            .HasForeignKey(x => x.FormaPagamentoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}