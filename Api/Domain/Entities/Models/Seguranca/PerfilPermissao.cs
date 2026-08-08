using MenuFast.Api.Api.Domain.Entities.Models.Funcionario;

namespace MenuFast.Api.Api.Domain.Entities.Models.Seguranca {
    public class PerfilPermissao {
        public int PerfilId { get; set; }
        public Perfil Perfil { get; set; } = null!;
        public int PermissaoId { get; set; }
        public Permissao Permissao { get; set; } = null!;
        public bool Ativo { get; internal set; }
    }
}