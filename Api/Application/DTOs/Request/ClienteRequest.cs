namespace MenuFast.Api.Api.Application.DTOs.Request {
        public class ClienteRequest {
            public string Nome { get; set; } = string.Empty;
            public string CPF { get; set; } = string.Empty;
            public DateTime? DataNascimento { get; set; }
            public string Telefone { get; set; } = string.Empty;
            public string WhatsApp { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           
            public string CEP { get; set; } = string.Empty;
            public string Logradouro { get; set; } = string.Empty;
            public string Numero { get; set; } = string.Empty;
            public string Complemento { get; set; } = string.Empty;
            public string Bairro { get; set; } = string.Empty;
            public string Cidade { get; set; } = string.Empty;
            public string Estado { get; set; } = string.Empty;
            public string PontoReferencia { get; set; } = string.Empty;
            public string Observacao { get; set; } = string.Empty;
            public decimal Latitude { get; set; }
            public decimal Longitude { get; set; }
        
    }
}
