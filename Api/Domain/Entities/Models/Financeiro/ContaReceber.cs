using MenuFast.Api.Api.Domain.Enum;

namespace MenuFast.Api.Api.Domain.Entities.Models.Financeiro;

public class ContaReceber {
    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public DateTime DataVencimento { get; set; }
    public DateTime? DataRecebimento { get; set; }
    public bool Recebido { get; set; }
    public StatusContaFinanceira Status { get; set; }

}