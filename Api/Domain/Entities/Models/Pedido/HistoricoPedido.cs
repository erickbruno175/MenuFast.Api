using MenuFast.Api.Api.Domain.Enum;

namespace MenuFast.Api.Api.Domain.Entities.Models.Pedido {
    public class HistoricoPedido {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public AcaoHistoricoPedido Acao { get; set; } 
        public string? Observacao { get; set; }
        public DateTime Data { get; set; }
        public Guid UsuarioId { get; set; }
        public Pedido Pedido { get; set; } = null!;
    }
}
