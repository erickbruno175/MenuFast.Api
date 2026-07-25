namespace MenuFast.Api.Api.Domain.Entities.Models.ConfiguracoesEmpresa {
    public class ProvedorPagamento {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public bool Ativo { get; set; }
        public ICollection<FormaPagamento> FormasPagamento { get; set; } = new List<FormaPagamento>();
    }
}