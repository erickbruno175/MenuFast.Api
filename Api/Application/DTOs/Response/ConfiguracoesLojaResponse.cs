using MenuFast.Api.Api.Domain.Entities.Models.ConfiguracoesLoja;
using MenuFast.Api.Api.Domain.Enum;

namespace MenuFast.Api.Api.Application.DTOs.Response {
    public class ConfiguracoesLojaResponse {

    public int Id { get; set; }
        public bool TrabalhaComMesa { get; set; } = false;
        public bool TrabalhaComDelivery { get; set; } = false;
        public bool TrabalhaComRetirada { get; set; } = false;
        public bool PermiteVendaSemEstoque { get; set; } = false;
        public bool CobraTaxaServico { get; set; } = false;
        public decimal PercentualTaxaServico { get; set; }
        public bool CobraTaxaEntrega { get; set; } = false;
        public TipoTaxaEntrega TipoTaxaEntrega { get; set; }
        public decimal? TaxaEntrega { get; set; }
        public decimal? TaxaBaseEntrega { get; set; }
        public decimal? ValorPorKm { get; set; }
        public decimal? DistanciaMaximaEntregaKm { get; set; }
       
        public bool AbilitarImpressoraTermica { get; set; } = false;
        public bool AbilitarKDS { get; set; } = false;
        public bool Ativo { get; set; } = false;
        public string RazaoSocial { get; set; }
        public string Email { get; set; }

        public ICollection<HorarioFuncionamento> horarioFuncionamentos { get; set; }
    }


    public class  FormaPagamento {
        public int Id { get; set; }
        public string Descricao { get; set; }

    }
}
