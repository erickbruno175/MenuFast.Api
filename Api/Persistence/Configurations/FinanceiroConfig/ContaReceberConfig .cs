using MenuFast.Api.Api.Domain.Entities.Models.Financeiro;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations.FinanceiroConfig;

public class ContaReceberConfig : IEntityTypeConfiguration<ContaReceber> {
    public void Configure(EntityTypeBuilder<ContaReceber> builder) {
        builder.ToTable("ContaReceber");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasComment("Identificador único da conta a receber.");
        builder.Property(x => x.EmpresaId).HasComment("Empresa responsável pela conta a receber.");
        builder.Property(x => x.Descricao).IsRequired().HasMaxLength(200).HasComment("Descrição da conta a receber.");
        builder.Property(x => x.Valor).HasPrecision(18, 2).HasComment("Valor da conta a receber.");
        builder.Property(x => x.DataVencimento).HasComment("Data de vencimento da conta.");
        builder.Property(x => x.DataRecebimento).HasComment("Data de recebimento da conta.");
        builder.Property(x => x.Recebido).HasComment("Indica se a conta foi recebida.");
        builder.Property(x => x.Status).HasComment("Status atual da conta financeira.");
    }
}