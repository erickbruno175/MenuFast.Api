namespace MenuFast.Api.Api.Domain.Entities.Models.ConfiguracoesEmpresa;

public class Impressora {
    public Guid Id { get; set; }
    public Guid TerminalId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public string EnderecoIp { get; set; } = string.Empty;
    public int Porta { get; set; }
    public bool Padrao { get; set; }
    public bool Ativa { get; set; } = true;
    public Terminal? Terminal { get; set; }
}