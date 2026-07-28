namespace MenuFast.Api.Api.Application.DTOs.Request {
    public class RequestDadosEmpresa {
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
        public string? Facebook { get; set; }
        public string? Instagram { get; set; }
        public string? WhatsApp { get; set; }
        public string? TikTok { get; set; }
        public string? YouTube { get; set; }
        public string? LinkedIn { get; set; }
        public string? Site { get; set; }
        public string? Logo { get; set; }
    }
}