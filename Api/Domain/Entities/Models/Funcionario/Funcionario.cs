using MenuFast.Api.Api.Domain.Enum;
using MenuFast.Api.Api.Domain.Entities;
using MenuFast.Api.Api.Domain.Entities.Models.Empresa;

namespace MenuFast.Api.Api.Domain.Entities.Models.Funcionario {
    public class Funcionario {
        public int Id { get; set; }
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
        public int PerfilId { get; set; }
        public Perfil Perfil { get; set; } = null!;
        public int FuncaoId { get; set; }
        public Funcao Funcao { get; set; } = null!;
        public int? ResponsavelId { get; set; } = null!;
        public int? EmpresaId { get; set; } = null!;
        public Empresa.Empresa? Empresa { get; set; } = null!;
        public int TentativasLogin { get; set; }
        public bool Bloqueado { get; set; }
        public DateTime? DataBloqueio { get; set; }
        public DateTime? DataUltimoLogin { get; set; }

    }
}
