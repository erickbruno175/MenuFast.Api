using MenuFast.Api.Api.Domain.Entities;
using MenuFast.Api.Api.Domain.Entities.Models.Financeiro;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations.FinanceiroConfig;

public class ComissaoVendaConfig : IEntityTypeConfiguration<ComissaoVenda> {
    public void Configure(EntityTypeBuilder<ComissaoVenda> entity) {
        entity.ToTable("ComissaoVenda");
        entity.HasKey(x => x.Id);
        entity.Property(x => x.ValorVenda).HasPrecision(18, 2);
        entity.Property(x => x.PercentualComissao).HasPrecision(5, 2);
        entity.Property(x => x.ValorComissao).HasPrecision(18, 2);
        entity.HasOne(x => x.Funcionario).WithMany().HasForeignKey(x => x.FuncionarioId).OnDelete(DeleteBehavior.Restrict);
        entity.HasOne(x => x.Pedido).WithMany().HasForeignKey(x => x.PedidoId).OnDelete(DeleteBehavior.Restrict);
    }
}