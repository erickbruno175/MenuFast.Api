namespace MenuFast.Api.Api.Domain.Entities.Models.Empresa;

public class ContaBancaria {
    public Guid Id { get; set; }
    public Guid EmpresaId { get; set; }
    public string Banco { get; set; } = string.Empty;
    public string Agencia { get; set; } = string.Empty;
    public string Conta { get; set; } = string.Empty;
    public string Digito { get; set; } = string.Empty;
    public string Titular { get; set; } = string.Empty;
    public string DocumentoTitular { get; set; } = string.Empty;
    public ICollection<ChavePix> ChavesPix { get; set; } = new List<ChavePix>();
    public Empresa? Empresa { get; set; }
}