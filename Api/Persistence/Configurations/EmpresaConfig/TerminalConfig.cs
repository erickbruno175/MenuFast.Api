using MenuFast.Api.Api.Domain.Entities.Models;
using MenuFast.Api.Api.Domain.Entities.Models.ConfiguracoesEmpresa;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations.ConfiguracoesEmpresa;

public class TerminalConfig : IEntityTypeConfiguration<Terminal> {
    public void Configure(EntityTypeBuilder<Terminal> builder) {
        builder.ToTable("Terminal");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn(200, 1);

        builder.Property(x => x.Id).HasComment("Identificador único do terminal.");
        builder.Property(x => x.EmpresaId).HasComment("Empresa vinculada ao terminal.");
        builder.Property(x => x.Nome).HasMaxLength(100).IsRequired().HasComment("Nome do terminal.");
        builder.Property(x => x.Identificacao).HasMaxLength(100).IsRequired().HasComment("Identificador único do dispositivo.");
        builder.Property(x => x.Tipo).HasComment("Tipo de utilização do terminal.");
        builder.Property(x => x.Dispositivo).HasMaxLength(200).HasComment("Nome ou modelo do dispositivo.");
        builder.Property(x => x.SistemaOperacional).HasMaxLength(100).HasComment("Sistema operacional utilizado.");
        builder.Property(x => x.Ativo).HasComment("Indica se o terminal está ativo.");
        builder.Property(x => x.DataCadastro).HasComment("Data de cadastro do terminal.");
        builder.HasOne(x => x.Empresa).WithMany(x => x.Terminais).HasForeignKey(x => x.EmpresaId).OnDelete(DeleteBehavior.Cascade);
    }
}