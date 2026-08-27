using MenuFast.Api.Api.Domain.Entities.Models.Cardapio;
using MenuFast.Api.Api.Domain.Entities.Models.Loja;

namespace MenuFast.Api.Api.Application.DTOs.Request {
    public class ProdutoRequest {
        public int CategoriaProdutoId { get; set; }
        public string Nome { get; set; }
        public int LojaId { get; set; }
        public decimal Preco { get; set; }
        public decimal Custo { get; set; }
        public bool ControlaEstoque { get; set; }
        public bool Ativo { get; set; } 
        public string? FotoProduto { get; set; }
        public string? Descricao { get; set; }
        public string? Codigo { get; set; }
        public int? QuantidadeEstoque { get; set; }
        public int? EstoqueMinimo { get; set; }
        public string? Tamanho { get; set; }
    }

    public class FiltroProdutoRequest {
        public string Nome { get; set; } = string.Empty;
        public int CategoriaId { get; set; }
        public bool Ativos { get; set; }
        public string Codigo { get; set; }

          
    }
 
    public class CategoriaRequest {
        public int CategoriaId { get; set; }
        public string Nome { get; set; }
        public int LojaId { get; set; }
    }
}
