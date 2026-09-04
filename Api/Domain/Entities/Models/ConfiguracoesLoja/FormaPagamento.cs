using MenuFast.Api.Api.Domain.Entities.Models.Loja;

namespace MenuFast.Api.Api.Domain.Entities.Models.ConfiguracoesLoja;

public class FormaPagamento {
    public int Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public int LojaId { get; set; }
    public Loja.Loja Loja { get; set; }
    public bool PermiteTroco { get; set; }
    public bool Ativo { get; set; }
}