using MenuFast.Api.Api.Domain.Entities.Models.Financeiro;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations.FinanceiroConfig {
    public class VendaConfig : IEntityTypeConfiguration<Venda> {
        public void Configure(EntityTypeBuilder<Venda> builder) {
            builder.ToTable("Vendas");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ValorBruto).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(x => x.Desconto).HasColumnType("decimal(18,2)").HasDefaultValue(0);
            builder.Property(x => x.Acrescimo).HasColumnType("decimal(18,2)").HasDefaultValue(0);
            builder.Property(x => x.ValorTotal).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(x => x.DataVenda).IsRequired();
            builder.Property(x => x.StatusPagamento).HasConversion<int>().IsRequired();
            builder.HasIndex(x => x.LojaId);
            builder.HasIndex(x => x.DataVenda);
            builder.HasMany(x => x.Pagamentos).WithOne(x => x.Venda).HasForeignKey(x => x.VendaId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Pedidos)
                .WithOne(x => x.Venda)
                .HasForeignKey(x => x.VendaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}