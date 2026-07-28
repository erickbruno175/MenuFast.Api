using MenuFast.Api.Api.Domain.Entities.Models.Seguranca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations.HistoricoConfig {
    public class HistoricoAcessoConfig : IEntityTypeConfiguration<HistoricoAcesso> {
        public void Configure(EntityTypeBuilder<HistoricoAcesso> builder) {
            builder.ToTable("HistoricoAcessos");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Ip).HasMaxLength(50);
            builder.Property(x => x.Dispositivo).HasMaxLength(200);
            builder.Property(x => x.Token).HasMaxLength(500);
            builder.Property(x => x.DataLogin).IsRequired();
            builder.Property(x => x.SessaoAtiva).IsRequired();
            builder.Property(x=> x.TipoAcesso).IsRequired();
            builder.HasOne(x => x.Funcionario).WithMany().HasForeignKey(x => x.FuncionarioId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}