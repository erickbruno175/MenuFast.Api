using MenuFast.Api.Api.Domain.Entities.Models.Loja;
using MenuFast.Api.Api.Domain.Entities.Models.Loja;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations.EmpresaConfig;

public class LojaConfig : IEntityTypeConfiguration<Loja> {
    public void Configure(EntityTypeBuilder<Loja> builder) {
        builder.ToTable("Loja");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn(100, 1);
        builder.Property(x => x.Id).HasComment("Identificador único da empresa.");
        builder.Property(x => x.Slug).HasMaxLength(100).IsRequired().HasComment("Slug único utilizado para identificar a empresa na URL.");
        builder.HasIndex(x => x.Slug).IsUnique();
        builder.Property(x => x.RazaoSocial).HasMaxLength(200).IsRequired().HasComment("Razão social da empresa.");
        builder.Property(x => x.NomeFantasia).HasMaxLength(200).IsRequired().HasComment("Nome fantasia da empresa.");
        builder.Property(x => x.Cnpj).HasMaxLength(18).IsRequired().HasComment("CNPJ da empresa.");
        builder.Property(x => x.InscricaoEstadual).HasMaxLength(30).HasComment("Inscrição estadual da empresa.");
        builder.Property(x => x.Telefone).HasMaxLength(20).IsRequired().HasComment("Telefone principal da empresa.");
        builder.Property(x => x.Email).HasMaxLength(150).IsRequired().HasComment("E-mail principal da empresa.");
        builder.Property(x => x.Cep).HasMaxLength(9).HasComment("CEP do endereço da empresa.");
        builder.Property(x => x.Bairro).HasMaxLength(100).HasComment("Bairro da empresa.");
        builder.Property(x => x.Cidade).HasMaxLength(100).HasComment("Cidade da empresa.");
        builder.Property(x => x.Estado).HasMaxLength(100).HasComment("Estado da empresa.");
        builder.Property(x => x.Uf).HasMaxLength(2).HasComment("Sigla da unidade federativa.");
        builder.Property(x => x.Logo).HasMaxLength(500).HasComment("Caminho ou URL da logomarca da empresa.");
        builder.Property(x => x.Ativo).HasComment("Indica se a empresa está ativa no sistema.");
        builder.Property(x => x.Logradouro).HasMaxLength(200).HasComment("Logradouro da empresa.");
        builder.Property(x => x.Numero).HasMaxLength(20).HasComment("Número do endereço.");
        builder.Property(x => x.Complemento).HasMaxLength(100).HasComment("Complemento do endereço.");
        builder.Property(x => x.DataCadastro).HasComment("Data de cadastro da empresa.");
        builder.Property(x => x.Facebook).HasComment("URL do perfil ou página da empresa no Facebook.");
        builder.Property(x => x.Instagram).HasComment("URL do perfil da empresa no Instagram.");
        builder.Property(x => x.WhatsApp).HasComment("Número ou link do WhatsApp da empresa.");
        builder.Property(x => x.Site).HasComment("Site oficial da empresa.");
        builder.Property(x => x.Latitude).HasComment("Latitude da localização da empresa."); 
        builder.Property(x => x.Longitude).HasComment("Longitude da localização da empresa.");
        builder.Property(x => x.ConfiguracaoFinalizada).HasComment("Verifica se todas as configurações iniciai foram cadastradas");
       
    }
}