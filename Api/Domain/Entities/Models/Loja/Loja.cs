using MenuFast.Api.Api.Domain.Entities.Models.ConfiguracoesLoja;

namespace MenuFast.Api.Api.Domain.Entities.Models.Loja;

public class Loja {
    public int Id { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string RazaoSocial { get; set; } = string.Empty;
    public string NomeFantasia { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public string InscricaoEstadual { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool Ativo { get; set; } = true;
    public string Cep { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string Logradouro { get; set; }
    public string Numero { get; set; }
    public string Sigla {  get; set; } = string.Empty;
    public string? Complemento { get; set; }
    public DateTime DataCadastro { get; set; }
    public string? Facebook { get; set; }
    public string? Instagram { get; set; }
    public string? WhatsApp { get; set; }
    public string? Site { get; set; }
    public string Uf { get; set; } = string.Empty;
    public string? Logo { get; set; }
    public bool ConfiguracaoFinalizada { get; set; } = false;
    public ConfiguracaoLoja? Configuracao { get; set; }
    public ICollection<HorarioFuncionamento> Horarios { get; set; } = new List<HorarioFuncionamento>();
    public ICollection<ContaBancaria> ContasBancarias { get; set; } = new List<ContaBancaria>();
    public DateTime DataAlteracao { get; internal set; }
}