using MenuFast.Api.Api.Domain.Entities.Models.Loja;

namespace MenuFast.Api.Api.Domain.Entities.Models.ConfiguracoesLoja {
    public class ConfiguracaoProvedorPagamento {
        public int Id { get; set; }
        public int LojaId { get; set; }
        public Loja.Loja Loja { get; set; }
        public int ProvedorPagamentoId { get; set; }
        public string? ChaveApi { get; set; }
        public string? Token { get; set; }
        public string? SecretKey { get; set; }
        public bool Ativo { get; set; }
        public ProvedorPagamento ProvedorPagamento { get; set; } = null!;
    }
}
