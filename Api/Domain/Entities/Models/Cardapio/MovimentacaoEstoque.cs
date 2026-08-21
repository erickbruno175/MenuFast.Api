using MenuFast.Api.Api.Domain.Enum;

namespace MenuFast.Api.Api.Domain.Entities.Models.Cardapio {
    public class MovimentacaoEstoque {
        public int Id { get; set; }
        public int EstoqueProdutoId { get; set; }
        public EstoqueProduto EstoqueProduto { get; set; } = null!;
        public TipoMovimentacaoEstoque Tipo { get; set; }
        public int Quantidade { get; set; }
        public int QuantidadeAnterior { get; set; }
        public int QuantidadeAtual { get; set; }
        public string? Observacao { get; set; }
        public int? PedidoId { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}