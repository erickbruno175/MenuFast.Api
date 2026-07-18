using MenuFast.Api.Api.Domain.Entities.Models.Cozinha;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations.CozinhaConfig;

public class OrdemProducaoConfig : IEntityTypeConfiguration<OrdemProducao> {
    public void Configure(EntityTypeBuilder<OrdemProducao> builder) {
        builder.ToTable("OrdemProducao");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasComment("Identificador único da ordem de produção.");
        builder.Property(x => x.PedidoId).HasComment("Pedido vinculado à ordem de produção.");
        builder.Property(x => x.FuncionarioId).HasComment("Funcionário responsável pela produção.");
        builder.Property(x => x.Status).HasComment("Status atual da ordem de produção.");
        builder.Property(x => x.Prioridade).HasComment("Prioridade da ordem de produção.");
        builder.Property(x => x.DataEntrada).HasComment("Data e hora de entrada da ordem na produção.");
        builder.Property(x => x.InicioPreparo).HasComment("Data e hora de início do preparo.");
        builder.Property(x => x.FimPreparo).HasComment("Data e hora de término do preparo.");
        builder.Property(x => x.Observacao).HasMaxLength(500).HasComment("Observações da ordem de produção.");
    }
}