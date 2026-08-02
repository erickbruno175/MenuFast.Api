using MenuFast.Api.Api.Domain.Entities.Models.Loja;

namespace MenuFast.Api.Api.Domain.Entities.Models.Cardapio {
    public class CategoriaProduto {
        public int Id { get; set; }
        public int CardapioId { get; set; }
        public int LojaId { get; set; }
        public Loja.Loja Loja { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public int Ordem { get; set; }
        public bool Ativo { get; set; } = true;
        public Cardapio? Cardapio { get; set; }
        public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
    }
}