using DocumentFormat.OpenXml.Drawing;

namespace MenuFast.Api.Api.Domain.Entities.Models.Cardapio {
    public class Produto {
        public Guid Id { get; set; }
        public Guid CategoriaProdutoId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }
        public decimal Custo { get; set; }
        public string? CodigoBarras { get; set; }
        public bool ControlaEstoque { get; set; }
        public bool Ativo { get; set; } = true;
        public string? FotoProduto { get; set; }
        public CategoriaProduto? CategoriaProduto { get; set; }
        public ICollection<Complemento> Complementos { get; set; } = new List<Complemento>();

    }
}