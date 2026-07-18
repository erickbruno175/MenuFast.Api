using MenuFast.Api.Api.Domain.Entities.Models.Pedido;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations.PedidoConfig;

public class EntregadorConfig : IEntityTypeConfiguration<Entregador> {
    public void Configure(EntityTypeBuilder<Entregador> builder) {
        builder.ToTable("Entregador");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasComment("Identificador único do entregador.");
        builder.Property(x => x.Nome).IsRequired().HasMaxLength(150).HasComment("Nome do entregador.");
        builder.Property(x => x.Telefone).HasMaxLength(20).HasComment("Telefone do entregador.");
        builder.Property(x => x.MarcaMoto).HasMaxLength(50).HasComment("Marca da motocicleta.");
        builder.Property(x => x.Modelo).HasMaxLength(50).HasComment("Modelo da motocicleta.");
        builder.Property(x => x.Cor).HasMaxLength(50).HasComment("Cor da motocicleta.");
        builder.Property(x => x.Ano).HasComment("Ano da motocicleta.");
        builder.Property(x => x.Placa).HasMaxLength(10).HasComment("Placa da motocicleta.");
        builder.HasMany(x => x.Entregas)
            .WithOne(x => x.Entregador)
            .HasForeignKey(x => x.MotoboyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}