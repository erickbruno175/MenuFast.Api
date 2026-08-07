using MenuFast.Api.Api.Domain.Entities.Models.Funcionario;
using MenuFast.Api.Api.Domain.Entities.Models.Loja;

namespace MenuFast.Api.Api.Application.DTOs.Request {
    public class CadastrarFuncionarioRequest {

        public string Nome { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;
        public bool PrimeiroAcesso { get; set; } = true;
        public bool Ativo { get; set; } = true;
        public DateTime DataAdmissao { get; set; }
        public decimal? Salario { get; set; }
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
        public int PerfilId { get; set; }
        public Perfil Perfil { get; set; } = null!;
        public int FuncaoId { get; set; }
        public int? ResponsavelId { get; set; } = null!;
        public int LojaId { get; set; }
        public Loja Loja { get; set; }
        public int TentativasLogin { get; set; }
        public bool Bloqueado { get; set; }
        public DateTime? DataBloqueio { get; set; }
        public DateTime? DataUltimoLogin { get; set; }

    }
}
