using MenuFast.Api.Api.Domain.Entities.Models.Funcionario;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MenuFast.Api.Api.Persistence.Configurations;

public class FuncionarioConfig : IEntityTypeConfiguration<Funcionario> {
    public void Configure(EntityTypeBuilder<Funcionario> builder) {
        builder.ToTable("Funcionario");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Nome).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Cpf).IsRequired().HasMaxLength(11);
        builder.HasIndex(x => x.Cpf).IsUnique();
        builder.Property(x => x.Email).IsRequired().HasMaxLength(150);
        builder.HasIndex(x => x.Email).IsUnique();
        builder.Property(x => x.Telefone).HasMaxLength(20);
        builder.Property(x => x.Login).IsRequired().HasMaxLength(50);
        builder.HasIndex(x => x.Login).IsUnique();
        builder.Property(x => x.SenhaHash).IsRequired().HasMaxLength(500);
        builder.Property(x => x.PrimeiroAcesso).IsRequired();
        builder.Property(x => x.Ativo).IsRequired();
        builder.Property(x => x.DataAdmissao).IsRequired();
        builder.Property(x => x.Salario).HasColumnType("decimal(18,2)");
        builder.Property(x => x.DataCadastro).IsRequired();
        builder.Property(x => x.UltimoLogin);
        builder.HasOne(x => x.Perfil).WithMany().HasForeignKey(x => x.PerfilId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Funcao).WithMany(x => x.Funcionarios).HasForeignKey(x => x.FuncaoId).OnDelete(DeleteBehavior.Restrict);
    }
}