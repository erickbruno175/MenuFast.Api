using MenuFast.Api.Api.Domain.Entities.Models.Empresa;
using MenuFast.Api.Api.Domain.Entities.Models.Financeiro;
using MenuFast.Api.Api.Domain.Entities.Models.Funcionario;

using MenuFast.Api.Api.Domain.Enum;

namespace MenuFast.Api.Api.Domain.Entities.Models.Financeiro;

public class MovimentoCaixa {
    public int Id { get; set; }
    public int LojaId { get; set; }
    public Loja Loja { get; set; }
    public int CaixaId { get; set; }
    public Caixa Caixa { get; set; } = null!;
    public int FuncionarioId { get; set; }
    public TipoMovimentoCaixa Tipo { get; set; }
    public decimal Valor { get; set; }
    public string? Descricao { get; set; }
    public DateTime Data { get; set; }
    public StatusContaFinanceira Status { get; set; }
    public Funcionario.Funcionario Funcionario { get; set; }

}