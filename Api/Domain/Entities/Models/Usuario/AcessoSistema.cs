namespace MenuFast.Api.Api.Domain.Entities.Models.Usuario;

public class AcessoSistema {
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public DateTime DataHora { get; set; }
    public string EnderecoIp { get; set; } = string.Empty;
    public string Dispositivo { get; set; } = string.Empty;
    public string SistemaOperacional { get; set; } = string.Empty;
    public string Navegador { get; set; } = string.Empty;
    public bool Sucesso { get; set; }
    public string? MotivoFalha { get; set; }
    public Usuario? Usuario { get; set; }
}