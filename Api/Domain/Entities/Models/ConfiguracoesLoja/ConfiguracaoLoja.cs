using MenuFast.Api.Api.Domain.Entities.Models.Loja;

namespace MenuFast.Api.Api.Domain.Entities.Models.ConfiguracoesLoja;

public class ConfiguracaoLoja {
    public int Id { get; set; }
    public int LojaId { get; set; }
    public bool TrabalhaComMesa { get; set; } = false;
    public bool TrabalhaComDelivery { get; set; } = false;
    public bool TrabalhaComRetirada { get; set; } = false;
    public bool PermiteVendaSemEstoque { get; set; } = false;
    public bool CobraTaxaServico { get; set; } = false;
    public decimal PercentualTaxaServico { get; set; }
    public bool ExigirGarcomNaMesa { get; set; } = false;
    public bool EnviarPedidoAutomaticamenteCozinha { get; set; } = false;
    public bool EnviarPedidoAutomaticamenteBar { get; set; } = false;
    public bool Ativo { get; set; } = false;
    public bool AbilitarImpressoraTermica {  get; set; } = false;
    public bool AbilitarKDS {  get; set; } = false;
    public Loja.Loja Loja { get; set; }
}