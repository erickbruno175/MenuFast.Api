using MenuFast.Api.Api.Domain.Entities.Models.ConfiguracoesEmpresa;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations.ConfiguracoesEmpresa;

public class ProvedorPagamentoConfig : IEntityTypeConfiguration<ProvedorPagamento> {
    public void Configure(EntityTypeBuilder<ProvedorPagamento> builder) {
        builder.ToTable("ProvedorPagamento");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasComment("Identificador único do provedor de pagamento.");
        builder.Property(x => x.Nome).HasMaxLength(100).IsRequired().HasComment("Nome do provedor de pagamento.");
        builder.Property(x => x.Codigo).HasMaxLength(50).IsRequired().HasComment("Código interno do provedor de pagamento.");
        builder.Property(x => x.Ativo).HasComment("Indica se o provedor de pagamento está ativo.");
        builder.HasMany(x => x.FormasPagamento).WithOne(x => x.ProvedorPagamento).HasForeignKey(x => x.ProvedorPagamentoId).OnDelete(DeleteBehavior.Restrict);
    }
}