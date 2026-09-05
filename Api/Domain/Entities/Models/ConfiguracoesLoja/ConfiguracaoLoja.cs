using MenuFast.Api.Api.Domain.Entities.Models.Loja;
using MenuFast.Api.Api.Domain.Enum;

namespace MenuFast.Api.Api.Domain.Entities.Models.ConfiguracoesLoja;

public class ConfiguracaoLoja {
    public int Id { get; set; }

    public int LojaId { get; set; }
    public bool TrabalhaComMesa { get; set; } = false;
    public bool TrabalhaComDelivery { get; set; } = false;
    public bool TrabalhaComRetirada { get; set; } = false;
    public bool PermiteVendaSemEstoque { get; set; } = false;
    public bool CobraTaxaServico { get; set; } = false;
    public decimal? PercentualTaxaServico { get; set; }
    public bool CobraTaxaEntrega { get; set; } = false;
    public TipoTaxaEntrega TipoTaxaEntrega { get; set; }
    public decimal? TaxaEntrega { get; set; }
    public decimal? TaxaBaseEntrega { get; set; }
    public decimal? ValorPorKm { get; set; }
    public decimal? DistanciaMaximaEntregaKm { get; set; }
    public bool Ativo { get; set; } = false;
    public bool AbilitarImpressoraTermica { get; set; } = false;
    public bool AbilitarKDS { get; set; } = false;
    public Loja.Loja Loja { get; set; }
    public int TaxaEntregaMinima { get; internal set; }
    public decimal? ValorAberturaCaixa { get; set; } = 0;
}