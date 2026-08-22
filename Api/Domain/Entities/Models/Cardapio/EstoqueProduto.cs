namespace MenuFast.Api.Api.Domain.Entities.Models.Cardapio {
    public class EstoqueProduto {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; } = null!;
        public int Quantidade { get; set; }
        public int EstoqueMinimo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public bool AlertaEstoqueEnviado { get; set; }
        public DateTime UltimoAlertaEstoque { get; set; }
        public ICollection<MovimentacaoEstoque> Movimentacoe { get; set; }= new List<MovimentacaoEstoque>();
    }
}