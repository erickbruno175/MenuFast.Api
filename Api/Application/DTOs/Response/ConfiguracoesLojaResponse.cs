using MenuFast.Api.Api.Domain.Entities.Models.ConfiguracoesLoja;

namespace MenuFast.Api.Api.Application.DTOs.Response {
    public class ConfiguracoesLojaResponse {

        public int Id { get; set; }
        public bool TrabalhaComMesa { get; set; } = false;
        public bool TrabalhaComDelivery { get; set; } = false;
        public bool TrabalhaComRetirada { get; set; } = false;
        public bool PermiteVendaSemEstoque { get; set; } = false;
        public bool CobraTaxaServico { get; set; } = false;
        public decimal PercentualTaxaServico { get; set; }
        public bool ExigirGarcomNaMesa { get; set; } = false;
        public bool ImprimirPedidoAutomaticamente { get; set; } = false;
        public bool EnviarPedidoAutomaticamenteCozinha { get; set; } = false;
        public bool EnviarPedidoAutomaticamenteBar { get; set; } = false;
        public bool Ativo { get; set; } = false;
        public string  RazaoSocial { get; set; }
        public string  Email { get; set; }

        public ICollection<HorarioFuncionamento> horarioFuncionamentos { get; set; }

    }
}
