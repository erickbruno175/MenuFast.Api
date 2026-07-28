using MenuFast.Api.Api.Domain.Entities.Models.Empresa;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations.EmpresaConfig;
public class ContaBancariaConfig : IEntityTypeConfiguration<ContaBancaria> {
    public void Configure(EntityTypeBuilder<ContaBancaria> builder) {
        builder.ToTable("ContaBancaria");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn(1001, 1);

        builder.Property(x => x.Id).HasComment("Identificador único da conta bancária.");
        builder.Property(x => x.EmpresaId).HasComment("Empresa proprietária da conta bancária.");
        builder.Property(x => x.Banco).HasMaxLength(100).IsRequired().HasComment("Nome da instituição financeira.");
        builder.Property(x => x.Agencia).HasMaxLength(20).IsRequired().HasComment("Número da agência.");
        builder.Property(x => x.Conta).HasMaxLength(30).IsRequired().HasComment("Número da conta bancária.");
        builder.Property(x => x.Digito).HasMaxLength(5).HasComment("Dígito verificador da conta.");
        builder.Property(x => x.Titular).HasMaxLength(200).IsRequired().HasComment("Nome do titular da conta.");
        builder.Property(x => x.DocumentoTitular).HasMaxLength(20).IsRequired().HasComment("CPF ou CNPJ do titular da conta.");
        builder.HasOne(x => x.Empresa).WithMany(x => x.ContasBancarias).HasForeignKey(x => x.EmpresaId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.ChavesPix).WithOne(x => x.ContaBancaria).HasForeignKey(x => x.ContaBancariaId).OnDelete(DeleteBehavior.Cascade);
    }
}