namespace MenuFast.Api.Api.Domain.Entities.Models.ConfiguracoesEmpresa {
    public class ConfiguracaoProvedorPagamento {
        public Guid Id { get; set; }
        public Guid EmpresaId { get; set; }
        public Guid ProvedorPagamentoId { get; set; }
        public string? ChaveApi { get; set; }
        public string? Token { get; set; }
        public string? SecretKey { get; set; }
        public bool Ativo { get; set; }
        public ProvedorPagamento ProvedorPagamento { get; set; } = null!;
    }
}
