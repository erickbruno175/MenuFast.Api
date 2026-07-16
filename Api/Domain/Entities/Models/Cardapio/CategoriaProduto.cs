namespace MenuFast.Api.Api.Domain.Entities.Models.Cardapio {
    public class CategoriaProduto {
        public Guid Id { get; set; }
        public Guid CardapioId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public int Ordem { get; set; }
        public bool Ativo { get; set; } = true;
        public Cardapio? Cardapio { get; set; }
        public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
    }
}