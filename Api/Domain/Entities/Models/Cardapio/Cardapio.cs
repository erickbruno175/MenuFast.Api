namespace MenuFast.Api.Api.Domain.Entities.Models.Cardapio {
    public class Cardapio {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public bool Ativo { get; set; } = true;
        public DateTime DataCadastro { get; set; } = DateTime.Now;
        public ICollection<CategoriaProduto> Categorias { get; set; } = new List<CategoriaProduto>();
    }
}
