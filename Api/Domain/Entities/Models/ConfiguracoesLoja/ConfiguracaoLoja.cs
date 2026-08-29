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
    // ENTREGA
    public bool CobraTaxaEntrega { get; set; } = false;

    public TipoTaxaEntrega TipoTaxaEntrega { get; set; }

    // Usado quando TipoTaxaEntrega = Fixa
    public decimal? TaxaEntrega { get; set; }

    // Usado quando TipoTaxaEntrega = PorDistancia
    public decimal? TaxaBaseEntrega { get; set; }
    public decimal? ValorPorKm { get; set; }
    public decimal? DistanciaMaximaEntregaKm { get; set; }
    public bool ExigirGarcomNaMesa { get; set; } = false;

    public bool EnviarPedidoAutomaticamenteCozinha { get; set; } = false;

    public bool EnviarPedidoAutomaticamenteBar { get; set; } = false;

    public bool Ativo { get; set; } = false;
    public bool AbilitarImpressoraTermica { get; set; } = false;

    public bool AbilitarKDS { get; set; } = false;

    public Loja.Loja Loja { get; set; }
}