using MenuFast.Api.Api.Domain.Entities.Models.Funcionario;

namespace MenuFast.Api.Api.Domain.Entities.Models.Seguranca {
    public class TokenRedefinicaoSenha {
        public int Id { get; set; }
        public int FuncionarioId { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
        public DateTime DataExpiracao { get; set; }
        public bool Usado { get; set; }
        public Funcionario.Funcionario Funcionario { get; set; } = null!;
    }
}
