using MenuFast.Api.Api.Domain.Entities.Models.Pedido;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations.PedidoConfig;

public class HistoricoPedidoConfig : IEntityTypeConfiguration<HistoricoPedido> {
    public void Configure(EntityTypeBuilder<HistoricoPedido> builder) {
        builder.ToTable("HistoricoPedido");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasComment("Identificador único do histórico do pedido.");
        builder.Property(x => x.PedidoId).HasComment("Pedido relacionado ao histórico.");
        builder.Property(x => x.Acao).HasComment("Ação realizada no histórico do pedido.");
        builder.Property(x => x.Observacao).HasMaxLength(500).HasComment("Observação do histórico.");
        builder.Property(x => x.Data).HasComment("Data e hora do registro do histórico.");
        builder.Property(x => x.UsuarioId).HasComment("Usuário responsável pela ação.");
        builder.HasOne(x => x.Pedido).WithMany().HasForeignKey(x => x.PedidoId).OnDelete(DeleteBehavior.Cascade);
    }
}