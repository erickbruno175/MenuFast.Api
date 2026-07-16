using MenuFast.Api.Domain.Entities.Models.Financeiro;

namespace MenuFast.Api.Api.Domain.Entities.Models.Pedido {
        public class PagamentoPedido {
            public Guid Id { get; set; }
            public Guid PedidoId { get; set; }
            public int FormaPagamentoId { get; set; }
            public FormaPagamento FormaPagamento { get; set; }
            public decimal Valor { get; set; }
            public DateTime DataPagamento { get; set; }
            public Pedido Pedido { get; set; } = null!;
        
    }
}