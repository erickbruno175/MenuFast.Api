using MenuFast.Api.Api.Domain.Entities.Models.Usuario;
using MenuFast.Api.Api.Domain.Enum;

namespace MenuFast.Api.Api.Domain.Entities.Models.Financeiro;

public class MovimentoCaixa {
    public Guid Id { get; set; }
    public Guid CaixaId { get; set; }
    public int FuncioanrioId { get; set; }
    public Funcionario Funcionario { get; set; }
    public TipoMovimentoCaixa Tipo { get; set; }
    public decimal Valor { get; set; }
    public string? Descricao { get; set; }
    public DateTime Data { get; set; }
    public Caixa Caixa { get; set; } = null!;
}