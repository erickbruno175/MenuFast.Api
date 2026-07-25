using MenuFast.Api.Api.Domain.Entities.Models.ConfiguracoesEmpresa;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations.ConfiguracoesEmpresa;

public class ConfiguracaoProvedorPagamentoConfig : IEntityTypeConfiguration<ConfiguracaoProvedorPagamento> {
    public void Configure(EntityTypeBuilder<ConfiguracaoProvedorPagamento> builder) {
        builder.ToTable("ConfiguracaoProvedorPagamento");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn(1001, 1);

        builder.Property(x => x.Id).HasComment("Identificador único da configuração do provedor de pagamento.");
        builder.Property(x => x.EmpresaId).HasComment("Empresa vinculada ao provedor de pagamento.");
        builder.Property(x => x.ProvedorPagamentoId).HasComment("Provedor de pagamento utilizado pela empresa.");
        builder.Property(x => x.ChaveApi).HasMaxLength(500).HasComment("Chave de acesso da API do provedor de pagamento.");
        builder.Property(x => x.Token).HasMaxLength(500).HasComment("Token de autenticação do provedor de pagamento.");
        builder.Property(x => x.SecretKey).HasMaxLength(500).HasComment("Chave secreta do provedor de pagamento.");
        builder.Property(x => x.Ativo).HasComment("Indica se a configuração do provedor está ativa.");
        builder.HasOne(x => x.ProvedorPagamento).WithMany().HasForeignKey(x => x.ProvedorPagamentoId).OnDelete(DeleteBehavior.Restrict);
    }
}