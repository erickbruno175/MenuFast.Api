namespace MenuFast.Api.Api.Domain.Entities.Models.ConfiguracoesEmpresa {
    public class ConfiguracaoProvedorPagamento {
        public int Id { get; set; }
        public int EmpresaId { get; set; }
        public int ProvedorPagamentoId { get; set; }
        public string? ChaveApi { get; set; }
        public string? Token { get; set; }
        public string? SecretKey { get; set; }
        public bool Ativo { get; set; }
        public ProvedorPagamento ProvedorPagamento { get; set; } = null!;
    }
}
