using MenuFast.Api.Api.Domain.Entities.Models.Pedido;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations.PedidoConfig;

public class EntregaConfig : IEntityTypeConfiguration<Entrega> {
    public void Configure(EntityTypeBuilder<Entrega> builder) {
        builder.ToTable("Entrega");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasComment("Identificador único da entrega.");
        builder.Property(x => x.PedidoId).HasComment("Pedido vinculado à entrega.");
        builder.Property(x => x.ClienteEnderecoId).HasComment("Endereço do cliente para entrega.");
        builder.Property(x => x.MotoboyId).HasComment("Entregador responsável pela entrega.");
        builder.Property(x => x.Status).HasComment("Status atual da entrega.");
        builder.Property(x => x.TaxaEntrega).HasPrecision(18, 2).HasComment("Valor da taxa de entrega.");
        builder.Property(x => x.DataSaida).HasComment("Data e hora de saída para entrega.");
        builder.Property(x => x.DataEntrega).HasComment("Data e hora da entrega realizada.");
        builder.HasOne(x => x.Pedido).WithOne(x => x.Entrega).HasForeignKey<Entrega>(x => x.PedidoId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Entregador).WithMany(x => x.Entregas).HasForeignKey(x => x.MotoboyId).OnDelete(DeleteBehavior.Restrict);
    }
}