using DocumentFormat.OpenXml.Drawing;
using MenuFast.Api.Api.Domain.Entities.Models.Loja;

namespace MenuFast.Api.Api.Domain.Entities.Models.Cardapio {
    public class Produto {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public int CategoriaProdutoId { get; set; }
        public bool ControlaEstoque { get; set; }
        public string? Descricao { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int LojaId { get; set; }
        public Loja.Loja Loja { get; set; } = null!;
        public decimal Preco { get; set; }
        public bool Ativo { get; set; }
        public string? FotoProduto { get; set; }
        public DateTime DataCadastro { get; set; }
        public CategoriaProduto? CategoriaProduto { get; set; }
        public EstoqueProduto? EstoqueProduto { get; set; }
       
    }
}