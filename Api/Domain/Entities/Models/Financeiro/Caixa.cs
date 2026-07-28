
using MenuFast.Api.Api.Domain.Entities.Models.Empresa;

namespace MenuFast.Api.Api.Domain.Entities.Models.Financeiro {
    public class Caixa {
        public int Id { get; set; }
        public int LojaId { get; set; }
        public Loja Loja { get; set; }
        public string Nome { get; set; } = string.Empty;
        public bool Aberto { get; set; }
        public decimal ValorAbertura { get; set; }
        public decimal ValorFechamento { get; set; }
        public DateTime? DataAbertura { get; set; }
        public DateTime? DataFechamento { get; set; }
        public ICollection<MovimentoCaixa> Movimentos { get; set; } = [ ];
        public int FuncioanrioId { get; set; }
        public Funcionario.Funcionario Funcionario{  get; set; }
    }
}
