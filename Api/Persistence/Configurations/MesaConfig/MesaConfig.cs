using MenuFast.Api.Api.Domain.Entities.Models.Mesa;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations.MesaConfig;

public class MesaConfig : IEntityTypeConfiguration<Mesa> {
    public void Configure(EntityTypeBuilder<Mesa> builder) {
        builder.ToTable("Mesa");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn(7001, 1);
        builder.Property(x => x.Id).HasComment("Identificador único da mesa.");
        builder.Property(x => x.Numero).IsRequired().HasMaxLength(20).HasComment("Número ou identificação da mesa.");
        builder.Property(x => x.ImagemUrl).HasMaxLength(500).HasComment("URL da imagem da mesa.");
        builder.HasMany(x => x.Pedidos).WithOne(x => x.Mesa).HasForeignKey(x => x.MesaId).OnDelete(DeleteBehavior.Restrict);
    }
}