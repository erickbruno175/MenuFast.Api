using MenuFast.Api.Api.Domain.Entities.Models.Financeiro;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations.FinanceiroConfig;

public class ContaPagarConfig : IEntityTypeConfiguration<ContaPagar> {
    public void Configure(EntityTypeBuilder<ContaPagar> builder) {
        builder.ToTable("ContaPagar");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn(13001, 1);

        builder.Property(x => x.Id).HasComment("Identificador único da conta a pagar.");
        builder.Property(x => x.LojaId).HasComment("Loja responsável pela conta a pagar.");
        builder.Property(x => x.Descricao).IsRequired().HasMaxLength(200).HasComment("Descrição da conta a pagar.");
        builder.Property(x => x.Valor).HasPrecision(18, 2).HasComment("Valor da conta a pagar.");
        builder.Property(x => x.DataVencimento).HasComment("Data de vencimento da conta.");
        builder.Property(x => x.DataPagamento).HasComment("Data de pagamento da conta.");
        builder.Property(x => x.Pago).HasComment("Indica se a conta foi paga.");
        builder.Property(x => x.Status).HasComment("Status atual da conta financeira.");

    }
}