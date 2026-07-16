using MenuFast.Api.Api.Domain.Entities.Models.Empresa;

namespace MenuFast.Api.Api.Domain.Entities.Models.Usuario;

public class Funcionario {
    public Guid Id { get; set; }
    public Guid EmpresaId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Cargo { get; set; } = string.Empty;
    public decimal Salario { get; set; }
    public DateTime DataAdmissao { get; set; }
    public DateTime? DataDemissao { get; set; }
    public bool Ativo { get; set; } = true;
    public Empresa? Empresa { get; set; }
    public Usuario? Usuario { get; set; }
}