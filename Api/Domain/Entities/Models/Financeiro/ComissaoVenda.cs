namespace MenuFast.Api.Api.Domain.Entities.Models.Financeiro {
    public class ComissaoVenda {
        public int Id { get; set; }
        public int FuncionarioId { get; set; }
        public int PedidoId { get; set; }
        public decimal ValorVenda { get; set; }
        public decimal PercentualComissao { get; set; }
        public decimal ValorComissao { get; set; }
        public DateTime DataVenda { get; set; }
        public Funcionario.Funcionario Funcionario { get; set; } = null!;
        public Pedido.Pedido Pedido { get; set; } = null!;
    }
}
