using MenuFast.Api.Api.Domain.Entities.Models.Financeiro;
using MenuFast.Api.Api.Domain.Entities.Models.Funcionario;

using MenuFast.Api.Api.Domain.Enum;

namespace MenuFast.Api.Api.Domain.Entities.Models.Financeiro;

public class MovimentoCaixa {
    public Guid Id { get; set; }
    public Guid CaixaId { get; set; }
    public Caixa Caixa { get; set; } = null!;
    public Guid FuncionarioId { get; set; }
    public TipoMovimentoCaixa Tipo { get; set; }
    public decimal Valor { get; set; }
    public string? Descricao { get; set; }
    public DateTime Data { get; set; }
    public StatusContaFinanceira Status { get; set; }
    public Funcionario.Funcionario Funcionario { get; set; }

}