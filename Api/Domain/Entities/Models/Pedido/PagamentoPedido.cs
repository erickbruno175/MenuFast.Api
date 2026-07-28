using MenuFast.Api.Api.Domain.Entities.Models.ConfiguracoesEmpresa;

namespace MenuFast.Api.Api.Domain.Entities.Models.Pedido {
    public class PagamentoPedido {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public int FormaPagamentoId { get; set; }
        public FormaPagamento FormaPagamento { get; set; } = null!;
        public decimal Valor { get; set; }
        public DateTime DataPagamento { get; set; }
        public Pedido Pedido { get; set; } = null!;
       
    }
}