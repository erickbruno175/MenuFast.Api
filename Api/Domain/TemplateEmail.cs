using MenuFast.Api.Api.Domain.Entities.Models.Empresa;

namespace MenuFast.Api.Api.Domain {
    public class TemplateEmail {
        public int Id { get; set; }
        public int EmpresaId { get; set; }
        public Loja Empresa { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Assunto { get; set; } = string.Empty;
        public string Conteudo { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }
}
