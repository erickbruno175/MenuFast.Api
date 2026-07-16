namespace MenuFast.Api.Api.Domain.Entities.Models.Usuario;

public class Permissao {
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public ICollection<Perfil> Perfis { get; set; } = new List<Perfil>();
}