namespace MenuFast.Api.Api.Domain.Entities.Models.Pedido {
    public class ItemPedido {
        public Guid Id { get; set; }
        public Guid PedidoId { get; set; }
        public Guid ProdutoId { get; set; }
        public decimal Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal Desconto { get; set; }
        public decimal Total { get; set; }
        public string? Observacao { get; set; }
        public Pedido Pedido { get; set; } = null!;
    }
}