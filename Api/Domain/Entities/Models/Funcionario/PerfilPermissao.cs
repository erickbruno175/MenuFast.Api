namespace MenuFast.Api.Api.Domain.Entities.Models.Funcionario {
    public class PerfilPermissao {
        public int PerfilId { get; set; }
        public Perfil Perfil { get; set; } = null!;
        public Guid PermissaoId { get; set; }
        public Permissao Permissao { get; set; } = null!;
    }
}