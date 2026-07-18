namespace MenuFast.Api.Api.Domain.Entities.Models.Funcionario {
    public class Perfil {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public bool Ativo { get; set; } = true;
        public ICollection<PerfilPermissao> PerfilPermissoes { get; set; } = [ ];
    }
}
