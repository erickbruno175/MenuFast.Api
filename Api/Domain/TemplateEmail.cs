using MenuFast.Api.Api.Domain.Entities.Models.Loja;

namespace MenuFast.Api.Api.Domain {
    public class TemplateEmail {
        public int Id { get; set; }
        public int LojaId { get; set; }
        public Loja Loja { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Assunto { get; set; } = string.Empty;
        public string Conteudo { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }
}
