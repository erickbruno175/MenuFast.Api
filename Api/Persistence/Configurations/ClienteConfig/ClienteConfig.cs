using MenuFast.Api.Api.Domain.Entities.Models.Cliente;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations.ClienteConfig;

public class ClienteConfig : IEntityTypeConfiguration<Cliente> {
    public void Configure(EntityTypeBuilder<Cliente> builder) {
        builder.ToTable("Cliente");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn(1001, 1);

        builder.Property(x => x.Id).HasComment("Identificador único do cliente.");
        builder.Property(x => x.EmpresaId).HasComment("Empresa à qual o cliente pertence.");
        builder.Property(x => x.Nome).IsRequired().HasMaxLength(150).HasComment("Nome completo do cliente.");
        builder.Property(x => x.CPF).IsRequired().HasMaxLength(14).HasComment("CPF do cliente.");
        builder.Property(x => x.DataNascimento).HasComment("Data de nascimento do cliente.");
        builder.Property(x => x.Telefone).HasMaxLength(20).HasComment("Telefone do cliente.");
        builder.Property(x => x.WhatsApp).HasMaxLength(20).HasComment("WhatsApp do cliente.");
        builder.Property(x => x.Email).HasMaxLength(150).HasComment("E-mail do cliente.");
        builder.Property(x => x.CEP).HasMaxLength(9).HasComment("CEP do endereço.");
        builder.Property(x => x.Logradouro).HasMaxLength(200).HasComment("Logradouro do endereço.");
        builder.Property(x => x.Numero).HasMaxLength(20).HasComment("Número do endereço.");
        builder.Property(x => x.Complemento).HasMaxLength(100).HasComment("Complemento do endereço.");
        builder.Property(x => x.Bairro).HasMaxLength(100).HasComment("Bairro do endereço.");
        builder.Property(x => x.Cidade).HasMaxLength(100).HasComment("Cidade do endereço.");
        builder.Property(x => x.Estado).HasMaxLength(2).HasComment("UF do endereço.");
        builder.Property(x => x.PontoReferencia).HasMaxLength(200).HasComment("Ponto de referência do endereço.");
        builder.Property(x => x.Observacao).HasMaxLength(500).HasComment("Observações do cliente.");
        builder.Property(x => x.Ativo).HasComment("Indica se o cliente está ativo.");
        builder.Property(x => x.DataCadastro).HasComment("Data de cadastro do cliente.");
        builder.Property(x => x.Latitude).HasPrecision(9, 6).HasComment("Latitude do endereço do cliente.");
        builder.Property(x => x.Longitude).HasPrecision(9, 6).HasComment("Longitude do endereço do cliente.");
    }
}