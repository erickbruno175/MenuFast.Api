using MenuFast.Api.Api.Domain.Enum;
using MenuFast.Api.Api.Domain.Entities;
using MenuFast.Api.Api.Domain.Entities.Models.Loja;

namespace MenuFast.Api.Api.Domain.Entities.Models.Funcionario {
    public class Funcionario {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;
        public bool PrimeiroAcesso { get; set; } = true;
        public bool Ativo { get; set; } 
        public DateTime? DataAdmissao { get; set; } = DateTime.MinValue;
        public decimal? Salario { get; set; }
        public DateTime? DataCadastro { get; set; } = DateTime.UtcNow;
        public int? PerfilId { get; set; }
        public Perfil? Perfil { get; set; } = null!;
        public int? LojaId { get; set; }
        public Loja.Loja? Loja { get; set; }
        public int? TentativasLogin { get; set; }
        public bool? Bloqueado { get; set; }
        public DateTime? DataBloqueio { get; set; } = null;
        public DateTime? DataUltimoLogin { get; set; } = null;
        public DateTime? DataExpiracaoSenha { get; set; } = null;

        public decimal PercentualComissao { get; set; }

    }
}
