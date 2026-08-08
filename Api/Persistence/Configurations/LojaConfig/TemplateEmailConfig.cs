using MenuFast.Api.Api.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations.EmpresaConfig {
    public class TemplateEmailConfig : IEntityTypeConfiguration<TemplateEmail> {
        public void Configure(EntityTypeBuilder<TemplateEmail> builder) {

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Conteudo);
            builder.Property(x => x.Ativo);
            builder.Property(x => x.Assunto);
            builder.Property(x => x.DataCadastro);
            builder.Property(x => x.DataAlteracao);
        }
    }
}
