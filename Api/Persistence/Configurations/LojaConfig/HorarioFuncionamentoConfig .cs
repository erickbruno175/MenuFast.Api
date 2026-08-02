using MenuFast.Api.Api.Domain.Entities.Models.ConfiguracoesLoja;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations.EmpresaConfig;
public class HorarioFuncionamentoConfig : IEntityTypeConfiguration<HorarioFuncionamento> {
    public void Configure(EntityTypeBuilder<HorarioFuncionamento> builder) {
        builder.ToTable("HorarioFuncionamento");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn(1001, 1);
        builder.Property(x => x.Id).HasComment("Identificador único do horário de funcionamento.");
        builder.Property(x => x.LojaId).HasComment("Loja vinculada ao horário de funcionamento.");
        builder.Property(x => x.DiaSemana).HasComment("Dia da semana em que o horário é aplicado.");
        builder.Property(x => x.HoraAbertura).HasComment("Horário de abertura do estabelecimento.");
        builder.Property(x => x.HoraFechamento).HasComment("Horário de fechamento do estabelecimento.");
        builder.Property(x => x.Fechado).HasComment("Indica se o estabelecimento não funciona neste dia.");
        builder.HasOne(x => x.Loja).WithMany(x => x.Horarios).HasForeignKey(x => x.LojaId).OnDelete(DeleteBehavior.Cascade);
    }
}