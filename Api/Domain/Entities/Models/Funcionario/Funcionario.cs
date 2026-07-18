using MenuFast.Api.Api.Domain.Enum;

namespace MenuFast.Api.Api.Domain.Entities.Models.Funcionario {
    public class Funcionario {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;
        public bool PrimeiroAcesso { get; set; } = true;
        public bool Ativo { get; set; } = true;
        public DateTime DataAdmissao { get; set; }
        public decimal Salario { get; set; }
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
        public DateTime? UltimoLogin { get; set; }
        public Guid PerfilId { get; set; }
        public Perfil Perfil { get; set; } = null!;
        public Guid FuncaoId { get; set; }
        public Funcao Funcao { get; set; } = null!;
    }
}
