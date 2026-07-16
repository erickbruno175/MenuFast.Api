using MenuFast.Api.Api.Domain.Entities.Models.Empresa;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations.EmpresaConfig;

public class ChavePixConfig : IEntityTypeConfiguration<ChavePix> {
    public void Configure(EntityTypeBuilder<ChavePix> builder) {
        builder.ToTable("ChavePix");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasComment("Identificador único da chave Pix.");
        builder.Property(x => x.ContaBancariaId).IsRequired().HasComment("Identificador da conta bancária vinculada à chave Pix.");
        builder.Property(x => x.Tipo).IsRequired().HasComment("Tipo da chave Pix: CPF, CNPJ, e-mail, telefone ou chave aleatória.");
        builder.Property(x => x.Chave).HasMaxLength(150).IsRequired().HasComment("Valor da chave Pix cadastrada.");
        builder.Property(x => x.Principal).HasDefaultValue(false).HasComment("Indica se esta é a chave Pix principal da conta bancária.");
        builder.HasOne(x => x.ContaBancaria).WithMany(x => x.ChavesPix).HasForeignKey(x => x.ContaBancariaId).OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(x => new { x.ContaBancariaId, x.Chave }).IsUnique();
    }
}