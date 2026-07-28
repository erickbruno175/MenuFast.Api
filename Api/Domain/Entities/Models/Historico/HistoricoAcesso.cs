using MenuFast.Api.Api.Domain.Entities.Models.Empresa;
using MenuFast.Api.Api.Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MenuFast.Api.Api.Domain.Entities.Models.Seguranca;

public class HistoricoAcesso {
    [Key]
    public int Id { get; set; }
    public int FuncionarioId { get; set; }
    [ForeignKey(nameof(FuncionarioId))]
    public Funcionario.Funcionario Funcionario { get; set; }
    public DateTime DataLogin { get; set; }
    public DateTime? DataLogout { get; set; }
    public bool SessaoAtiva { get; set; }
    public string? Ip { get; set; }
    public string? Dispositivo { get; set; }
    public string? Token { get; set; }
    public TipoAcesso TipoAcesso { get; set; } = new TipoAcesso();
    public int LojaId { get; set; }
    public Loja Loja { get; set; }
}