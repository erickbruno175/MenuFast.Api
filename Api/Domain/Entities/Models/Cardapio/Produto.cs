using DocumentFormat.OpenXml.Drawing;
using MenuFast.Api.Api.Domain.Entities.Models.Empresa;

namespace MenuFast.Api.Api.Domain.Entities.Models.Cardapio {
    public class Produto {
        public int Id { get; set; }
        public int CategoriaProdutoId { get; set; }
        public int LojaId { get; set; }
        public Loja Loja { get; set; }
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