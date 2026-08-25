namespace MenuFast.Api.Api.Application.DTOs.Request {
    public class DadosEmpresaRequest {
        public string RazaoSocial { get; set; } = string.Empty;
        public string NomeFantasia { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public string InscricaoEstadual { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
        public string Logradouro { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Uf { get; set; } = string.Empty;
        public string? Complemento { get; set; }
        public string Sigla { get; set; } = string.Empty;
        public string? WhatsApp { get; set; }
        public string? Site { get; set; }
        public string? Logo { get; set; }
        public IEnumerable<CadastrarHorarioFuncionamentoRequest> Horarios { get; set; }

    }
    public class CadastrarHorarioFuncionamentoRequest {
        public DayOfWeek DiaSemana { get; set; }
        public TimeSpan HoraAbertura { get; set; }
        public TimeSpan HoraFechamento { get; set; }
        public bool Fechado { get; set; }
    }
    public class CadastrarConfiguracaoLojaRequest {
        public bool TrabalhaComMesa { get; set; }
        public bool TrabalhaComDelivery { get; set; }
        public bool TrabalhaComRetirada { get; set; }
        public bool ControlaEstoque { get; set; }
        public bool PermiteVendaSemEstoque { get; set; }
        public bool CobraTaxaServico { get; set; }
        public decimal PercentualTaxaServico { get; set; }
        public bool ExigirGarcomNaMesa { get; set; }
        public bool EnviarPedidoAutomaticamenteCozinha { get; set; }
        public bool EnviarPedidoAutomaticamenteBar { get; internal set; }
        public bool AbilitarImpressoraTermica { get; set; } = false;
        public bool AbilitarKDS { get; set; } = false;
    }
}