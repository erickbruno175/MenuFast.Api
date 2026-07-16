namespace MenuFast.Api.Api.Domain.Entities.Models.Financeiro;

public class ContaReceber {
    public Guid Id { get; set; }
    public Guid EmpresaId { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public DateTime DataVencimento { get; set; }
    public DateTime? DataRecebimento { get; set; }
    public bool Recebido { get; set; }
}