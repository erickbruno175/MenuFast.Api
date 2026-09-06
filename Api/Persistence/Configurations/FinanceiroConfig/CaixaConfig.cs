using MenuFast.Api.Api.Domain.Entities.Models.Financeiro;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations.FinanceiroConfig;

public class CaixaConfig : IEntityTypeConfiguration<Caixa> {
    public void Configure(EntityTypeBuilder<Caixa> builder) {
        builder.ToTable("Caixa");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn(100, 1);
        builder.Property(x => x.Id).HasComment("Identificador único do caixa.");
        builder.Property(x => x.Nome).IsRequired().HasMaxLength(100).HasComment("Nome do caixa.");
        builder.Property(x => x.Aberto).HasComment("Indica se o caixa está aberto.");
        builder.Property(x => x.ValorAbertura).HasPrecision(18, 2).HasComment("Valor informado na abertura do caixa.");
        builder.Property(x => x.ValorFechamento).HasPrecision(18, 2).HasComment("Valor informado no fechamento do caixa.");
        builder.Property(x => x.DataAbertura).HasComment("Data e hora de abertura do caixa.");
        builder.Property(x => x.DataFechamento).HasComment("Data e hora de fechamento do caixa.");
        builder.Property(x => x.FuncioanrioId).HasComment("Funcionário responsável pelo caixa.");
        builder.Property(x=> x.Terminal).HasMaxLength(50).HasComment("Nome do terminal onde o caixa foi aberto.");
        builder.Property(x => x.IpTerminal).HasMaxLength(500).HasComment("Endereço IP do terminal onde o caixa foi aberto.");
        builder.HasMany(x => x.Movimentos).WithOne(x => x.Caixa).HasForeignKey(x => x.CaixaId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Funcionario).WithMany().HasForeignKey(x => x.FuncioanrioId).OnDelete(DeleteBehavior.Restrict);
    }
}