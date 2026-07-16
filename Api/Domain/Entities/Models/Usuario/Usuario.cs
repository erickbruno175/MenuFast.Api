namespace MenuFast.Api.Api.Domain.Entities.Models.Usuario;

public class Usuario {
    public Guid Id { get; set; }
    public Guid FuncionarioId { get; set; }
    public Guid PerfilId { get; set; }
    public string Login { get; set; } = string.Empty;
    public string SenhaHash { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool Ativo { get; set; } = true;
    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
    public DateTime? UltimoAcesso { get; set; }
    public Funcionario? Funcionario { get; set; }
    public Perfil? Perfil { get; set; }
    public ICollection<AcessoSistema> Acessos { get; set; } = new List<AcessoSistema>();
}