using MenuFast.Api.Api.Domain.Enum;

namespace MenuFast.Api.Api.Domain.Entities.Models.Pedido {
    public class Entrega {
        public Guid Id { get; set; }
        public Guid PedidoId { get; set; }
        public Guid? ClienteEnderecoId { get; set; }
        public Guid? MotoboyId { get; set; }
        public StatusPedido Status { get; set; }
        public decimal TaxaEntrega { get; set; }
        public DateTime? DataSaida { get; set; }
        public DateTime? DataEntrega { get; set; }
        public Pedido Pedido { get; set; } = null!;
        public Entregador? Entregador { get; set; }

    }
}