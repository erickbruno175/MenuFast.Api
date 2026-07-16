using MenuFast.Api.Api.Domain.Enum;

namespace MenuFast.Api.Api.Domain.Entities.Models.ConfiguracoesEmpresa;

public class Terminal {
    public Guid Id { get; set; }
    public Guid EmpresaId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Identificacao { get; set; } = string.Empty;
    public TipoTerminal Tipo { get; set; }
    public string? Dispositivo { get; set; }
    public string? SistemaOperacional { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
    public Empresa.Empresa? Empresa { get; set; }
}