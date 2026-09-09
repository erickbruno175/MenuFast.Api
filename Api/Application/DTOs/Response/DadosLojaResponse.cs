namespace MenuFast.Api.Api.Application.DTOs.Response;

public class DadosLojaResponse {
    public int Id { get; set; }
    public bool Ativo { get; set; }
    public string RazaoSocial { get; set; } = string.Empty;
    public string NomeFantasia { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public string? InscricaoEstadual { get; set; }
    public string? Telefone { get; set; }
    public string? Email { get; set; }
    public string? Cep { get; set; }
    public string? Logradouro { get; set; }
    public string? Numero { get; set; }
    public string? Bairro { get; set; }
    public string? Cidade { get; set; }
    public string? Estado { get; set; }
    public string? Uf { get; set; }
    public string? Complemento { get; set; }
    public string? Sigla { get; set; }
    public string? WhatsApp { get; set; }
    public string? Site { get; set; }
    public string? Logo { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool ConfiguracaoFinalizada { get; set; }
}