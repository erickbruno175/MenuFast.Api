namespace MenuFast.Api.Api.Domain.Entities.Models.Funcionario {
    public class Permissao {
        public int Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public string Codigo {  get; set; } = string.Empty;
        public ICollection<PerfilPermissao> PerfilPermissoes { get; set; } = [ ];
    }
}
