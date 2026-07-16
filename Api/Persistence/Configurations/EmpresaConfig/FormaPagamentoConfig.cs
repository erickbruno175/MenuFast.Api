using MenuFast.Api.Api.Domain.Entities.Models.ConfiguracoesEmpresa;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations.ConfiguracoesEmpresa;

public class FormaPagamentoConfig : IEntityTypeConfiguration<FormaPagamento> {
    public void Configure(EntityTypeBuilder<FormaPagamento> builder) {
        builder.ToTable("FormaPagamento");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasComment("Identificador único da forma de pagamento.");
        builder.Property(x => x.Nome).HasMaxLength(100).IsRequired().HasComment("Nome da forma de pagamento.");
        builder.Property(x => x.PermiteTroco).HasComment("Indica se a forma de pagamento permite troco.");
        builder.Property(x => x.Ativo).HasComment("Indica se a forma de pagamento está ativa.");
        builder.Property(x => x.Foto).HasMaxLength(500).HasComment("Imagem ou ícone da forma de pagamento.");
        builder.Property(x => x.ProvedorPagamentoId).HasComment("Provedor de pagamento vinculado à forma de pagamento.");
        builder.HasOne(x => x.ProvedorPagamento).WithMany().HasForeignKey(x => x.ProvedorPagamentoId).OnDelete(DeleteBehavior.Restrict);
    }
}