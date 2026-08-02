using MenuFast.Api.Api.Domain.Entities.Models.Loja;

namespace MenuFast.Api.Api.Domain.Entities.Models.ConfiguracoesLoja;

public class FormaPagamento {
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int LojaId { get; set; }
    public Loja.Loja Loja { get; set; }
    public bool PermiteTroco { get; set; }
    public bool Ativo { get; set; }
    public string? Foto { get; set; }
    public int? ProvedorPagamentoId { get; set; }
    public ProvedorPagamento? ProvedorPagamento { get; set; }
}