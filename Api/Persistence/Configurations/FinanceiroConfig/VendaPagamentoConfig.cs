using MenuFast.Api.Api.Domain.Entities.Models.Financeiro;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations.FinanceiroConfig {
    public class VendaPagamentoConfig : IEntityTypeConfiguration<PagamentoVenda> {
        public void Configure(EntityTypeBuilder<PagamentoVenda> builder) {
            builder.ToTable("PagamentosVenda");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Valor).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(x => x.Troco).HasColumnType("decimal(18,2)").HasDefaultValue(0);
            builder.HasIndex(x => x.VendaId);
            builder.HasIndex(x => x.FormaPagamentoId);
            builder.HasOne(x => x.Venda).WithMany(x => x.Pagamentos).HasForeignKey(x => x.VendaId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}