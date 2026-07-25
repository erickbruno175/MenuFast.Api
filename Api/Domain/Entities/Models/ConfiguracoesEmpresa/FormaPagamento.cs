namespace MenuFast.Api.Api.Domain.Entities.Models.ConfiguracoesEmpresa;

public class FormaPagamento {
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public bool PermiteTroco { get; set; }
    public bool Ativo { get; set; }
    public string? Foto { get; set; }
    public int? ProvedorPagamentoId { get; set; }
    public ProvedorPagamento? ProvedorPagamento { get; set; }
}