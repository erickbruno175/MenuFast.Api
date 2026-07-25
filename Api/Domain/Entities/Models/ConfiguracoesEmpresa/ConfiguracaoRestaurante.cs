

namespace MenuFast.Api.Api.Domain.Entities.Models.ConfiguracoesEmpresa;

public class ConfiguracaoRestaurante {
    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public bool TrabalhaComMesa { get; set; }
    public bool TrabalhaComComanda { get; set; }
    public bool TrabalhaComDelivery { get; set; }
    public bool TrabalhaComRetirada { get; set; }
    public bool ControlaEstoque { get; set; }
    public bool PermiteVendaSemEstoque { get; set; }
    public bool CobraTaxaServico { get; set; }
    public decimal PercentualTaxaServico { get; set; }
    public bool ExigirGarcomNaMesa { get; set; }
    public bool ImprimirPedidoAutomaticamente { get; set; }
    public bool EnviarPedidoAutomaticamenteCozinha { get; set; }
    public bool ExigirCaixaAberto { get; set; }
    public bool ImprimirComprovanteFechamento { get; set; }
    public bool IdentificarClienteObrigatorio { get; set; }
    public bool Ativo { get; set; }
 
}
