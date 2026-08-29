using MenuFast.Api.Api.Domain.Entities.Models.Cardapio;

namespace MenuFast.Api.Api.Domain.Entities.Models.Pedido {
    public class ItemPedido {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public int ProdutoId { get; set; }
        public decimal Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal Desconto { get; set; }
        public decimal Total { get; set; }
        public string? Observacao { get; set; }
        public Pedido Pedido { get; set; } = null!;
        public Produto Produto { get; set; }

    }
}