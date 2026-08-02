using MenuFast.Api.Api.Domain.Entities.Models.Financeiro;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations.FinanceiroConfig;

public class MovimentoCaixaConfig : IEntityTypeConfiguration<MovimentoCaixa> {
    public void Configure(EntityTypeBuilder<MovimentoCaixa> builder) {
        builder.ToTable("MovimentoCaixa");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .UseIdentityColumn(11001, 1);

        builder.Property(x => x.Id)
            .HasComment("Identificador único do movimento de caixa.");

        builder.Property(x => x.LojaId)
            .HasComment("Loja vinculada ao movimento de caixa.");

        builder.Property(x => x.CaixaId)
            .HasComment("Caixa vinculado ao movimento.");

        builder.Property(x => x.FuncionarioId)
            .HasComment("Funcionário responsável pelo movimento.");

        builder.Property(x => x.Tipo)
            .HasComment("Tipo do movimento realizado no caixa.");

        builder.Property(x => x.Valor)
            .HasPrecision(18, 2)
            .HasComment("Valor do movimento.");

        builder.Property(x => x.Descricao)
            .HasMaxLength(300)
            .HasComment("Descrição do movimento.");

        builder.Property(x => x.Data)
            .HasComment("Data e hora do movimento.");

        // Loja → MovimentoCaixa
        builder.HasOne(x => x.Loja)
            .WithMany()
            .HasForeignKey(x => x.LojaId)
            .OnDelete(DeleteBehavior.NoAction);

        // Caixa → MovimentoCaixa
        builder.HasOne(x => x.Caixa)
            .WithMany(x => x.Movimentos)
            .HasForeignKey(x => x.CaixaId)
            .OnDelete(DeleteBehavior.NoAction);

        // Funcionario → MovimentoCaixa
        builder.HasOne(x => x.Funcionario)
            .WithMany()
            .HasForeignKey(x => x.FuncionarioId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}