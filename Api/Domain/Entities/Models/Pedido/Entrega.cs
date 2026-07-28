using MenuFast.Api.Api.Domain.Entities.Models.Empresa;
using MenuFast.Api.Api.Domain.Enum;

namespace MenuFast.Api.Api.Domain.Entities.Models.Pedido {
    public class Entrega {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public int FuncionarioId { get; set; }
        public int? ClienteEnderecoId { get; set; }
        public int? MotoboyId { get; set; }
        public StatusPedido Status { get; set; }
        public decimal TaxaEntrega { get; set; }
        public DateTime? DataSaida { get; set; }
        public DateTime? DataEntrega { get; set; }
        public Pedido Pedido { get; set; } = null!;
        public Entregador? Entregador { get; set; }
        public int LojaId { get; set; }
        public Loja Loja { get; set; }

    }
}