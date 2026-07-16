namespace MenuFast.Api.Api.Domain.Entities.Models.Cardapio {
    public class OpcaoProduto {
        public Guid Id { get; set; }
        public Guid ProdutoId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal Acrescimo { get; set; }
        public Produto? Produto { get; set; }
    }
}
