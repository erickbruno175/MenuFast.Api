using MenuFast.Api.Api.Domain.Enum;

namespace MenuFast.Api.Api.Application.DTOs.Response;

public class ConfiguracoesLojaResponse {
    public int Id { get; set; }
    public bool TrabalhaComMesa { get; set; }
    public bool TrabalhaComDelivery { get; set; }
    public bool TrabalhaComRetirada { get; set; }
    public bool PermiteVendaSemEstoque { get; set; }
    public bool CobraTaxaServico { get; set; }
    public decimal PercentualTaxaServico { get; set; }
    public bool CobraTaxaEntrega { get; set; }
    public TipoTaxaEntrega TipoTaxaEntrega { get; set; }
    public decimal? TaxaEntrega { get; set; }
    public decimal? TaxaBaseEntrega { get; set; }
    public decimal? ValorPorKm { get; set; }
    public decimal? DistanciaMaximaEntregaKm { get; set; }
    public decimal? ValorAberturaCaixa { get; set; }
    public bool AbilitarImpressoraTermica { get; set; }
    public bool AbilitarKDS { get; set; }
    public bool Ativo { get; set; }
    public string RazaoSocial { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class HorarioFuncionamentoResponse {
    public int Id { get; set; }
    public int LojaId { get; set; }
    public DayOfWeek DiaSemana { get; set; }
    public TimeSpan HoraAbertura { get; set; }
    public TimeSpan HoraFechamento { get; set; }
    public bool Fechado { get; set; }
}

public class FormaPagamentoResponse {
    public int Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
}
