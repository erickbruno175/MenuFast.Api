namespace MenuFast.Api.Api.Domain.Entities.Models.Financeiro;

public class ContaPagar {
    public Guid Id { get; set; }
    public Guid EmpresaId { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public DateTime DataVencimento { get; set; }
    public DateTime? DataPagamento { get; set; }
    public bool Pago { get; set; }
}