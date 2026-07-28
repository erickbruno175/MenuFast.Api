using MenuFast.Api.Api.Domain.Entities.Models.Empresa;

namespace MenuFast.Api.Api.Domain.Entities.Models.Cardapio {
    public class Complemento {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public bool Obrigatorio { get; set; }
        public bool Ativo { get; set; } = true;
        public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
        public int LojaId { get; set; }
        public Loja Loja { get; set; }
    }
}